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
        private bool checkedMouse = false;
        private bool isMouseWorking;
        private bool isFirstTime = true;
        Cube cube;
        Matrix world;
        Matrix view;
        Matrix projection;
        Rectangle Xrect;
        Rectangle Vrect;
        Texture2D Xtex;
        Texture2D Vtex;

        public Tutorial(GraphicsDevice GraphicsDevice, SpriteFont Font, SwitchGameState _gameState, ContentManager Content)
        {
            cube = new Cube();
            lang = new Text();
            welcome = new Welcome();
            cube.Model = Content.Load<Model>("rubik");
            graphicsDevice = GraphicsDevice;
            font = Font;
            gameState = _gameState;
            world = gameState.world;
            view = gameState.view;
            projection = gameState.projection;
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
                    if (keyboard.IsKeyDown(Keys.J) && oldKeyboardState.IsKeyUp(Keys.J))
                    {
                        currentStage = TutorialStage.Keyboard;
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            switch (currentStage)
            {
                case TutorialStage.BasicUsage:
                    if (!isMouseWorking)
                        spriteBatch.DrawString(font, "To move the pointer on screen, move the mouse", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    else if (!isFirstTime)
                    {
                        spriteBatch.DrawString(font, "Good job! It looks like the mouse is working", new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    }
                    break;
                case TutorialStage.Keyboard:
                    spriteBatch.DrawString(font, "Up", new Vector2(0, 50), Color.Black);
                    spriteBatch.DrawString(font, "Down", new Vector2(0, 50 + Xtex.Height/5), Color.Black);
                    spriteBatch.DrawString(font, "Left", new Vector2(0, 50 + Xtex.Height/5 * 2), Color.Black);
                    spriteBatch.DrawString(font, "Right", new Vector2(0, 50 + Xtex.Height/5 * 3), Color.Black);
                    spriteBatch.DrawString(font, "Forward", new Vector2(0, 50 + Xtex.Height/5 * 4), Color.Black);
                    spriteBatch.DrawString(font, "Backwards", new Vector2(0, 50 + Xtex.Height/5 * 5), Color.Black);
                    DrawXV(spriteBatch, "Up", 50);
                    DrawXV(spriteBatch, "Down", 50 + Xtex.Height);
                    DrawXV(spriteBatch, "Left", 50 + Xtex.Height/5 * 2);
                    DrawXV(spriteBatch, "Right", 50 + +Xtex.Height/5 * 3);
                    DrawXV(spriteBatch, "Forward", 50 + +Xtex.Height/5 * 4);
                    DrawXV(spriteBatch, "Backwards", 50 + +Xtex.Height/5 * 5);
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
                spriteBatch.Draw(Vtex, new Rectangle((int)( Vtex.Width/5), placeY, Xtex.Width/5, Xtex.Height/5), Color.White);
            else
                spriteBatch.Draw(Xtex, new Rectangle((int)(Xtex.Width/5), placeY, Xtex.Width/5, Xtex.Height/5), Color.White);
        }
        private Dictionary<string, bool> InitKeysList()
        {
            Dictionary<string, bool> value = new Dictionary<string, bool>();
            value.Add("Up", false);
            value.Add("Down", false);
            value.Add("Left", false);
            value.Add("Right", false);
            value.Add("Forward", false);
            value.Add("Backwards", false);
            return value;
        }
    }
}
