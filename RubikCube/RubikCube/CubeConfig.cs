using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RubikCube
{
    public class CubeConfig : ICloneable
    {
        string[,] right = new string[3, 3];
        string[,] left = new string[3, 3];
        string[,] up = new string[3, 3];
        string[,] down = new string[3, 3];
        string[,] front = new string[3, 3];
        string[,] back = new string[3, 3];
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
                    Debug.Write(front[j, i]);
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
            TurnAxis(ref right, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curUp = new string[3];
            string[] curBack = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = front[2, i];
                curUp[i] = up[2, i];
                curBack[i] = back[2, i];
                curDown[i] = down[2, i];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    front[2, i] = curDown[i];
                    up[2, i] = curFront[i];
                    back[2, i] = curUp[i];
                    down[2, i] = curBack[i];
                }
                else
                {
                    //sides turn
                    front[2, i] = curUp[i];
                    up[2, i] = curBack[i];
                    back[2, i] = curDown[i];
                    down[2, i] = curFront[i];
                }
            }
        }

        private void LeftTurn(bool isClockWise)
        {
            TurnAxis(ref left, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curUp = new string[3];
            string[] curBack = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = front[0, i];
                curUp[i] = up[0, i];
                curBack[i] = back[0, i];
                curDown[i] = down[0, i];
            }
            for (int i = 0; i < 3; i++)
            {
                if (!isClockWise)
                {
                    //sides turn
                    front[0, i] = curDown[i];
                    up[0, i] = curFront[i];
                    back[0, i] = curUp[i];
                    down[0, i] = curBack[i];
                }
                else
                {
                    //sides turn
                    front[0, i] = curUp[i];
                    up[0, i] = curBack[i];
                    back[0, i] = curDown[i];
                    down[0, i] = curFront[i];
                }
            }
        }

        private void FrontTurn(bool isClockWise)
        {
            TurnAxis(ref front, isClockWise);

            //sides declare
            string[] curLeft = new string[3];
            string[] curRight = new string[3];
            string[] curUp = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curLeft[i] = left[2, 2 - i];
                curRight[i] = right[0, 2 - i];
                curUp[i] = up[i, 2];
                curDown[i] = down[i, 0];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    left[2, i] = curDown[i];
                    down[i, 0] = curRight[i];
                    right[0, i] = curUp[i];
                    up[i, 2] = curLeft[i];
                }
                else
                {
                    //sides turn
                    left[2, i] = curUp[2 - i];
                    down[i, 0] = curLeft[2 - i];
                    right[0, i] = curDown[2 - i];
                    up[i, 2] = curRight[2 - i];
                }
            }
        }

        private void BackTurn(bool isClockWise)
        {
            TurnAxis(ref back, isClockWise);

            //sides declare
            string[] curLeft = new string[3];
            string[] curRight = new string[3];
            string[] curUp = new string[3];
            string[] curDown = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curLeft[i] = left[0, 2 - i];
                curRight[i] = right[2, 2 - i];
                curUp[i] = up[i, 0];
                curDown[i] = down[i, 2];
            }
            for (int i = 0; i < 3; i++)
            {
                if (!isClockWise)
                {
                    //sides turn
                    left[0, i] = curDown[i];
                    down[i, 2] = curRight[i];
                    right[2, i] = curUp[i];
                    up[i, 0] = curLeft[i];
                }
                else
                {
                    //sides turn
                    left[0, i] = curUp[2 - i];
                    down[i, 2] = curLeft[2 - i];
                    right[2, i] = curDown[2 - i];
                    up[i, 0] = curRight[2 - i];
                }
            }
        }

        private void UpTurn(bool isClockWise)
        {
            TurnAxis(ref up, isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curLeft = new string[3];
            string[] curBack = new string[3];
            string[] curRight = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = front[i, 0];
                curLeft[i] = left[i, 0];
                curBack[i] = back[i, 2];
                curRight[i] = right[i, 0];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    front[i, 0] = curRight[i];
                    left[i, 0] = curFront[i];
                    back[i, 2] = curLeft[2 - i];
                    right[i, 0] = curBack[2 - i];
                }
                else
                {
                    //sides turn
                    front[i, 0] = curLeft[i];
                    left[i, 0] = curBack[2 - i];
                    back[i, 2] = curRight[2 - i];
                    right[i, 0] = curFront[i];
                }
            }
        }

        private void DownTurn(bool isClockWise)
        {
            TurnAxis(ref down, !isClockWise);

            //four sides turn
            string[] curFront = new string[3];
            string[] curLeft = new string[3];
            string[] curBack = new string[3];
            string[] curRight = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = front[i, 2];
                curLeft[i] = left[i, 2];
                curBack[i] = back[i, 0];
                curRight[i] = right[i, 2];
            }
            for (int i = 0; i < 3; i++)
            {
                if (isClockWise)
                {
                    //sides turn
                    front[i, 2] = curRight[i];
                    left[i, 2] = curFront[i];
                    back[i, 0] = curLeft[2 - i];
                    right[i, 2] = curBack[2 - i];
                }
                else
                {
                    //sides turn
                    front[i, 2] = curLeft[i];
                    left[i, 2] = curBack[2 - i];
                    back[i, 0] = curRight[2 - i];
                    right[i, 2] = curFront[i];
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
                    up[i, j] = "Red";
                    down[i, j] = "Orange";
                    front[i, j] = "Green";
                    back[i, j] = "Blue";
                    right[i, j] = "Yellow";
                    left[i, j] = "White";
                }
            }
        }

        public List<string[,]> GetCubeState()
        {
            return new List<string[,]> { (string[,]) right.Clone(), (string[,]) left.Clone(), (string[,]) up.Clone(),
                (string[,]) down.Clone(), (string[,]) front.Clone(), (string[,]) back.Clone() };
        }

        public Tuple<Vector3, bool> CharToVector(string real)
        {
            bool isClockWise = true;
            if ((real.Contains("l")) || (real.Contains("L")))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Left, isClockWise);
            }
            if ((real.Contains("r")) || (real.Contains("R")))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Right, isClockWise);
            }
            if ((real.Contains("b")) || (real.Contains("B")))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Backward, isClockWise);
            }
            if ((real.Contains("f")) || (real.Contains("F")))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Forward, isClockWise);
            }
            if ((real.Contains("u")) || (real.Contains("U")))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Up, isClockWise);
            }
            if ((real.Contains("d")) || (real.Contains("D")))
            {
                if (real.Contains('I'))
                    isClockWise = false;
                return new Tuple<Vector3, bool>(Vector3.Down, isClockWise);
            }
            return new Tuple<Vector3, bool>(Vector3.Zero, false);
        }

        public void SetStates(List<string[,]> state)
        {
            right = state[0];
            left = state[1];
            up = state[2];
            down = state[3];
            front = state[4];
            back = state[5];
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
