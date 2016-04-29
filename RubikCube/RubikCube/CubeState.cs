using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RubikCube
{
    public class CubeState
    {
        //the array which represents the cube according to the mesh name
        readonly int[, ,] cubeArray = new int[3, 3, 3];

        /// <summary>
        /// initialize according to the original state
        /// </summary>
        public CubeState()
        {
            OriginalCubeState();
        }

        /// <summary>
        /// set each spot according to its original value
        /// </summary>
        public void OriginalCubeState()
        {
            cubeArray[0, 0, 0] = 14;
            cubeArray[0, 0, 1] = 5;
            cubeArray[0, 0, 2] = 19;
            cubeArray[0, 1, 0] = 2;
            cubeArray[0, 1, 1] = 13;
            cubeArray[0, 1, 2] = 0;
            cubeArray[0, 2, 0] = 4;
            cubeArray[0, 2, 1] = 16;
            cubeArray[0, 2, 2] = 12;

            cubeArray[1, 0, 0] = 15;
            cubeArray[1, 1, 0] = 1;
            cubeArray[1, 2, 0] = 7;
            cubeArray[1, 2, 1] = 25;
            cubeArray[1, 2, 2] = 9;
            cubeArray[1, 1, 2] = 3;
            cubeArray[1, 0, 2] = 24;
            cubeArray[1, 0, 1] = 10;

            cubeArray[2, 0, 0] = 6;
            cubeArray[2, 0, 1] = 8;
            cubeArray[2, 0, 2] = 22;
            cubeArray[2, 1, 0] = 18;
            cubeArray[2, 1, 1] = 21;
            cubeArray[2, 1, 2] = 11;
            cubeArray[2, 2, 0] = 17;
            cubeArray[2, 2, 1] = 20;
            cubeArray[2, 2, 2] = 23;
        }

        /// <summary>
        /// Get the cubies on a specific side
        /// </summary>
        /// <param name="side"></param>
        /// <param name="isClockWise"></param>
        /// <returns></returns>
        public int[] FindCubiesOnSide(Vector3 side, bool isClockWise)
        {
            //sets the limits of the loop
            int x = 0, y = 0, z = 0;
            int limitX = 0, limitY = 0, limitZ = 0;
            FindLimitsX(ref side, ref x, ref limitX);
            FindLimitsY(ref side, ref y, ref limitY);
            FindLimitsZ(ref side, ref z, ref limitZ);

            //create the return value
            List<int> returnValue = new List<int>();

            //add each cubie to the result
            for (; x < limitX; x++)
            {
                //change the z according to the vector given
                if (side.Z >= 0) z = 0;
                if (side.Z < 0) z = 2;
                for (; z < limitZ; z++)
                {
                    //change the y according to the vector given
                    if (side.Y <= 0) y = 0;
                    if (side.Y > 0) y = 2;

                    for (; y < limitY; y++)
                    {
                        //adds the value
                        returnValue.Add(cubeArray[x, y, z]);
                    }
                }
            }

            //Rotate(side,isClockWise);
            return returnValue.ToArray();
        }

        /// <summary>
        /// make the logical rotation
        /// </summary>
        /// <param name="side">side to rotate</param>
        /// <param name="isClockWise"></param>
        public void Rotate(Vector3 side, bool isClockWise)
        {
            //rotate each of the sides acording to its set of spots in the array,
            //and if its clockwise or not

            if (side == Vector3.Backward)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Forward)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = originalOuterNum;
                }
            }
            else if (side == Vector3.Up)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 2, 0];
                    cubeArray[1, 2, 0] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = cubeArray[1, 2, 2];
                    cubeArray[1, 2, 2] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Down)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[1, 0, 0];
                    cubeArray[1, 0, 0] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = cubeArray[1, 0, 2];
                    cubeArray[1, 0, 2] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Left)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[0, 1, 0];
                    cubeArray[0, 1, 0] = cubeArray[0, 0, 1];
                    cubeArray[0, 0, 1] = cubeArray[0, 1, 2];
                    cubeArray[0, 1, 2] = cubeArray[0, 2, 1];
                    cubeArray[0, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[0, 0, 0];
                    cubeArray[0, 0, 0] = cubeArray[0, 0, 2];
                    cubeArray[0, 0, 2] = cubeArray[0, 2, 2];
                    cubeArray[0, 2, 2] = cubeArray[0, 2, 0];
                    cubeArray[0, 2, 0] = originalOuterNum;
                }
            }
            else if (side == Vector3.Right)
            {
                if (isClockWise)
                {
                    int originalCenterNum = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = originalOuterNum;
                }
                else
                {
                    int originalCenterNum = cubeArray[2, 1, 0];
                    cubeArray[2, 1, 0] = cubeArray[2, 2, 1];
                    cubeArray[2, 2, 1] = cubeArray[2, 1, 2];
                    cubeArray[2, 1, 2] = cubeArray[2, 0, 1];
                    cubeArray[2, 0, 1] = originalCenterNum;
                    int originalOuterNum = cubeArray[2, 0, 0];
                    cubeArray[2, 0, 0] = cubeArray[2, 2, 0];
                    cubeArray[2, 2, 0] = cubeArray[2, 2, 2];
                    cubeArray[2, 2, 2] = cubeArray[2, 0, 2];
                    cubeArray[2, 0, 2] = originalOuterNum;
                }
            }
        }

        #region find limits

        /// <summary>
        /// gets the limits for x
        /// </summary>
        /// <param name="side"></param>
        /// <param name="x"></param>
        /// <param name="limitX"></param>
        private static void FindLimitsX(ref Vector3 side, ref int x, ref int limitX)
        {
            if (side.X == 0)
            {
                x = 0; limitX = 3;
            }
            if (side.X < 0)
            {
                x = 0; limitX = 1;
            }
            if (side.X > 0)
            {
                x = 2; limitX = 3;
            }
        }
        /// <summary>
        /// gets the limits of the y
        /// </summary>
        /// <param name="side"></param>
        /// <param name="y"></param>
        /// <param name="limitY"></param>
        private static void FindLimitsY(ref Vector3 side, ref int y, ref int limitY)
        {
            if (side.Y == 0)
            {
                y = 0; limitY = 3;
            }
            if (side.Y < 0)
            {
                y = 0; limitY = 1;
            }
            if (side.Y > 0)
            {
                y = 2; limitY = 3;
            }
        }
        /// <summary>
        /// gets the limits of the z
        /// </summary>
        /// <param name="side"></param>
        /// <param name="z"></param>
        /// <param name="limitZ"></param>
        private static void FindLimitsZ(ref Vector3 side, ref int z, ref int limitZ)
        {
            if (side.Z == 0)
            {
                z = 0; limitZ = 3;
            }
            if (side.Z < 0)
            {
                z = 2; limitZ = 3;
            }
            if (side.Z > 0)
            {
                z = 0; limitZ = 1;
            }
        }

        #endregion
    }
}
