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
using Keys = Microsoft.Xna.Framework.Input.Keys;

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
        private bool checkedMouse;
        private bool isMouseWorking;
        private bool isFirstTime = true;
        private string allKeys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private bool isKeyboardWorikng;
        private bool shouldCheckKeyboard;
        private ButtonSetUp button;

        public Tutorial(GraphicsDevice GraphicsDevice, SpriteFont Font, SwitchGameState _gameState,GraphicsDeviceManager graphicsManager,ContentManager content)
        {
            button = new ButtonSetUp(graphicsManager,GraphicsDevice,content);
            lang = new Text();
            graphicsDevice = GraphicsDevice;
            font = Font;
            welcome = new Welcome();
            gameState = _gameState;
        }

        public void Update(GameTime gameTime)
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
                oldKeyboardState = keyboard;
            }
            switch (currentStage)
            {
                case TutorialStage.BasicUsage:
                    if ((mouse.X != oldMouseState.X || mouse.Y != oldMouseState.Y) && !checkedMouse && !isFirstTime)
                    {
                        checkedMouse = true;
                        isMouseWorking = true;
                    }
                    if (isMouseWorking)
                    {
                        button.BtnContinue.Update(false,gameTime);
                        if (button.BtnContinue.IsClicked)
                        {
                            shouldCheckKeyboard = true;
                        }
                        if (CheckForClick(keyboard) && shouldCheckKeyboard)
                        {
                            isKeyboardWorikng = true;
                            shouldCheckKeyboard = false;
                        }
                    }
                    break;
                case TutorialStage.Keyboard:
                    break;
                case TutorialStage.Mouse:
                    break;
                case TutorialStage.CodeLine:
                    break;
            }
            if (!isFirstTime)
            {
                oldMouseState = mouse;
                oldKeyboardState = keyboard;
            }
        }

        private bool CheckForClick(KeyboardState keyboard)
        {
            for (int i = 0; i < allKeys.Length; i++)
            {
                Keys currentKey = (Keys) (Enum.Parse(typeof (Keys), allKeys.Substring(i, 1)));
                if (keyboard.IsKeyDown(currentKey) && oldKeyboardState.IsKeyUp(currentKey))
                    return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            if (!isMouseWorking)
                spriteBatch.DrawString(font, "To move the pointer on screen, move the mouse", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
            else if(!isFirstTime && !shouldCheckKeyboard && !isKeyboardWorikng)
            {
                spriteBatch.DrawString(font, "Good job! It looks like the mouse is working", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);       
                button.BtnContinue.Draw(spriteBatch);
            }
            else if(!isKeyboardWorikng && shouldCheckKeyboard)
            {
                spriteBatch.DrawString(font, "Now try to use the keyboard", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);                                
            }
            else 
            {
                spriteBatch.DrawString(font, "Nice! Looks like the keyboard is working as well", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);                                                
            }
            spriteBatch.End();
        }
    }
}
