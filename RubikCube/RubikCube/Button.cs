using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RubikCube
{
    class Button
    {
        readonly Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        public Vector2 Size;
        public bool IsClicked;
        private readonly Clocks normalClocks;
        private readonly Clocks muteClocks;
        private bool clickDetected;
        private MouseState oldMouseState;

        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            normalClocks = new Clocks();
            normalClocks.InitTimer(500);
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
