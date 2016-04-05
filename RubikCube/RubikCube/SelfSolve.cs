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
        List<string[,]> currentState;
        List<string[,]> randomState;
        List<string[,]> oldState;
        private static readonly List<string> AllMoves = new List<string> { "R", "L", "U", "D", "F", "B", "RI", "LI", "UI", "DI", "FI", "BI" };
        KeyboardState oldKeyboardState;
        public Queue<CubeConfig> StatesToCheck = new Queue<CubeConfig>();

        public SelfSolve(Cube _cube)
        {
            cube = _cube;
            CubeState = cube.cubeConfig;
        }

        public void Update()
        {
            currentState = CubeState.GetCubeState();
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A))
                RunTree();
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
            var temp = currentState;
            foreach (string move in AllMoves)
            {
                bool shouldAddToList = true;
                CubeConfig tempState = new CubeConfig();
                tempState.SetStates(currentState);
                tempState.Rotate(move);
                foreach (var state in StatesToCheck)
                {
                    if (AreSame(state.GetCubeState(), tempState.GetCubeState()))
                    {
                        shouldAddToList = false;
                        break;
                    }
                }
                if (shouldAddToList)
                    StatesToCheck.Enqueue(tempState);
            }
            CubeState.SetStates(temp);
        }
    }
}
