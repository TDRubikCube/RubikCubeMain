using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RubikCube
{

    class Camera
    {
        #region define vars
        //the previous location of the mouse
        Vector2 previousMousePosition;

        //the old state of the mouse
        public MouseState OldMouseState;

        //the view matrix, which is the camera itself
        private Matrix view;

        //the horizontal angle of the camera, withing the sphere
        float horizontalAngle;

        //the vertical angle of the camera within the sphere
        float verticalAngle;

        //the previous value of the mouse wheel
        int previousMouseWheel;

        //the radius of the sphere which contains the camera
        float radius = 27.32f;
        #endregion

        /// <summary>
        /// the logic of the camera and its function
        /// </summary>
        public Camera()
        {
            //the camera itself
            view = Matrix.CreateLookAt(new Vector3(20, 20, 20), new Vector3(0, 0, 0), Vector3.Up);

            //defines the initial vertical angle according to the camera's location 
            verticalAngle = AngleBetweenVectorAndAxis(Matrix.Invert(view).Translation, "x");

            //defines the initial horizontal angle according to the camera's location 
            horizontalAngle = AngleBetweenVectorAndAxis(Matrix.Invert(view).Translation, "y");
        }

        /// <summary>
        /// the main update of the camera
        /// </summary>
        public void Update()
        {
            OldMouseState = Mouse.GetState();
            previousMouseWheel = OldMouseState.ScrollWheelValue;
        }

        /// <summary>
        /// sets the relative vector3 vars according to the camera position
        /// </summary>
        /// <param name="cameraPos">the camera position as a vector3</param>
        public void RealRotate(Vector3 cameraPos)
        {
            //sets according to when the green face is the front
            if ((IsFaceGreen(cameraPos)))
            {
                RealLeft = Vector3.Left;
                RealRight = Vector3.Right;
                RealForward = Vector3.Backward;
                RealBackward = Vector3.Forward;
            }
            //sets according to when the yellow face is the front
            else if (IsFaceYellow(cameraPos))
            {
                RealLeft = Vector3.Backward;
                RealRight = Vector3.Forward;
                RealForward = Vector3.Right;
                RealBackward = Vector3.Left;
            }
            //sets according to when the blue face is the front
            else if (IsFaceBlue(cameraPos))
            {
                RealLeft = Vector3.Right;
                RealRight = Vector3.Left;
                RealForward = Vector3.Forward;
                RealBackward = Vector3.Backward;
            }
            //sets according to when the white face is the 
            else if (IsFaceWhite(cameraPos))
            {
                RealLeft = Vector3.Forward;
                RealRight = Vector3.Backward;
                RealForward = Vector3.Left;
                RealBackward = Vector3.Right;
            }
        }

        /// <summary>
        /// checks if the white face is the front face
        /// </summary>
        /// <param name="cameraPos"></param>
        /// <returns></returns>
        public bool IsFaceWhite(Vector3 cameraPos)
        {
            return cameraPos.X >= -radius * 1.012f && cameraPos.X <= -radius / 1.52f && cameraPos.Z >= -radius / 1.52f && cameraPos.Z <= radius / 1.52f;
        }

        /// <summary>
        /// checks if the blue face is the front face
        /// </summary>
        /// <param name="cameraPos"></param>
        /// <returns></returns>
        public bool IsFaceBlue(Vector3 cameraPos)
        {
            return cameraPos.X >= -radius / 1.366f && cameraPos.X <= radius / 1.366f && cameraPos.Z >= -radius * 1.012f && cameraPos.Z <= -radius / 1.52f;
        }

        /// <summary>
        /// checks if the yelow face is the front face
        /// </summary>
        /// <param name="cameraPos"></param>
        /// <returns></returns>
        public bool IsFaceYellow(Vector3 cameraPos)
        {
            return cameraPos.X >= radius / 1.366f && cameraPos.X <= radius * 1.012f && cameraPos.Z >= -radius / 1.366f && cameraPos.Z <= radius / 1.366f;
        }

        /// <summary>
        /// checks if the green face is the front face
        /// </summary>
        /// <param name="cameraPos"></param>
        /// <returns></returns>
        public bool IsFaceGreen(Vector3 cameraPos)
        {
            return cameraPos.X >= -radius / 1.61f && cameraPos.X <= radius / 1.61f && cameraPos.Z > radius / 1.438f && cameraPos.Z <= radius * 1.012f;
        }

        /// <summary>
        /// makes the camera move using the mouse
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="oldMouseState"></param>
        public void CameraMovement(MouseState mouseState, MouseState oldMouseState)
        {
            //gets the current mouse postion
            Vector2 currentMousePos = new Vector2(mouseState.X, mouseState.Y);

            //gets the old mouse position
            previousMousePosition = new Vector2(oldMouseState.X, oldMouseState.Y);

            //addes to the horizontal angle proportionally to the horizontal movement of the mouse
            horizontalAngle += (currentMousePos.X - previousMousePosition.X) * 0.01f;

            //addes to the vertical angle proportionally to the vertical movement of the mouse
            verticalAngle += (currentMousePos.Y - previousMousePosition.Y) * 0.01f;

            //sets the camera position according to 3d trigonometry of a sphere
            Vector3 newCameraPosition = new Vector3(
               -(float)(radius * Math.Sin(horizontalAngle) * Math.Sin(verticalAngle)),  //x
                (float)(radius * Math.Cos(verticalAngle)),                              //y
                (float)(radius * Math.Sin(verticalAngle) * Math.Cos(horizontalAngle))); //z

            //checks whether should allow the user to zoom out
            if (mouseState.ScrollWheelValue < previousMouseWheel && radius < 107.32)
            {
                radius += 2;
            }
            //checks whether should allow the user to zoom in
            else if (mouseState.ScrollWheelValue > previousMouseWheel && radius > 17.33)
            {
                radius -= 2;
            }
            view = Matrix.CreateLookAt(newCameraPosition, new Vector3(0, 0, 0), Vector3.Up);
        }

        /// <summary>
        /// the angle between the camera and the axis requested
        /// </summary>
        /// <param name="vector">positon of camera</param>
        /// <param name="constant">axis requested</param>
        /// <returns>angle</returns>
        private float AngleBetweenVectorAndAxis(Vector3 vector, string constant)
        {
            //angle to the x axis
            if (constant == "x" && !vector.Z.Equals(0))
            {
                return MathHelper.ToDegrees((float)(Math.Atan(vector.Y / vector.Z)));
            }

            //angle to the x axis
            if (constant == "y" && !vector.Z.Equals(0))
            {
                return MathHelper.ToDegrees((float)(Math.Atan(vector.X / vector.Z)));
            }

            //angle to the x axis
            if (constant == "z" && !vector.X.Equals(0))
            {
                return MathHelper.ToDegrees((float)(Math.Atan(vector.Y / vector.X)));
            }
            return 0;
        }

        /// <summary>
        /// the current camera
        /// </summary>
        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        //the real vectors relative to the position of the camera
        #region RealRotate
        public Vector3 RealLeft { get; set; }
        public Vector3 RealRight { get; set; }
        public Vector3 RealForward { get; set; }
        public Vector3 RealBackward { get; set; }
        #endregion
    }
}
