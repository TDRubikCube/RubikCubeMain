using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace RubikCube
{
    class SelfSolve
    {
        CubeConfig CubeState;
        Cube cube;
        List<string[,]> userState;
        List<string[,]> targetState;
        private static Random rng = new Random();
        private bool isFirstTime;
        private static readonly List<string> AllMoves = new List<string> { "R", "L", "U", "D", "F", "B", "RI", "LI", "UI", "DI", "FI", "BI" };
        KeyboardState oldKeyboardState;
        public List<Tuple<string, KeyValuePair<int, CubeConfig>>> StatesToCheck = new List<Tuple<string, KeyValuePair<int, CubeConfig>>>();
        public static List<Tuple<string, KeyValuePair<int, CubeConfig>>> FirstCheckRun = new List<Tuple<string, KeyValuePair<int, CubeConfig>>>();
        public List<KeyValuePair<int, CubeConfig>> StatesChecked = new List<KeyValuePair<int, CubeConfig>>();

        public SelfSolve(Cube _cube)
        {
            cube = _cube;
            CubeState = cube.cubeConfig;
            targetState = CubeState.GetCubeState();
        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D1) && oldKeyboardState.IsKeyUp(Keys.D1))
            {
                isFirstTime = true;
                StatesChecked.Clear();
                StatesToCheck.Clear();
                RunTree();
            }
            if (keyboardState.IsKeyDown(Keys.D2) && oldKeyboardState.IsKeyUp(Keys.D2))
            {
                Debug.WriteLine(GetCubeValue(CubeState));
            }
            userState = CubeState.GetCubeState();
            oldKeyboardState = keyboardState;
        }

        public bool ShouldScramble { get; set; }

        public bool ShouldCompare { get; set; }

        public bool AreSame(CubeConfig first, CubeConfig second)
        {
            for (int i = 0; i < 6; i++)
            {
                string[,] firstState = first.GetCubeState()[i];
                string[,] secondState = second.GetCubeState()[i];
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (firstState[x, y] != secondState[x, y])
                            return false;
                    }
                }
            }
            return true;
        }

        public void RunTree()
        {
            //declerations
            string lastMove = "";
            List<string[,]> currentBaseState = new List<string[,]>();
            if (StatesToCheck.Count != 0)
                currentBaseState = StatesToCheck[0].Item2.Value.GetCubeState();

            //shuffle move list
            Shuffle(AllMoves);

            //start tree
            foreach (string move in AllMoves)
            {
                userState = CubeState.GetCubeState();
                bool shouldAddToList = true;
                CubeConfig tempState = new CubeConfig();

                //assign tempState the current base state than rotate it
                if (isFirstTime)
                {
                    currentBaseState = userState;
                    tempState.SetStates(currentBaseState);
                }
                else if (FirstCheckRun.Count == 0)
                {
                    currentBaseState = StatesToCheck[0].Item2.Value.GetCubeState();
                    tempState.SetStates(currentBaseState);
                }
                else
                {
                    currentBaseState = FirstCheckRun[0].Item2.Value.GetCubeState();
                    tempState.SetStates(currentBaseState);
                }
                tempState.Rotate(move);

                //check for same states
                foreach (var state in StatesChecked)
                {
                    if (AreSame(state.Value, tempState))
                    {
                        shouldAddToList = false;
                        break;
                    }
                }
                foreach (var state in StatesToCheck)
                {
                    if (AreSame(state.Item2.Value, tempState))
                    {
                        shouldAddToList = false;
                        break;
                    }
                }

                //add to StatesToCheck list
                if (shouldAddToList)
                {
                    string prvMoves = "";
                    if (!isFirstTime)
                    {
                        if (FirstCheckRun.Count == 0)
                            prvMoves = StatesToCheck[0].Item1;
                        else
                            prvMoves = FirstCheckRun[0].Item1;
                    }
                    StatesToCheck.Add(new Tuple<string, KeyValuePair<int, CubeConfig>>(prvMoves + move, new KeyValuePair<int, CubeConfig>(GetCubeValue(tempState),
                        (CubeConfig)tempState.Clone())));
                }
                lastMove = move;
            }


            //add to checked list
            CubeConfig stateChecked = new CubeConfig();
            stateChecked.SetStates(currentBaseState);
            if (lastMove.Contains("I"))
                stateChecked.Rotate(lastMove[0].ToString());
            else
                stateChecked.Rotate(lastMove + "I");
            StatesChecked.Add(new KeyValuePair<int, CubeConfig>(GetCubeValue(stateChecked), (CubeConfig)stateChecked.Clone()));

            //remove from need to check list
            if (!isFirstTime && FirstCheckRun.Count == 0)
            {
                StatesToCheck.RemoveAt(0);
            }

            //sort by value
            OrganizeTree();

            if (isFirstTime)
            {
                foreach (var t in StatesToCheck)
                {
                    FirstCheckRun.Add(t);
                }
                StatesToCheck.Clear();
            }
            else if (FirstCheckRun.Count > 0)
            {
                FirstCheckRun.RemoveAt(0);
            }


            //run again if needed (added a few if's to prevent index out of range)
            if (StatesToCheck.Count < 5000)
            {
                if (isFirstTime)
                {
                    isFirstTime = false;
                    RunTree();
                }
                if (StatesToCheck.Count != 0)
                {
                    if (StatesToCheck[0].Item2.Key != 0)
                        RunTree();
                }
            }
        }

        public void OrganizeTree()
        {
            StatesToCheck.Sort(Compare1);
        }

        static int Compare1(Tuple<string, KeyValuePair<int, CubeConfig>> a, Tuple<string, KeyValuePair<int, CubeConfig>> b)
        {
            return a.Item2.Key.CompareTo(b.Item2.Key);
        }

        public int GetCubeValue(CubeConfig cubeState)
        {
            var state = cubeState.GetCubeState();
            int value = 0;
            for (int i = 0; i < state.Count; i++)
            {
                string[,] currentFace = state[i];
                string[,] targetFace = targetState[i];
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (currentFace[x, y] != targetFace[x, y])
                            value++;
                    }
                }
            }
            return value;
        }

        public static void Shuffle(List<string> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
