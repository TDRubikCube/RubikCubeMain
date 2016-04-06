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
        List<string[,]> targetState;
        private static readonly List<string> AllMoves = new List<string> { "R", "L", "U", "D", "F", "B", "RI", "LI", "UI", "DI", "FI", "BI" };
        KeyboardState oldKeyboardState;
        public List<KeyValuePair<int, CubeConfig>> StatesToCheck = new List<KeyValuePair<int, CubeConfig>>();

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
                RunTree();
            currentState = CubeState.GetCubeState();
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
            List<string[,]> stateToCheck = new List<string[,]>();
            foreach (string move in AllMoves)
            {
                currentState = CubeState.GetCubeState();
                if (StatesToCheck.Count != 0)
                    stateToCheck = StatesToCheck[0].Value.GetCubeState();
                bool shouldAddToList = true;
                CubeConfig tempState = new CubeConfig();
                if (StatesToCheck.Count == 0)
                {
                    tempState.SetStates(currentState);
                }
                else
                {
                    tempState.SetStates(stateToCheck);
                }
                tempState.Rotate(move);
                foreach (var state in StatesToCheck)
                {
                    if (AreSame(state.Value, tempState))
                    {
                        shouldAddToList = false;
                        break;
                    }
                }
                if (shouldAddToList)
                    StatesToCheck.Add(new KeyValuePair<int, CubeConfig>(GetCubeValue(tempState), (CubeConfig)tempState.Clone()));
            }
            OrganizeTree();
            if (StatesToCheck[0].Key != 0)
                RunTree();
        }

        public void OrganizeTree()
        {
            StatesToCheck.Sort(Compare1);
        }

        static int Compare1(KeyValuePair<int, CubeConfig> a, KeyValuePair<int, CubeConfig> b)
        {
            return a.Key.CompareTo(b.Key);
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
    }
}
