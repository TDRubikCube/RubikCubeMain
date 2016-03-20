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

        void RightTurn()
        {
            for (int i = 0; i < 3; i++)
			{
                string[] greenTemp = new string[3];
                greenTemp[i] = Front[2, i];
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
