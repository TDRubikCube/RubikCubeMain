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
        /// <param name="newTexture"></param>
        /// <param name="graphics"></param>
        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            //Creates a Clock for the delay of regular buttons, sets it to 500 mili-seconds
            normalClocks = new Clocks();
            normalClocks.InitTimer(500);
            //Creates a Clock for the delay of the mute buttons, sets it to 20 mili-seconds
            muteClocks = new Clocks();
            muteClocks.InitTimer(20);
            texture = newTexture;
            Size = new Vector2(graphics.Viewport.Width / 5f, graphics.Viewport.Height / 7f);
        }

        public void Update(bool isMuteButton, GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            rectangle = new Rectangle((int)(position.X), (int)(position.Y), (int)(Size.X), (int)(Size.Y));
            Point mousePos = new Point(mouse.X, mouse.Y);
            if ((rectangle.Contains(mousePos) && mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)|| clickDetected)
            {
                clickDetected = true;
                if (normalClocks.CallTimer(gameTime) && !isMuteButton)
                {
                    IsClicked = true;
                    clickDetected = false;
                }
                else
                {
                    if (isMuteButton && muteClocks.CallTimer(gameTime))
                    {
                        IsClicked = true;
                        clickDetected = false;
                    }
                }
            }
            else IsClicked = false;
            oldMouseState = mouse;
        }

        public void SetPosition(Vector2 newPositon)
        {
            position = newPositon;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
