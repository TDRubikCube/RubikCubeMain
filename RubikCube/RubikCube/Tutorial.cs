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
        Cube cube;
        Texture2D Xtex;
        Texture2D Vtex;
        List<string> allSides = new List<string> { "Up", "Down", "Left", "Right", "Forward", "Backwards" };

        public Tutorial(GraphicsDevice GraphicsDevice, SpriteFont Font, SwitchGameState _gameState, ContentManager Content, GraphicsDeviceManager graphicsManager)
        {
            button = new ButtonSetUp(graphicsManager, GraphicsDevice, Content);
            cube = new Cube();
            lang = new Text();
            welcome = new Welcome();
            cube.Model = Content.Load<Model>("rubik");
            graphicsDevice = GraphicsDevice;
            font = Font;
            gameState = _gameState;
            gameState.IsUsingKeyboard = false;
            gameState.CheckKeysTTR = InitKeysList();
            Xtex = Content.Load<Texture2D>("pics/X");
            Vtex = Content.Load<Texture2D>("pics/V");
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphicsDeviceManager)
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
                    gameState.IsUsingKeyboard = false;
                    if ((mouse.X != oldMouseState.X || mouse.Y != oldMouseState.Y) && !checkedMouse && !isFirstTime)
                    {
                        checkedMouse = true;
                        isMouseWorking = true;
                    }
                    if (isMouseWorking)
                    {
                        button.BtnContinue.Update(false, gameTime);
                        if (button.BtnContinue.IsClicked)
                        {
                            shouldCheckKeyboard = true;
                        }
                        if (CheckForClick(keyboard) && shouldCheckKeyboard)
                        {
                            isKeyboardWorikng = true;
                            shouldCheckKeyboard = false;
                        }
                        if (keyboard.IsKeyDown(Keys.J) && oldKeyboardState.IsKeyUp(Keys.J))
                        {
                            currentStage = TutorialStage.Keyboard;
                        }
                    }
                    break;
                case TutorialStage.Keyboard:
                    gameState.IsUsingKeyboard = true;
                    gameState.CurrentGameState = GameState.FreePlay;
                    gameState.Update(gameTime, graphicsDeviceManager);
                    break;
                case TutorialStage.Mouse:
                    gameState.IsUsingKeyboard = false;
                    break;
                case TutorialStage.CodeLine:
                    gameState.IsUsingKeyboard = false;
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
                Keys currentKey = (Keys)(Enum.Parse(typeof(Keys), allKeys.Substring(i, 1)));
                if (keyboard.IsKeyDown(currentKey) && oldKeyboardState.IsKeyUp(currentKey))
                    return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            switch (currentStage)
            {
                case TutorialStage.BasicUsage:
                    //checks mouse
                    if (!isMouseWorking)
                    {
                        spriteBatch.DrawString(font, "To move the pointer on screen, move the mouse",
                            new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    }
                    //continue to keyboard
                    else if (!isFirstTime && !shouldCheckKeyboard && !isKeyboardWorikng)
                    {
                        spriteBatch.DrawString(font, "Good job! It looks like the mouse is working",
                            new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                        button.BtnContinue.Draw(spriteBatch);
                    }
                    //checks keyboard
                    else if (!isKeyboardWorikng && shouldCheckKeyboard)
                    {
                        spriteBatch.DrawString(font, "Now try to use the keyboard",
                            new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    }
                    //continue to next stage
                    else
                    {
                        spriteBatch.DrawString(font, "Nice! Looks like the keyboard is working as well",
                            new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    }
                    break;
                case TutorialStage.Keyboard:
                    for (int i = 0; i < allSides.Count; i++)
                    {
                        spriteBatch.DrawString(font, allSides[i], new Vector2(0, 50 + Xtex.Height / 5 * i), Color.Black);
                        DrawXV(spriteBatch, allSides[i], 50 + Xtex.Height / 5 * i);
                    }
                    break;
                case TutorialStage.CodeLine:
                    break;
                case TutorialStage.Mouse:
                    break;
            }
            spriteBatch.End();
        }

        private void DrawXV(SpriteBatch spriteBatch, string key, int placeY)
        {
            if (gameState.CheckKeysTTR[key])
                spriteBatch.Draw(Vtex, new Rectangle(Vtex.Width / 2, placeY, Xtex.Width / 7, Xtex.Height / 7), Color.White);
            else
                spriteBatch.Draw(Xtex, new Rectangle(Xtex.Width / 2, placeY, Xtex.Width / 7, Xtex.Height / 7), Color.White);
        }
        private Dictionary<string, bool> InitKeysList()
        {
            return allSides.ToDictionary(t => t, t => false);
        }
    }
}
