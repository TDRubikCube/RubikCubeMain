using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RubikCube
{
    /// <summary>
    /// Responsible for all the physical rotation of the cube
    /// </summary>
    public class Cube
    {
        #region class vars
        Model model; //The main model of the cube
        Clocks clocks = new Clocks(); //Creates a new clock to be used
        public CubeConfig CubeConfig; //Creates a config of the colors of the cube
        Matrix[] meshTransforms;
        readonly CubeState cubeState = new CubeState(); //Creates a new cube state
        readonly Random rand = new Random(); //Createsa new random parameter
        public List<Vector3> ScramblingVectors; //Makes a list of vectors for the scramble function
        #endregion

        #region normal vars
        public int Angle; //The angle of the current rotating side of the cube
        public int RotationSpeed = 10; //The speed of the rotation of the cube
        int howManyTurns; //How many turns have passed since the cube started rotating
        #endregion

        #region constructor
        /// <summary>
        /// Constract the cube
        /// </summary>
        public Cube()
        {
            //loads the cube in its original state
            meshTransforms = new Matrix[26];
            OriginalCubeDraw();
            ScramblingVectors = new List<Vector3>();
            model = null;
            CubeConfig = new CubeConfig();
        }

        #endregion

        #region void
        /// <summary>
        /// Scrambles the cube
        /// </summary>
        public void Scramble()
        {
            ScrambleResult = "";
            List<char> temp = new List<char>();
            //All posible turns for the scramble
            List<char> possibleTurns = new List<char> { 'R', 'L', 'U', 'D', 'F', 'B', 'I' };
            for (int i = 0; i < 25; i++)
            {
                char currentChar = possibleTurns[rand.Next(0, 6)];
                //Makes sure that neighbor turns won't cancle each other
                if (temp.Count >= 3)
                {
                    if (currentChar == temp[temp.Count - 1] && temp[temp.Count - 2] == temp[temp.Count - 3] && currentChar == temp[temp.Count - 2])
                    {
                        while (currentChar == temp[temp.Count - 1])
                        {
                            currentChar = possibleTurns[rand.Next(0, 6)];
                        }
                    }
                }
                temp.Add(currentChar);
                ScrambleResult += currentChar;
            }
        }
        /// <summary>
        /// Solves the cube, reseting it
        /// </summary>
        public void Reset()
        {
            //resets the cube state to its original state
            cubeState.OriginalCubeState();
            //Draws the original cube
            OriginalCubeDraw();
            //Clears all the vectors from the scrambling function, if the game is currently scrambling
            ScramblingVectors.Clear();
        }
        /// <summary>
        /// Draws the original cube, with all the sides pefectly placed
        /// </summary>
        private void OriginalCubeDraw()
        {
            for (int i = 0; i < 26; i++)
            {
                meshTransforms[i] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            }
        }
        /// <summary>
        /// Rotates the cube
        /// </summary>
        /// <param name="side">The side that should rotate</param>
        /// <param name="isClockWise">Sould it rotate it clockWise or counterClockWise</param>
        /// <param name="algOrder">The order of the algorithm</param>
        public void Rotate(Vector3 side, bool isClockWise, string algOrder)
        {
            Angle -= RotationSpeed; //Changes the angle of the side depending on the rotation speed
            float sidePosition = Main.CubieSize;
            float partOfRotation = 100f / RotationSpeed;
            
            //Send directions to RotateSide based on the side
            #region turn each side
            if (side == Vector3.Left)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, MathHelper.PiOver2 / partOfRotation, 1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                    else
                    {
                        RotateSide(i, -MathHelper.PiOver2 / partOfRotation, 1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                }
            }
            else if (side == Vector3.Right)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, -1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, -1.5f * sidePosition, -1.5f * sidePosition, 0, 'x');
                    }
                }

            }
            else if (side == Vector3.Up)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                }
            }
            else if (side == Vector3.Down)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, 0, 0, 'y');
                    }
                }
            }
            else if (side == Vector3.Forward)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (!isClockWise)
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, -1.5f * sidePosition, 'z');
                    }
                    else
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, 1.5f * sidePosition, 'z');
                    }
                }
            }
            else if (side == Vector3.Backward)
            {
                howManyTurns++;
                foreach (int i in cubeState.FindCubiesOnSide(side, isClockWise))
                {
                    if (!isClockWise)
                    {
                        RotateSide(i, MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, 1.5f * sidePosition, 'z');
                    }
                    else
                    {
                        RotateSide(i, -MathHelper.PiOver2 / 10f, 0, -1.5f * sidePosition, -1.5f * sidePosition, 'z');
                    }
                }
            }
            #endregion
           
            //When the program is done rotating
            if (howManyTurns == (110 - RotationSpeed) / 10)
            {
                //rotate the colors
                CubeConfig.Rotate(side,isClockWise);
                
                //logical rotation of the cube
                cubeState.Rotate(side, isClockWise);
               
                //reset the turn counter
                howManyTurns = 0;
            }
        }
        /// <summary>
        /// Decides what side to roatate and rotates it
        /// </summary>
        /// <param name="i"></param>
        /// <param name="angle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="rotationAxis"></param>
        private void RotateSide(int i, float angle, float x, float y, float z, char rotationAxis)
        {
            if (rotationAxis == 'x')
            {
                //Moves the cube to the axis, rotates and moves back
                MeshTransforms[i] *= Matrix.CreateTranslation(new Vector3(x, y, z)) * Matrix.CreateRotationX(angle) * Matrix.CreateTranslation(new Vector3(-x, -y, -z));
            }
            else if (rotationAxis == 'y')
            {
                //Moves the cube to the axis, rotates and moves back
                MeshTransforms[i] *= Matrix.CreateTranslation(new Vector3(x, y, z)) * Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(new Vector3(-x, -y, -z));
            }
            else if (rotationAxis == 'z')
            {
                //Moves the cube to the axis, rotates and moves back
                MeshTransforms[i] *= Matrix.CreateTranslation(new Vector3(x, y, z)) * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(new Vector3(-x, -y, -z));
            }

        }
        #endregion

        #region model & matrix
        /// <summary>
        /// Gets and sets the model
        /// </summary>
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        /// <summary>
        /// Dedines each of the meshes of the cube, in a form of a matrix
        /// </summary>
        public Matrix[] MeshTransforms
        {
            get { return meshTransforms; }
            set { meshTransforms = value; }
        }
        #endregion
        /// <summary>
        /// Gets and sets the results of the scramble
        /// </summary>
        public string ScrambleResult { get; set; }
        /// <summary>
        /// Gets and sets if the game should add the order of scramble to algOrder
        /// </summary>
        public bool ShouldAddScrambleToOrder { get; set; }

    }
}
