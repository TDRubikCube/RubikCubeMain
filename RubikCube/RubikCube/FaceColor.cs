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
    class FaceColor
    {
        string[,] Right = new string[3, 3];
        string[,] Left = new string[3, 3];
        string[,] Up = new string[3, 3];
        string[,] Down = new string[3, 3];
        string[,] Front = new string[3, 3];
        string[,] Back = new string[3, 3];
        public FaceColor()
        {
            Initialize();
        }

        public void debug()
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
            //this is forward!!
            if (side == Vector3.Backward)
                FrontTurn(isClockWise);
        }

        public void RightTurn(bool isClockWise)
        {
            //four sides turn
            string[] curFront = new string[3];
            string[] curUp = new string[3];
            string[] curBack = new string[3];
            string[] curDown = new string[3];

            //axis turn
            string[] axisUp = new string[3];
            string[] axisRight = new string[3];
            string[] axisDown = new string[3];
            string[] axisLeft = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curFront[i] = Front[2, i];
                curUp[i] = Up[2, i];
                curBack[i] = Back[2, i];
                curDown[i] = Down[2, i];

                //axis init
                axisUp[i] = Right[i, 0];
                axisRight[i] = Right[2, i];
                axisDown[i] = Right[i, 2];
                axisLeft[i] = Right[0, i];

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

                    //axis turn
                    Right[i, 0] = axisLeft[2-i];
                    Right[2, i] = axisUp[i];
                    Right[i, 2] = axisRight[2-i];
                    Right[0, i] = axisDown[i];
                }
                else
                {
                    //sides turn
                    Front[2, i] = curUp[i];
                    Up[2, i] = curBack[i];
                    Back[2, i] = curDown[i];
                    Down[2, i] = curFront[i];

                    //axis turn
                    Right[i, 0] = axisRight[i];
                    Right[2, i] = axisDown[2-i];
                    Right[i, 2] = axisLeft[i];
                    Right[0, i] = axisUp[2-i];
                }
            }
        }

        public void FrontTurn(bool isClockWise)
        {
            //sides declare
            string[] curLeft = new string[3];
            string[] curRight = new string[3];
            string[] curUp = new string[3];
            string[] curDown = new string[3];

            //axis declare
            string[] axisUp = new string[3];
            string[] axisRight = new string[3];
            string[] axisDown = new string[3];
            string[] axisLeft = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                curLeft[i] = Left[2, 2 - i];
                curRight[i] = Right[0, 2 - i];
                curUp[i] = Up[i, 2];
                curDown[i] = Down[i, 0];

                //axis init 
                axisUp[i] = Front[i, 0];
                axisRight[i] = Front[2, i];
                axisDown[i] = Front[i, 2];
                axisLeft[i] = Front[0, i];
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

                    //axis turn
                    Front[i, 0] = axisLeft[2-i];
                    Front[2, i] = axisUp[i];
                    Front[i, 2] = axisRight[2-i];
                    Front[0, i] = axisDown[i];
                }
                else
                {
                    //sides turn
                    Left[2, i] = curUp[i];
                    Down[i, 0] = curLeft[i];
                    Right[0, i] = curDown[i];
                    Up[i, 2] = curRight[i];

                    //axis turn
                    Front[i, 0] = axisRight[i];
                    Front[2, i] = axisDown[2-i];
                    Front[i, 2] = axisLeft[i];
                    Front[0, i] = axisUp[2-i];
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
    }
}
