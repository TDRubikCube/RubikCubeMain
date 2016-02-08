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
using System.Windows.Forms;

namespace RubikCube
{
    public enum TutorialStage
    {
        BasicUsage, Keyboard, Mouse, CodeLine
    }
    class Tutorial
    {
        private SpriteFont font;
        private Text lang;
        private GraphicsDevice graphicsDevice;
        private Welcome welcome;
        private bool justFinished = true;
        private SwitchGameState gameState;
        private TutorialStage currentStage;
        private MouseState oldMouseState;
        private KeyboardState oldKeyboardState;
        private bool checkedMouse = false;
        private bool isMouseWorking;
        private bool isFirstTime = true;

        public Tutorial(GraphicsDevice GraphicsDevice, SpriteFont Font, SwitchGameState _gameState)
        {
            lang = new Text();
            graphicsDevice = GraphicsDevice;
            font = Font;
            welcome = new Welcome();
            gameState = _gameState;
        }

        public void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (justFinished)
            {
                welcome.Show();
                justFinished = false;
            }
            if (!Application.OpenForms.OfType<Welcome>().Any() && isFirstTime)
            {
                currentStage = TutorialStage.BasicUsage;
                isFirstTime = false;
                oldMouseState = mouse;
            }
            switch (currentStage)
            {
                case TutorialStage.BasicUsage:
                    if ((mouse.X != oldMouseState.X || mouse.Y != oldMouseState.Y) && !checkedMouse && !isFirstTime)
                    {
                        checkedMouse = true;
                        isMouseWorking = true;
                    }
                    break;
                case TutorialStage.Keyboard:
                    break;
                case TutorialStage.Mouse:
                    break;
                case TutorialStage.CodeLine:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            if (!isMouseWorking)
                spriteBatch.DrawString(font, "To move the pointer on screen, move the mouse", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
            else if(!isFirstTime)
            {
                spriteBatch.DrawString(font, "Good job! It looks like the mouse is working", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);                
            }
            spriteBatch.End();
        }
    }
}
