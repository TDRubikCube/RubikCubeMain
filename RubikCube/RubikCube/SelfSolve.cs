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
        CubeConfig VirtualState;
        Cube cube;
        List<string[,]> currentState;
        List<string[,]> randomState;
        List<string[,]> oldState;
        const List<string> AllMoves = new List<string> { "R", "L", "U", "D", "F", "B", "RI", "LI", "UI", "DI", "FI", "BI" };
        KeyboardState oldKeyboardState;

        public SelfSolve(Cube _cube)
        {
            CubeState = new CubeConfig();
            cube = _cube;
        }

        public void Update()
        {
            currentState = CubeState.GetCubeState();
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyUp(Keys.A) && oldKeyboardState.IsKeyDown(Keys.A))
            {
                ShouldScramble = true;
                oldState = CubeState.GetCubeState();
            }
            if (keyboardState.IsKeyUp(Keys.B) && oldKeyboardState.IsKeyDown(Keys.B))
            {
                randomState = CubeState.GetCubeState();
                AreSame(oldState, randomState);
            }
            oldKeyboardState = keyboardState;
        }

        public bool ShouldScramble { get; set; }

        public bool ShouldCompare { get; set; }

        public bool AreSame(List<string[,]> first, List<string[,]> second)
        {
            for (int i = 0; i < 6; i++)
            {
                string[,] firstState = first[i];
                string[,] secondState = second[i];
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
            VirtualState.SetStates(currentState);
            foreach (string move in AllMoves)
            {

            }
        }
    }
}
