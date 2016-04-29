using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RubikCube
{
    public class CubeConfig : ICloneable
    {
        //defines each of the faces according to their colors (the names are relative to when the green face is the front)
        string[,] right = new string[3, 3];
        string[,] left = new string[3, 3];
        string[,] up = new string[3, 3];
        string[,] down = new string[3, 3];
        string[,] front = new string[3, 3];
        string[,] back = new string[3, 3];

        /// <summary>
        /// initialize the faces with their default color
        /// </summary>
        public CubeConfig()
        {
            Initialize();
        }

        /// <summary>
        /// Send to the relevent rotations according to the vector recieved
        /// </summary>
        /// <param name="side">side to rotate in a form of a vecotr</param>
        /// <param name="isClockWise"></param>
        public void Rotate(Vector3 side, bool isClockWise)
        {
            //rotate the right side
            if (side == Vector3.Right)
                RightTurn(isClockWise);

            //rotate the left side
            else if (side == Vector3.Left)
                LeftTurn(isClockWise);

            //rotate the front face (since they are opposite)
            else if (side == Vector3.Backward)
                FrontTurn(isClockWise);

            //rotate the back face (since they are opposite)
            else if (side == Vector3.Forward)
                BackTurn(isClockWise);

            //rotate the up side
            else if (side == Vector3.Up)
                UpTurn(isClockWise);

            //rotate the dows side
            else if (side == Vector3.Down)
                DownTurn(isClockWise);
        }

        /// <summary>
        ///Send to the relevent rotations according to the vector recieved
        /// </summary>
        /// <param name="move">side to rotate in a form of a command</param>
        public void Rotate(string move)
        {
            //translate to a vector
            Vector3 side = CharToVector(move).Item1;
            //checks if clickwise
            bool isClockWise = CharToVector(move).Item2;

            //rotate the right side
            if (side == Vector3.Right)
                RightTurn(isClockWise);

            //rotate the left side
            else if (side == Vector3.Left)
                LeftTurn(isClockWise);

            //rotate the front face (since they are opposite)
            else if (side == Vector3.Backward)
                FrontTurn(isClockWise);

            //rotate the back face (since they are opposite)
            else if (side == Vector3.Forward)
                BackTurn(isClockWise);

            //rotate the up side
            else if (side == Vector3.Up)
                UpTurn(isClockWise);

            //rotate the dows side
            else if (side == Vector3.Down)
                DownTurn(isClockWise);
        }

        #region each rotation
        /// <summary>
        /// turn the right side
        /// </summary>
        /// <param name="isClockWise"></param>
        private void RightTurn(bool isClockWise)
        {
            //turn the axis on which the face rotates
            TurnAxis(ref right, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curUp = new string[3];
            string[] curBack = new string[3];
            string[] curDown = new string[3];

            //initialize the temp sides
            for (int i = 0; i < 3; i++)
            {
                //define each temp as a part of a face which turns
                curFront[i] = front[2, i];
                curUp[i] = up[2, i];
                curBack[i] = back[2, i];
                curDown[i] = down[2, i];
            }

            //turn the sides
            for (int i = 0; i < 3; i++)
            {
                //rotate clockwise 
                if (isClockWise)
                {
                    // down to front
                    front[2, i] = curDown[i];
                    //front to up
                    up[2, i] = curFront[i];
                    //up to back
                    back[2, i] = curUp[i];
                    //back to down
                    down[2, i] = curBack[i];
                }
                else
                {
                    //up to front
                    front[2, i] = curUp[i];
                    //back to up
                    up[2, i] = curBack[i];
                    //dow to back
                    back[2, i] = curDown[i];
                    //front to down
                    down[2, i] = curFront[i];
                }
            }
        }

        /// <summary>
        /// turn the left side
        /// </summary>
        /// <param name="isClockWise"></param>
        private void LeftTurn(bool isClockWise)
        {
            //turn the axis on which the face rotates
            TurnAxis(ref left, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curUp = new string[3];
            string[] curBack = new string[3];
            string[] curDown = new string[3];

            //initialize the four temp sides
            for (int i = 0; i < 3; i++)
            {
                //sets each temp as a part of a face
                curFront[i] = front[0, i];
                curUp[i] = up[0, i];
                curBack[i] = back[0, i];
                curDown[i] = down[0, i];
            }
            for (int i = 0; i < 3; i++)
            {
                //if isnt clockwise
                if (!isClockWise)
                {
                    //down to front
                    front[0, i] = curDown[i];
                    //front to up
                    up[0, i] = curFront[i];
                    //up to back
                    back[0, i] = curUp[i];
                    //back to down
                    down[0, i] = curBack[i];
                }
                else
                {
                    //up to front
                    front[0, i] = curUp[i];
                    //back to up
                    up[0, i] = curBack[i];
                    //down to back
                    back[0, i] = curDown[i];
                    //front to down
                    down[0, i] = curFront[i];
                }
            }
        }

        /// <summary>
        /// rotate the front side
        /// </summary>
        /// <param name="isClockWise"></param>
        private void FrontTurn(bool isClockWise)
        {
            //turn the axis on which the face rotates
            TurnAxis(ref front, isClockWise);

            //sides declare
            string[] curLeft = new string[3];
            string[] curRight = new string[3];
            string[] curUp = new string[3];
            string[] curDown = new string[3];

            //initialize the four temp sides
            for (int i = 0; i < 3; i++)
            {
                //sets each temp as a part of a face
                curLeft[i] = left[2, 2 - i];
                curRight[i] = right[0, 2 - i];
                curUp[i] = up[i, 2];
                curDown[i] = down[i, 0];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //down to left
                    left[2, i] = curDown[i];
                    //right to down                    
                    down[i, 0] = curRight[i];
                    //up to right                    
                    right[0, i] = curUp[i];
                    //lert to up                    
                    up[i, 2] = curLeft[i];
                }
                else
                {
                    //up to left  
                    left[2, i] = curUp[2 - i];
                    //left to down
                    down[i, 0] = curLeft[2 - i];
                    //down to right
                    right[0, i] = curDown[2 - i];
                    //right to up
                    up[i, 2] = curRight[2 - i];
                }
            }
        }

        /// <summary>
        /// rotate the back side
        /// </summary>
        /// <param name="isClockWise"></param>
        private void BackTurn(bool isClockWise)
        {
            //turn the axis on which the face rotates
            TurnAxis(ref back, isClockWise);

            //sides declare
            string[] curLeft = new string[3];
            string[] curRight = new string[3];
            string[] curUp = new string[3];
            string[] curDown = new string[3];

            //initialize the four temp sides
            for (int i = 0; i < 3; i++)
            {
                //sets each temp as a part of a face
                curLeft[i] = left[0, 2 - i];
                curRight[i] = right[2, 2 - i];
                curUp[i] = up[i, 0];
                curDown[i] = down[i, 2];
            }
            for (int i = 0; i < 3; i++)
            {
                //if isnt clockwise
                if (!isClockWise)
                {
                    //down to left
                    left[0, i] = curDown[i];
                    //right to down
                    down[i, 2] = curRight[i];
                    //up to right
                    right[2, i] = curUp[i];
                    //left to up
                    up[i, 0] = curLeft[i];
                }
                else
                {
                    //up to left
                    left[0, i] = curUp[2 - i];
                    //left to down
                    down[i, 2] = curLeft[2 - i];
                    //down to right
                    right[2, i] = curDown[2 - i];
                    //right to up
                    up[i, 0] = curRight[2 - i];
                }
            }
        }

        /// <summary>
        /// rotate the up side
        /// </summary>
        /// <param name="isClockWise"></param>
        private void UpTurn(bool isClockWise)
        {
            //turn the axis on which the face rotates
            TurnAxis(ref up, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curLeft = new string[3];
            string[] curBack = new string[3];
            string[] curRight = new string[3];

            //initialize the four temp sides
            for (int i = 0; i < 3; i++)
            {
                //sets each temp as a part of a face
                curFront[i] = front[i, 0];
                curLeft[i] = left[i, 0];
                curBack[i] = back[i, 2];
                curRight[i] = right[i, 0];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //right to front
                    front[i, 0] = curRight[i];
                    //front to left
                    left[i, 0] = curFront[i];
                    //left to back
                    back[i, 2] = curLeft[2 - i];
                    //back to right
                    right[i, 0] = curBack[2 - i];
                }
                else
                {
                    //left to front
                    front[i, 0] = curLeft[i];
                    //back to left
                    left[i, 0] = curBack[2 - i];
                    //right to back
                    back[i, 2] = curRight[2 - i];
                    //front to right
                    right[i, 0] = curFront[i];
                }
            }
        }

        private void DownTurn(bool isClockWise)
        {
            //turn the axis on which the face rotates
            TurnAxis(ref down, !isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curLeft = new string[3];
            string[] curBack = new string[3];
            string[] curRight = new string[3];

            //initialize the four temp sides
            for (int i = 0; i < 3; i++)
            {
                //sets each temp as a part of a face
                curFront[i] = front[i, 2];
                curLeft[i] = left[i, 2];
                curBack[i] = back[i, 0];
                curRight[i] = right[i, 2];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //right to front
                    front[i, 2] = curRight[i];
                    //front to left
                    left[i, 2] = curFront[i];
                    //left to back
                    back[i, 0] = curLeft[2 - i];
                    //back to right
                    right[i, 2] = curBack[2 - i];
                }
                else
                {
                    //left to front
                    front[i, 2] = curLeft[i];
                    //back to left
                    left[i, 2] = curBack[2 - i];
                    //right to back
                    back[i, 0] = curRight[2 - i];
                    //front to right
                    right[i, 2] = curFront[i];
                }
            }
        }
        #endregion

        /// <summary>
        /// turn the axis on which the face rotates
        /// </summary>
        /// <param name="side">teh side to rotate</param>
        /// <param name="isClockWise"></param>
        private void TurnAxis(ref string[,] side, bool isClockWise)
        {
            //initialize the four temp sides
            string[] axisLeft = new string[3];
            string[] axisUp = new string[3];
            string[] axisRight = new string[3];
            string[] axisDown = new string[3];

            //marks whether the part has already changed
            bool[,] axisUsed = new bool[3, 3];

            //initialize the four temp sides
            for (int i = 0; i < 3; i++)
            {
                //sets each temp as a part of the face
                axisUp[i] = side[i, 0];
                axisRight[i] = side[2, i];
                axisDown[i] = side[i, 2];
                axisLeft[i] = side[0, i];
            }

            //makes the rotation
            for (int i = 0; i < 3; i++)
            {
                //checks if it hasnt changed yet, if it didnt  rotate and mark as changed
                if (!axisUsed[i, 0])
                {
                    side[i, 0] = isClockWise ? axisLeft[2 - i] : axisRight[i];
                    axisUsed[i, 0] = true;
                }
                //checks if it hasnt changed yet, if it didnt  rotate and mark as changed
                if (!axisUsed[2, i])
                {
                    side[2, i] = isClockWise ? axisUp[i] : axisDown[2 - i];
                    axisUsed[2, i] = true;
                }
                //checks if it hasnt changed yet, if it didnt  rotate and mark as changed
                if (!axisUsed[i, 2])
                {
                    side[i, 2] = isClockWise ? axisRight[2 - i] : axisLeft[i];
                    axisUsed[i, 2] = true;
                }
                //checks if it hasnt changed yet, if it didnt  rotate and mark as changed
                if (!axisUsed[0, i])
                {
                    side[0, i] = isClockWise ? axisDown[i] : axisUp[2 - i];
                    axisUsed[0, i] = true;
                }
            }
        }

        /// <summary>
        /// initialzie the colors as the initial state of each of them
        /// </summary>
        void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    up[i, j] = "Red";
                    down[i, j] = "Orange";
                    front[i, j] = "Green";
                    back[i, j] = "Blue";
                    right[i, j] = "Yellow";
                    left[i, j] = "White";
                }
            }
        }

        /// <summary>
        /// returns all of the faces of the current cube
        /// </summary>
        /// <returns></returns>
        public List<string[,]> GetCubeState()
        {
            return new List<string[,]> { (string[,]) right.Clone(), (string[,]) left.Clone(), (string[,]) up.Clone(),
                (string[,]) down.Clone(), (string[,]) front.Clone(), (string[,]) back.Clone() };
        }

        /// <summary>
        /// translate char to a vector
        /// </summary>
        /// <param name="real">the origianl char</param>
        /// <returns>vector and isclockwise</returns>
        public Tuple<Vector3, bool> CharToVector(string real)
        {
            bool isClockWise = true;
            //if there is a "L" 
            if ((real.Contains("l")) || (real.Contains("L")))
            {
                //if there is an i than mark clockwise as false
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Left, isClockWise);
            }
            //if there is a "R" 
            if ((real.Contains("r")) || (real.Contains("R")))
            {
                //if there is an i than mark clockwise as false
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Right, isClockWise);
            }
            //if there is a "B" 

            if ((real.Contains("b")) || (real.Contains("B")))
            {
                //if there is an i than mark clockwise as false
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Backward, isClockWise);
            }
            //if there is a "F" 
            if ((real.Contains("f")) || (real.Contains("F")))
            {
                //if there is an i than mark clockwise as false
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Forward, isClockWise);
            }
            //if there is a "U" 
            if ((real.Contains("u")) || (real.Contains("U")))
            {
                //if there is an i than mark clockwise as false
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Up, isClockWise);
            }
            //if there is a "D" 
            if ((real.Contains("d")) || (real.Contains("D")))
            {
                //if there is an i than mark clockwise as false
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Down, isClockWise);
            }
            return new Tuple<Vector3, bool>(Vector3.Zero, false);
        }

        /// <summary>
        /// gets a state of a cube and set it as the current state
        /// </summary>
        /// <param name="state"></param>
        public void SetStates(List<string[,]> state)
        {
            right = state[0];
            left = state[1];
            up = state[2];
            down = state[3];
            front = state[4];
            back = state[5];
        }

        /// <summary>
        /// make a new instance of the cube
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
