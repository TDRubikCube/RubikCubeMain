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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RubikCube
{
    public class CubeConfig
    {
        string[,] Right = new string[3, 3];
        string[,] Left = new string[3, 3];
        string[,] Up = new string[3, 3];
        string[,] Down = new string[3, 3];
        string[,] Front = new string[3, 3];
        string[,] Back = new string[3, 3];
        public CubeConfig()
        {
            Initialize();
        }

        public void CheckFaceColor()
        {
            Debug.WriteLine("");
            int temp = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp++;
                    Debug.Write(Front[j, i]);
                    if (temp == 3)
                        Debug.WriteLine("");
                }
                temp = 0;
            }
            Debug.WriteLine("");
        }

        public void Rotate(Vector3 side, bool isClockWise)
        {
            if (side == Vector3.Right)
                RightTurn(isClockWise);
            if (side == Vector3.Left)
                LeftTurn(isClockWise);
            //this is forward!!
            if (side == Vector3.Backward)
                FrontTurn(isClockWise);
            //this is backward!
            if (side == Vector3.Forward)
                BackTurn(isClockWise);
            if (side == Vector3.Up)
                UpTurn(isClockWise);
            if (side == Vector3.Down)
                DownTurn(isClockWise);
        }

        public void Rotate(string move)
        {
            Vector3 side = CharToVector(move).Item1;
            bool isClockWise = CharToVector(move).Item2;
            if (side == Vector3.Right)
                RightTurn(isClockWise);
            if (side == Vector3.Left)
                LeftTurn(isClockWise);
            //this is forward!!
            if (side == Vector3.Backward)
                FrontTurn(isClockWise);
            //this is backward!
            if (side == Vector3.Forward)
                BackTurn(isClockWise);
            if (side == Vector3.Up)
                UpTurn(isClockWise);
            if (side == Vector3.Down)
                DownTurn(isClockWise);
        }

        private void RightTurn(bool isClockWise)
        {
            TurnAxis(ref Right, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curUp = new string[3];
            string[] curBack = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = Front[2, i];
                curUp[i] = Up[2, i];
                curBack[i] = Back[2, i];
                curDown[i] = Down[2, i];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    Front[2, i] = curDown[i];
                    Up[2, i] = curFront[i];
                    Back[2, i] = curUp[i];
                    Down[2, i] = curBack[i];
                }
                else
                {
                    //sides turn
                    Front[2, i] = curUp[i];
                    Up[2, i] = curBack[i];
                    Back[2, i] = curDown[i];
                    Down[2, i] = curFront[i];
                }
            }
        }

        private void LeftTurn(bool isClockWise)
        {
            TurnAxis(ref Left, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curUp = new string[3];
            string[] curBack = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = Front[0, i];
                curUp[i] = Up[0, i];
                curBack[i] = Back[0, i];
                curDown[i] = Down[0, i];
            }
            for (int i = 0; i < 3; i++)
            {
                if (!isClockWise)
                {
                    //sides turn
                    Front[0, i] = curDown[i];
                    Up[0, i] = curFront[i];
                    Back[0, i] = curUp[i];
                    Down[0, i] = curBack[i];
                }
                else
                {
                    //sides turn
                    Front[0, i] = curUp[i];
                    Up[0, i] = curBack[i];
                    Back[0, i] = curDown[i];
                    Down[0, i] = curFront[i];
                }
            }
        }

        private void FrontTurn(bool isClockWise)
        {
            TurnAxis(ref Front, isClockWise);

            //sides declare
            string[] curLeft = new string[3];
            string[] curRight = new string[3];
            string[] curUp = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curLeft[i] = Left[2, 2 - i];
                curRight[i] = Right[0, 2 - i];
                curUp[i] = Up[i, 2];
                curDown[i] = Down[i, 0];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    Left[2, i] = curDown[i];
                    Down[i, 0] = curRight[i];
                    Right[0, i] = curUp[i];
                    Up[i, 2] = curLeft[i];
                }
                else
                {
                    //sides turn
                    Left[2, i] = curUp[2 - i];
                    Down[i, 0] = curLeft[2 - i];
                    Right[0, i] = curDown[2 - i];
                    Up[i, 2] = curRight[2 - i];
                }
            }
        }

        private void BackTurn(bool isClockWise)
        {
            TurnAxis(ref Back, isClockWise);

            //sides declare
            string[] curLeft = new string[3];
            string[] curRight = new string[3];
            string[] curUp = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curLeft[i] = Left[0, 2 - i];
                curRight[i] = Right[2, 2 - i];
                curUp[i] = Up[i, 0];
                curDown[i] = Down[i, 2];
            }
            for (int i = 0; i < 3; i++)
            {
                if (!isClockWise)
                {
                    //sides turn
                    Left[0, i] = curDown[i];
                    Down[i, 2] = curRight[i];
                    Right[2, i] = curUp[i];
                    Up[i, 0] = curLeft[i];
                }
                else
                {
                    //sides turn
                    Left[0, i] = curUp[2 - i];
                    Down[i, 2] = curLeft[2 - i];
                    Right[2, i] = curDown[2 - i];
                    Up[i, 0] = curRight[2 - i];
                }
            }
        }

        private void UpTurn(bool isClockWise)
        {
            TurnAxis(ref Up, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curLeft = new string[3];
            string[] curBack = new string[3];
            string[] curRight = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = Front[i, 0];
                curLeft[i] = Left[i, 0];
                curBack[i] = Back[i, 2];
                curRight[i] = Right[i, 0];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    Front[i, 0] = curRight[i];
                    Left[i, 0] = curFront[i];
                    Back[i, 2] = curLeft[2 - i];
                    Right[i, 0] = curBack[2 - i];
                }
                else
                {
                    //sides turn
                    Front[i, 0] = curLeft[i];
                    Left[i, 0] = curBack[2 - i];
                    Back[i, 2] = curRight[2 - i];
                    Right[i, 0] = curFront[i];
                }
            }
        }

        private void DownTurn(bool isClockWise)
        {
            TurnAxis(ref Down, !isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curLeft = new string[3];
            string[] curBack = new string[3];
            string[] curRight = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = Front[i, 2];
                curLeft[i] = Left[i, 2];
                curBack[i] = Back[i, 0];
                curRight[i] = Right[i, 2];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    Front[i, 2] = curRight[i];
                    Left[i, 2] = curFront[i];
                    Back[i, 0] = curLeft[2 - i];
                    Right[i, 2] = curBack[2 - i];
                }
                else
                {
                    //sides turn
                    Front[i, 2] = curLeft[i];
                    Left[i, 2] = curBack[2 - i];
                    Back[i, 0] = curRight[2 - i];
                    Right[i, 2] = curFront[i];
                }
            }
        }

        private void TurnAxis(ref string[,] side, bool isClockWise)
        {
            string[] axisLeft = new string[3];
            string[] axisUp = new string[3];
            string[] axisRight = new string[3];
            string[] axisDown = new string[3];
            bool[,] axisUsed = new bool[3, 3];

            for (int i = 0; i < 3; i++)
            {
                //axis init
                axisUp[i] = side[i, 0];
                axisRight[i] = side[2, i];
                axisDown[i] = side[i, 2];
                axisLeft[i] = side[0, i];
            }
            for (int i = 0; i < 3; i++)
            {
                if (!axisUsed[i, 0])
                {
                    side[i, 0] = isClockWise ? axisLeft[2 - i] : axisRight[i];
                    axisUsed[i, 0] = true;
                }
                if (!axisUsed[2, i])
                {
                    side[2, i] = isClockWise ? axisUp[i] : axisDown[2 - i];
                    axisUsed[2, i] = true;
                }
                if (!axisUsed[i, 2])
                {
                    side[i, 2] = isClockWise ? axisRight[2 - i] : axisLeft[i];
                    axisUsed[i, 2] = true;
                }
                if (!axisUsed[0, i])
                {
                    side[0, i] = isClockWise ? axisDown[i] : axisUp[2 - i];
                    axisUsed[0, i] = true;
                }
            }
        }

        void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Up[i, j] = "Red";
                    Down[i, j] = "Orange";
                    Front[i, j] = "Green";
                    Back[i, j] = "Blue";
                    Right[i, j] = "Yellow";
                    Left[i, j] = "White";
                }
            }
        }

        public List<string[,]> GetCubeState()
        {
            return new List<string[,]> { Right, Left, Up, Down, Front, Back };
        }

        public Tuple<Vector3, bool> CharToVector(string real)
        {
            bool isClockWise = true;
            if ((real == "l") || (real == "L"))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Left, isClockWise);
            }
            if ((real == "r") || (real == "R"))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Right, isClockWise);
            }
            if ((real == "b") || (real == "B"))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Backward, isClockWise);
            }
            if ((real == "f") || (real == "F"))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Forward, isClockWise);
            }
            if ((real == "u") || (real == "U"))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Up, isClockWise);
            }
            if ((real == "d") || (real == "D"))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Down, isClockWise);
            }
            return new Tuple<Vector3, bool>(Vector3.Zero, false);
        }

        public void SetStates(List<string[,]> state)
        {
            Right = state[0];
            Left = state[1];
            Up = state[2];
            Down = state[3];
            Front = state[4];
            Back = state[5];
        }
    }
}
