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
                    Debug.Write(Up[j, i]);
                    if(temp == 3)
                        Debug.WriteLine("");
                }
                temp = 0;
            }
            Debug.WriteLine("");
        }

        public void Rotate(Vector3 side, bool isClockWise)
        {
            //Debug.WriteLine(Front[2,0]);
            if (side == Vector3.Right)
                RightTurn(isClockWise, 2, ref Front, ref Up, ref Back, ref Down,ref Right);
            if (side == Vector3.Left)
                RightTurn(isClockWise, 0, ref Front, ref Down, ref Back, ref Up,ref Left);
            //this is forward!!
            if (side == Vector3.Backward)
                RightTurn(isClockWise, 2, ref Left, ref Up, ref Right, ref Down,ref Front);
            //this is backward!!
            if (side == Vector3.Forward)
                RightTurn(isClockWise, 0, ref Left, ref Down, ref Right, ref Up,ref Back);
        }

        public void RightTurn(bool isClockWise, int b, ref string[,] relFront, ref string[,] relUp, ref string[,] relBack, ref string[,] relDown,ref string[,] axis)
        {
            Debug.WriteLine(" ");
            Debug.WriteLine("Before");
            for (int i = 0; i < 3; i++)
            {
                Debug.Write(relFront[b, i] + " ");
            }
            Debug.WriteLine(" ");

            //four sides turn
            string[] first = new string[3];
            string[] second = new string[3];
            string[] third = new string[3];
            string[] fourth = new string[3];

            //axis turn
            string[] axisUp = new string[3];
            string[] axisRight = new string[3];
            string[] axisDown = new string[3];
            string[] axisLeft = new string[3];

            for (int i = 0; i < 3; i++)
            {
                //sides init
                first[i] = relFront[b, i];
                second[i] = relUp[b, i];
                third[i] = relBack[b, i];
                fourth[i] = relDown[b, i];

                //axis init
                axisUp[i] = axis[i, 0];
                axisRight[i] = axis[2, i];
                axisDown[i] = axis[i, 2];
                axisLeft[i] = axis[0, i];

                if (isClockWise)
                {
                    //sides turn
                    relFront[b, i] = fourth[i];
                    relUp[b, i] = first[i];
                    relBack[b, i] = second[i];
                    relDown[b, i] = third[i];

                    //axis turn
                    if (i < 2)
                    {
                        axis[i, 0] = axisLeft[i];
                        axis[2, i] = axisUp[i];
                        axis[i, 2] = axisRight[i];
                        axis[0, i] = axisDown[i];
                    }
                }
                else
                {
                    //sides turn
                    relFront[b, i] = second[i];
                    relUp[b, i] = third[i];
                    relBack[b, i] = fourth[i];
                    relDown[b, i] = first[i];

                    //axis turn
                    if (i < 2)
                    {
                        axis[i, 0] = axisRight[i];
                        axis[2, i] = axisDown[i];
                        axis[i, 2] = axisLeft[i];
                        axis[0, i] = axisUp[i];
                    }
                }
            }
            Debug.WriteLine("After");
            for (int i = 0; i < 3; i++)
            {
                Debug.Write(relFront[b, i] + " ");
            }
            Debug.WriteLine("");

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
