using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RubikCube
{
    /// <summary>
    /// Responsible of all the buttons
    /// </summary>
    class Button
    {
        readonly Texture2D texture; //Texture in use
        Vector2 position; //Position of the button
        Rectangle rectangle; //Rectangle of the button
        public Vector2 Size; //Size of the button
        public bool IsClicked; //If button was clicked, turns True
        private readonly Clocks normalClocks; //The delay after you press a button for normal buttoms
        private readonly Clocks muteClocks; //The delay after you press a button for the mute button
        private bool clickDetected; //If a click was detected
        private MouseState oldMouseState; //The old state of the mouse

        /// <summary>
        /// Constracts the Button
        /// </summary>
        /// <param name="newTexture">Texture to put on the button</param>
        /// <param name="graphics">needed for the default size</param>
        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            //Creates a Clock for the delay of regular buttons, sets it to 500 mili-seconds
            normalClocks = new Clocks();
            normalClocks.InitTimer(500);

            //Creates a Clock for the delay of the mute buttons, sets it to 20 mili-seconds
            muteClocks = new Clocks();
            muteClocks.InitTimer(20);

            //sets the texture as the one provided
            texture = newTexture;

            //defines the default size
            Size = new Vector2(graphics.Viewport.Width / 5f, graphics.Viewport.Height / 7f);
        }

        /// <summary>
        /// the main update, detecting the click on the button
        /// </summary>
        /// <param name="isMuteButton">checks if the button is the mute button or not</param>
        /// <param name="gameTime">needs in order to create a delay</param>
        public void Update(bool isMuteButton, GameTime gameTime)
        {
            //gets the current mouse state
            MouseState mouse = Mouse.GetState();

            //the rectangle containing the button
            rectangle = new Rectangle((int)(position.X), (int)(position.Y), (int)(Size.X), (int)(Size.Y));

            //the point which represents the location of the mouse
            Point mousePos = new Point(mouse.X, mouse.Y);

            //if the mouse is withing the boundaries of the button, and the mouse was clicked
            if ((rectangle.Contains(mousePos) && mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) || clickDetected)
            {
                //mark that a click happened
                clickDetected = true;

                //create the delay for normal buttons
                if (normalClocks.CallTimer(gameTime) && !isMuteButton)
                {
                    //mark the click after the delay
                    IsClicked = true;

                    //reset the detected click
                    clickDetected = false;
                }
                else
                {
                    //the delay for the mute button
                    if (isMuteButton && muteClocks.CallTimer(gameTime))
                    {
                        //mark the click after the delay
                        IsClicked = true;

                        //reset the detected click
                        clickDetected = false;
                    }
                }
            }
            //reset the click flag
            else IsClicked = false;

            //set the old mouse state
            oldMouseState = mouse;
        }


        /// <summary>
        /// set the position of the button
        /// </summary>
        /// <param name="newPositon">the position as a 2d vector</param>
        public void SetPosition(Vector2 newPositon)
        {
            position = newPositon;
        }


        /// <summary>
        /// draw the button
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //as long as there is a valid texture, draw the button
            if (texture != null)
                spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
