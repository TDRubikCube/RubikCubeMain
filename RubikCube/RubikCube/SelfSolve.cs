using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace RubikCube
{
    class SelfSolve
    {
        //craete a new cube
        readonly CubeConfig cubeState;

        //create a new instance of the cube class
        readonly Cube cube;

        //the state of the cube which the user sees
        List<string[,]> userState;

        //the wanted state of the cube
        readonly List<string[,]> targetState;

        //randon, required to shuffle the moves list
        private static readonly Random Rng = new Random();

        //marks if its the first time running the tree
        private bool isFirstTime;

        //list of all the moves possible
        private static readonly List<string> AllMoves = new List<string> { "R", "L", "U", "D", "F", "B", "RI", "LI", "UI", "DI", "FI", "BI" };

        //old keyboard state
        KeyboardState oldKeyboardState;

        // a list of the cubes and their value, with the moves needed to get to that state
        public List<Tuple<string, KeyValuePair<int, CubeConfig>>> StatesToCheck = new List<Tuple<string, KeyValuePair<int, CubeConfig>>>();

        //same list as above, but just for the first run (needed to run the 12 moves on all the 12 first states)
        public static List<Tuple<string, KeyValuePair<int, CubeConfig>>> FirstCheckRun = new List<Tuple<string, KeyValuePair<int, CubeConfig>>>();

        //a list of cube and their value, which were already checked
        public List<KeyValuePair<int, CubeConfig>> StatesChecked = new List<KeyValuePair<int, CubeConfig>>();

        /// <summary>
        /// the constructor of the self solve. potientially it will solve the cube, given its state
        /// </summary>
        /// <param name="_cube">the cube instance from switchGameState</param>
        public SelfSolve(Cube _cube)
        {
            //set the cube as the one from SwitchGameState
            cube = _cube;

            //the current state of the cube (the one the user sees)
            cubeState = cube.CubeConfig;

            //the solved state
            targetState = cubeState.GetCubeState();
        }

        /// <summary>
        /// the main update which allows to call the tree
        /// </summary>
        public void Update()
        {
            //get the current keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            //if the user clicked on the number "1" in the keyboard, call the tree (a randomly chosen key)
            if (keyboardState.IsKeyDown(Keys.D1) && oldKeyboardState.IsKeyUp(Keys.D1))
            {
                //marks this is the start of the tree
                isFirstTime = true;

                //clear all the lists
                StatesChecked.Clear();
                StatesToCheck.Clear();

                //activate the tree
                RunTree();
            }

            //set the user state as the current one (updates the user state every frame)
            userState = cubeState.GetCubeState();

            //sets the old keyboard state
            oldKeyboardState = keyboardState;
        }

        /// <summary>
        /// checks if two cubes are the same
        /// </summary>
        /// <param name="first">cube 1</param>
        /// <param name="second">cube 2</param>
        /// <returns>are they the same</returns>
        public bool AreSame(CubeConfig first, CubeConfig second)
        {
            for (int i = 0; i < 6; i++)
            {
                //set temp vars as each of the faces of each state
                string[,] firstState = first.GetCubeState()[i];
                string[,] secondState = second.GetCubeState()[i];
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        //if the colors are not the same, return false
                        if (firstState[x, y] != secondState[x, y])
                            return false;
                    }
                }
            }
            //if all were the same, return true
            return true;
        }

        /// <summary>
        /// the main tree which solves the cube
        /// </summary>
        public void RunTree()
        {
            //the last move made
            string lastMove = "";

            // set the current base to make moves on
            List<string[,]> currentBaseState = new List<string[,]>();

            //if it already ran before, and there are states to check, set the base state as the first state on the list
            if (StatesToCheck.Count != 0)
                currentBaseState = StatesToCheck[0].Item2.Value.GetCubeState();

            //shuffle move list
            Shuffle(AllMoves);

            //start tree
            foreach (string move in AllMoves)
            {
                //refresh the user state
                userState = cubeState.GetCubeState();

                //marks if should the cube to the list of cubes to check
                bool shouldAddToList = true;

                //the current state made rotations on
                CubeConfig tempState = new CubeConfig();

                //assign tempState the current base state than rotate it
                if (isFirstTime)
                {
                    //set the base state as the user state (since its the first run)
                    currentBaseState = userState;

                    //set the temp state as the base state
                    tempState.SetStates(currentBaseState);
                }
                //if it finished checking the list of first 12 states
                else if (FirstCheckRun.Count == 0)
                {
                    //set the base state as the first state on the list
                    currentBaseState = StatesToCheck[0].Item2.Value.GetCubeState();

                    //set the temp state as the base state
                    tempState.SetStates(currentBaseState);
                }
                //if it ran once, but didnt finish checking the first 12 states
                else
                {
                    //set the base state as the first of the 12 states
                    currentBaseState = FirstCheckRun[0].Item2.Value.GetCubeState();

                    //set the temp state as the base state
                    tempState.SetStates(currentBaseState);
                }

                //rotate the temp state to create the new state
                tempState.Rotate(move);

                //check for same states
                foreach (var state in StatesChecked)
                {
                    //if the new state is the same as a state that was already checked
                    if (AreSame(state.Value, tempState))
                    {
                        //dont add him to the list and exit the loop
                        shouldAddToList = false;
                        break;
                    }
                }
                //check if the list of moves to check has the new state in it
                foreach (var state in StatesToCheck)
                {
                    //if the states are the same
                    if (AreSame(state.Item2.Value, tempState))
                    {
                        //dont add to the list and exit the loop
                        shouldAddToList = false;
                        break;
                    }
                }

                //add to StatesToCheck list
                if (shouldAddToList)
                {
                    //create a "list" of moves made to get to the current state
                    string prvMoves = "";

                    //if there were already moved made, add them to said list
                    if (!isFirstTime)
                    {
                        //if it finished runing on the first 12 states, get the moves from the statesToCheck list
                        if (FirstCheckRun.Count == 0)
                            prvMoves = StatesToCheck[0].Item1;
                        else // if it didnt, get it from the list of first 12 states
                            prvMoves = FirstCheckRun[0].Item1;
                    }

                    //add the new state 
                    StatesToCheck.Add(new Tuple<string, KeyValuePair<int, CubeConfig>>(prvMoves + move, // the list of moves
                        new KeyValuePair<int, CubeConfig>(GetCubeValue(tempState), // the value of the state
                        (CubeConfig)tempState.Clone()))); // the state itself
                }
                //set the last move as the last move made by the loop
                lastMove = move;
            }


            //create a state to add to the states that were checked
            CubeConfig stateChecked = new CubeConfig();

            //get the current statae
            stateChecked.SetStates(currentBaseState);

            //make the counter of the last move made on him (to return it to this base form)
            if (lastMove.Contains("I"))
                stateChecked.Rotate(lastMove[0].ToString());
            else
                stateChecked.Rotate(lastMove + "I");

            //add it to the checked list
            StatesChecked.Add(new KeyValuePair<int, CubeConfig>(GetCubeValue(stateChecked), (CubeConfig)stateChecked.Clone()));

            //remove from need to check list
            if (!isFirstTime && FirstCheckRun.Count == 0)
            {
                StatesToCheck.RemoveAt(0);
            }

            //sort by value
            OrganizeTree();

            //if its the first run of the tree
            if (isFirstTime)
            {
                //add all the 12 first states to the list, of the 12 first states
                foreach (var t in StatesToCheck)
                {
                    FirstCheckRun.Add(t);
                }
                //clear the list of states needed to check (since we copied it the other list)
                StatesToCheck.Clear();
            }
            //if its not the first run, and the first 12 states list isnt empty yet
            else if (FirstCheckRun.Count > 0)
            {
                //remove the first state in it
                FirstCheckRun.RemoveAt(0);
            }


            //run again if needed (added a few if's to prevent index out of range), and added a limit of 5000 so it wont run forever if it fails
            if (StatesToCheck.Count < 5000)
            {
                //if its the first run of the tree
                if (isFirstTime)
                {
                    //mark its not the first run anymore, and run it again
                    isFirstTime = false;
                    RunTree();
                }
                //if there are states to check
                if (StatesToCheck.Count != 0)
                {
                    //if there isnt a state which is solved, run the tree again
                    if (StatesToCheck[0].Item2.Key != 0)
                        RunTree();
                }
            }
        }

        /// <summary>
        /// sort the tree
        /// </summary>
        public void OrganizeTree()
        {
            StatesToCheck.Sort(Compare1);
        }

        /// <summary>
        /// sort the list according to the value of the state
        /// </summary>
        /// <param name="a">first cube</param>
        /// <param name="b">second cube</param>
        /// <returns></returns>
        static int Compare1(Tuple<string, KeyValuePair<int, CubeConfig>> a, Tuple<string, KeyValuePair<int, CubeConfig>> b)
        {
            return a.Item2.Key.CompareTo(b.Item2.Key);
        }

        /// <summary>
        /// gets the value of the cube according to the mis-matches of the colors in every face, compared to the color that were supposed to be there
        /// </summary>
        /// <param name="cubeState">the state to evalute</param>
        /// <returns></returns>
        public int GetCubeValue(CubeConfig cubeState)
        {
            //get the current state of the cube
            var state = cubeState.GetCubeState();

            //create the value that will be returned
            int value = 0;

            //check each face for mis-matches
            for (int i = 0; i < state.Count; i++)
            {
                //create temp vars of each of the faces of each state
                string[,] currentFace = state[i];
                string[,] targetFace = targetState[i];
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        //in case of a mis-match, add a point
                        if (currentFace[x, y] != targetFace[x, y])
                            value++;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// shuffle the list
        /// </summary>
        /// <param name="list">list to shuffle</param>
        public static void Shuffle(List<string> list)
        {
            //number of items in the list
            int n = list.Count;

            //run for all the list
            while (n > 1)
            {
                //decrease the limit
                n--;

                //create a random number
                int k = Rng.Next(n + 1);

                //get a random value from the list in the k place
                var value = list[k];

                //replace the random value with the value in the n place
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}