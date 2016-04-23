using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/*
using System.Of[A].Down; */

namespace RubikCube
{
    public enum GameState
    {
        MainMenu, Tutorial, FreePlay, Options
    }
    public class SwitchGameState
    {
        #region classes & xna.vars declaration

        readonly Cube cube;
        readonly ButtonSetUp button;
        readonly Text lang;
        readonly SelfSolve solve;
        private Music music;
        readonly Matrix world;
        private readonly Camera camera;
        Matrix view;
        readonly TextBox textbox;
        readonly Matrix projection;
        private Vector3 cameraPos;
        KeyboardState oldKeyboardState;
        MouseState oldMouseState;
        Ray currentRay;
        readonly SpriteFont font;
        private readonly Clocks clocks;
        readonly SpriteFont mono;
        Point mousePos;
        Vector3 centerOfClickedMesh = Vector3.Zero;
        string currentFace = "";
        private string faceClosestToRay = "";
        private GraphicsDevice graphicsDevice;
        private float previousDistanceToMesh;

        #endregion

        #region normal vars

        private float currentScale;
        public bool JustSwitched = false;
        public string AlgOrder = "";
        bool shouldAllowCameraMovement = true;
        public string AllTimeAlgOrder = "";
        public string YAlgOrder = "";
        public int RotationsLeft;
        public bool ShouldRotate;
        public GameState CurrentGameState;
        private bool changeDetected;
        private Point mousePosOnClick;
        private Vector3 direction;
        private readonly List<string> allCubeColors;
        private bool shouldShowStopper;
        private bool shouldRunStopper;
        readonly Texture2D textTex;

        #endregion

        public SwitchGameState(GraphicsDevice graphicsDeviceFromMain, GraphicsDeviceManager graphics, ContentManager content, Music _music)
        {
            shouldRunStopper = true;
            shouldShowStopper = true;
            graphicsDevice = graphicsDeviceFromMain;
            allCubeColors = new List<string> { "white", "yellow", "green", "blue" };
            //class initialize
            lang = new Text();
            cube = new Cube();
            camera = new Camera();
            clocks = new Clocks();
            solve = new SelfSolve(cube);
            music = _music;
            textbox = new TextBox(cube, content);
            button = new ButtonSetUp(graphics, graphicsDevice, content)
            {
                ClassicBound =
                    new Rectangle((int)(graphicsDevice.Viewport.Width / 1.32f), graphicsDevice.Viewport.Height / 3, 60, 40),
                RockBound =
                    new Rectangle((int)(graphicsDevice.Viewport.Width / 1.55f), graphicsDevice.Viewport.Height / 3, 50, 40)
            };
            cube.Model = content.Load<Model>("rubik");
            textTex = content.Load<Texture2D>("pics/TextSquere");

            //text
            font = content.Load<SpriteFont>("font");
            mono = content.Load<SpriteFont>("mono");

            //matrixes
            world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            view = camera.View;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphicsDevice.Viewport.AspectRatio, 10f, 200f);

        }

        #region private methods

        /// <summary>
        /// the main rotation function
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="oldKeyboardState"></param>
        /// <param name="cameraPos"></param>
        private void RotateWhichSide(KeyboardState keyboardState, KeyboardState oldKeyboardState, Vector3 cameraPos)
        {
            camera.RealRotate(cameraPos);
            if (cube.Angle <= -100)
            {
                if ((AlgOrder[0] == 'Z') || (AlgOrder[0] == 'z'))
                {
                    char lastCommnad = AllTimeAlgOrder[AllTimeAlgOrder.Length - 1];
                    if ((lastCommnad == 'I') || (lastCommnad == 'i'))
                    {
                        YAlgOrder = lastCommnad + YAlgOrder;
                        AllTimeAlgOrder = AllTimeAlgOrder.Substring(0, AllTimeAlgOrder.Length - 1);
                    }
                    YAlgOrder = lastCommnad + YAlgOrder;
                    AllTimeAlgOrder = AllTimeAlgOrder.Substring(0, AllTimeAlgOrder.Length - 1);
                }
                if ((AlgOrder[0] == 'Y') || (AlgOrder[0] == 'y'))
                {
                    Debug.WriteLine("HELLO 1");
                    AllTimeAlgOrder += YAlgOrder[0];
                    YAlgOrder = YAlgOrder.Substring(1);
                    if (YAlgOrder.Length > 0)
                    {
                        if ((YAlgOrder[0] == 'I') || (YAlgOrder[0] == 'i'))
                        {
                            Debug.WriteLine("HELLO 2");
                            AllTimeAlgOrder += YAlgOrder[0];
                            YAlgOrder = YAlgOrder.Substring(1);
                        }
                    }
                    Debug.WriteLine("HELLO 3");
                }
                //////////////////////////////////////////////////////
                Debug.WriteLine("Original=   " + AlgOrder);
                if (AlgOrder.Length > 1)
                {
                    if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                    {
                        AllTimeAlgOrder += "I";
                        AlgOrder = AlgOrder.Substring(2);
                    }
                    else
                    {
                        AlgOrder = AlgOrder.Substring(1);

                    }
                    Debug.WriteLine("After Change" + AlgOrder);

                }
                else
                {

                    AlgOrder = "";
                }
                RotationsLeft = AlgOrder.Length;
                if (AlgOrder.Split('i').Length != -1)
                {
                    RotationsLeft -= AlgOrder.Split('i').Length - 1;

                }
                if (AlgOrder.Split('I').Length != -1)
                {
                    RotationsLeft -= AlgOrder.Split('I').Length - 1;

                }
                if (AlgOrder.Split('\'').Length != -1)
                {
                    RotationsLeft -= AlgOrder.Split('\'').Length - 1;

                }
                Debug.WriteLine("Number of rotations left:" + RotationsLeft);
                Debug.WriteLine("AllTimeAlgOrder " + AllTimeAlgOrder);
                Debug.WriteLine("YAlgOrder " + YAlgOrder);
                cube.Angle = 0;
            }
            /////////////
            if (keyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                DebugBorders("");
            }
            if (keyboardState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W))
            {
                DebugBorders("w");
                Debug.WriteLine("AlgOrder is: " + AlgOrder);
                Debug.WriteLine("AllTimeAlgOrder is: " + AllTimeAlgOrder);
                Debug.WriteLine("YAlgOrder is: " + YAlgOrder);
                Debug.WriteLine("cameraPos.X is: " + cameraPos.X);
                Debug.WriteLine("cameraPos.Y is: " + cameraPos.Y);
                Debug.WriteLine("cameraPos.Z is: " + cameraPos.Z);
                Debug.WriteLine("Angle is: " + cube.Angle);
                DebugBorders("w");
            }
            if ((keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl)) && keyboardState.IsKeyUp(Keys.Z) && oldKeyboardState.IsKeyDown(Keys.Z))
            {
                if (AllTimeAlgOrder.Length > 0)
                {
                    AlgOrder += "Z";
                }
            }
            if ((keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl)) && keyboardState.IsKeyUp(Keys.Y) && oldKeyboardState.IsKeyDown(Keys.Y))
            {
                if (YAlgOrder.Length > 0)
                    AlgOrder += "y";
            }

            UpdateAlgo(AlgOrder);

            //here the fun starts
            //Debug.WriteLine("algOrder=    "+algOrder);

            if (true) //Because we're getting rid of this fucking function ayyy
            {
                if (AlgOrder.Length > 0)
                {
                    if ((AlgOrder[0] == 'Z') || (AlgOrder[0] == 'z')) //Check for control+Z
                    {
                        if (AllTimeAlgOrder.Length > 0)
                        {
                            ControlZ();
                        }
                        else
                        {
                            AlgOrder = AlgOrder.Substring(1);
                            Console.Beep();
                        }
                    }
                    else if ((AlgOrder[0] == 'Y') || (AlgOrder[0] == 'y')) //Check for control+Y
                    {
                        if (YAlgOrder.Length > 0)
                        {
                            ControlY();
                        }
                        else
                        {
                            AlgOrder = AlgOrder.Substring(1);
                            Console.Beep(1, 100);
                            Console.Beep(5, 100);
                        }
                    }

                    else if ((AlgOrder[0] == 'L') || (AlgOrder[0] == 'l'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(CharToVector("L"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("L"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("L"), true, AlgOrder);
                        }
                    }
                    else if ((AlgOrder[0] == 'R') || (AlgOrder[0] == 'r'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(CharToVector("R"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("R"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("R"), true, AlgOrder);
                        }
                    }
                    else if ((AlgOrder[0] == 'U') || (AlgOrder[0] == 'u'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {

                                cube.Rotate(Vector3.Up, false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(Vector3.Up, true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(Vector3.Up, true, AlgOrder);
                        }
                    }
                    else if ((AlgOrder[0] == 'D') || (AlgOrder[0] == 'd'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(Vector3.Down, false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(Vector3.Down, true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(Vector3.Down, true, AlgOrder);
                        }
                    }
                    else if ((AlgOrder[0] == 'B') || (AlgOrder[0] == 'b'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(CharToVector("B"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("B"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("B"), true, AlgOrder);
                        }
                    }
                    else if ((AlgOrder[0] == 'F') || (AlgOrder[0] == 'f'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(CharToVector("F"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("F"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("F"), true, AlgOrder);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("AlgOrder unknown = " + AlgOrder);
                        AlgOrder = AlgOrder.Substring(1);
                    }
                }
            }
        }

        private void CheckForClick(ref KeyboardState keyboardState, ref KeyboardState oldKeyboardState, Keys key, Vector3 direction)
        {
            if (keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
            {
                AlgOrder += (VectorToChar(direction));
                AllTimeAlgOrder += (VectorToChar(direction));
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
                YAlgOrder = "";
            }
            //camera.RealRotate(cameraPos);
        }

        /// <summary>
        /// sets the oldState vars
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="keyboardState"></param>
        private void OldState(ref MouseState mouseState, ref KeyboardState keyboardState)
        {
            oldMouseState = mouseState;
            oldKeyboardState = keyboardState;
        }

        /// <summary>
        /// updates everything that's specific for a GameState
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="keyboardState"></param>
        /// <param name="gameTime"></param>
        private void SwitchUpdate(KeyboardState keyboardState, GameTime gameTime)
        {
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (button.BtnFreePlay.IsClicked)
                        CurrentGameState = GameState.FreePlay;
                    if (button.BtnTutorial.IsClicked) CurrentGameState = GameState.Tutorial;
                    if (button.BtnOptions.IsClicked) CurrentGameState = GameState.Options;
                    button.BtnOptions.Update(false, gameTime);
                    button.BtnTutorial.Update(false, gameTime);
                    button.BtnFreePlay.Update(false, gameTime);
                    break;
                case GameState.Tutorial:
                    if (keyboardState.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;
                    break;
                case GameState.Options:
                    if (keyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right)) MediaPlayer.Stop();
                    if (button.BtnRussian.IsClicked) lang.Russian();
                    if (button.BtnHebrew.IsClicked) lang.Hebrew();
                    if (button.BtnEnglish.IsClicked) lang.English();
                    button.BtnEnglish.Update(false, gameTime);
                    button.BtnHebrew.Update(false, gameTime);
                    button.BtnRussian.Update(false, gameTime);
                    CheckClickOnAddMusic();
                    music.Update();
                    if (keyboardState.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;
                    break;
                case GameState.FreePlay:
                    if (keyboardState.IsKeyDown(Keys.Escape))
                    {
                        CurrentGameState = GameState.MainMenu;
                        DebugBorders("MainMenu");
                    }
                    if (button.BtnScramble.IsClicked) ShouldRotate = true;
                    if (button.BtnSolve.IsClicked)
                    {
                        clocks.StopStoper();
                        cube.Angle = 0;
                        ShouldRotate = false;
                        AlgOrder = "";
                        AllTimeAlgOrder = "";
                        YAlgOrder = "";
                        cube.Solve();
                        DebugBorders("Reset!");
                    }
                    textbox.Update(keyboardState, oldKeyboardState, gameTime, mono, AlgOrder, camera, cube);
                    //if (cube.ScrambleIndex >= 25)
                    if (ShouldRotate || solve.ShouldScramble)
                    {
                        cube.Scramble();
                        AlgOrder += cube.ScrambleResult;
                        ShouldRotate = false;
                        solve.ShouldScramble = false;
                    }
                    CheckClickOnStopper();
                    clocks.UpdateStoper(gameTime);
                    if (shouldRunStopper)
                        clocks.StartStoper();
                    button.BtnScramble.Update(false, gameTime);
                    button.BtnSolve.Update(false, gameTime);

                    break;

            }
        }

        private void CheckClickOnAddMusic()
        {
            Rectangle rect = new Rectangle((int)(graphicsDevice.Viewport.Width / 2f - font.MeasureString(lang.OptionsAddMusic).X / 2), 100, (int)font.MeasureString(lang.OptionsAddMusic).X, (int)font.MeasureString(lang.OptionsAddMusic).Y);
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                if (rect.Contains(mousePos))
                {
                    music.AddMusic();
                }
            }
        }

        private void CheckClickOnStopper()
        {
            MouseState mouse = Mouse.GetState();
            Point mousePoint = new Point(mouse.X, mouse.Y);
            int locX = (int)(graphicsDevice.Viewport.Width / 1.31);
            int locY = (int)(graphicsDevice.Viewport.Height / 2f);
            Rectangle showStopperRect = new Rectangle(locX, locY, (int)font.MeasureString(lang.FreePlayStopperShow).X, (int)font.MeasureString(lang.FreePlayStopperShow).Y);
            Rectangle pauseStopperRect = new Rectangle(locX, 50 + locY, (int)font.MeasureString(lang.FreePlayStopperPause).X, (int)font.MeasureString(lang.FreePlayStopperPause).Y);
            Rectangle resumeStopperRect = new Rectangle(locX, 100 + locY, (int)font.MeasureString(lang.FreePlayStopperResume).X, (int)font.MeasureString(lang.FreePlayStopperResume).Y);
            Rectangle resetStopperRect = new Rectangle(locX, 150 + locY, (int)font.MeasureString(lang.FreePlayStopperReset).X, (int)font.MeasureString(lang.FreePlayStopperReset).Y);
            if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                if (showStopperRect.Contains(mousePoint))
                {
                    shouldShowStopper = !shouldShowStopper;
                    if (shouldRunStopper)
                    {
                        shouldRunStopper = false;
                        clocks.PauseStoper();
                    }
                    else
                    {
                        shouldRunStopper = true;
                        clocks.ResumeStoper();
                    }
                }
                else if (pauseStopperRect.Contains(mousePoint) && shouldRunStopper)
                {
                    shouldRunStopper = false;
                    clocks.PauseStoper();
                }
                else if (resumeStopperRect.Contains(mousePoint) && !shouldRunStopper)
                {
                    shouldRunStopper = true;
                    clocks.ResumeStoper();
                }
                else if (resetStopperRect.Contains(mousePoint))
                {
                    clocks.StopStoper();
                }
            }
        }

        /// <summary>
        /// draws everything that is specific for a GameState
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphicsDevice"></param>
        private void SwitchDraw(SpriteBatch spriteBatch)
        {
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    spriteBatch.DrawString(font, lang.MainTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    button.BtnTutorial.Draw(spriteBatch);
                    button.BtnOptions.Draw(spriteBatch);
                    button.BtnFreePlay.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.FreePlay:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, lang.FreePlayTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlayScramble, new Vector2(graphicsDevice.Viewport.Width / 13f, graphicsDevice.Viewport.Height / 1.4f), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlaySolve, new Vector2(graphicsDevice.Viewport.Width / 4f, graphicsDevice.Viewport.Height / 1.4f), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlayStopperShow, new Vector2(graphicsDevice.Viewport.Width / 1.35f, graphicsDevice.Viewport.Height / 2f), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlayStopperPause, new Vector2(graphicsDevice.Viewport.Width / 1.35f, 50 + graphicsDevice.Viewport.Height / 2f), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlayStopperResume, new Vector2(graphicsDevice.Viewport.Width / 1.35f, 100 + graphicsDevice.Viewport.Height / 2f), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlayStopperReset, new Vector2(graphicsDevice.Viewport.Width / 1.35f, 150 + graphicsDevice.Viewport.Height / 2f), Color.Black);
                    if (shouldShowStopper)
                        clocks.DrawStoper(spriteBatch, font, new Vector2(graphicsDevice.Viewport.Width / 3f, 30));
                    button.BtnScramble.Draw(spriteBatch);
                    button.BtnSolve.Draw(spriteBatch);
                    spriteBatch.End();
                    DrawModel(cube, world, view, projection);
                    textbox.Draw(spriteBatch, mono, font, textTex);
                    break;
                case GameState.Options:
                    spriteBatch.Begin();
                    button.BtnRussian.Draw(spriteBatch);
                    button.BtnHebrew.Draw(spriteBatch);
                    button.BtnEnglish.Draw(spriteBatch);
                    spriteBatch.DrawString(font, lang.OptionsTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    spriteBatch.DrawString(font, lang.OptionsFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 40), Color.Black);
                    spriteBatch.DrawString(font, lang.OptionsAddMusic, new Vector2(graphicsDevice.Viewport.Width / 2f - font.MeasureString(lang.OptionsAddMusic).X / 2, 100), Color.Black);
                    spriteBatch.DrawString(font, "English", new Vector2(graphicsDevice.Viewport.Width / 2.5f, 440), Color.Black);
                    spriteBatch.DrawString(font, "ת י ר ב ע", new Vector2(graphicsDevice.Viewport.Width / 1.85f, 440), Color.Black);
                    spriteBatch.DrawString(font, "Russian", new Vector2(graphicsDevice.Viewport.Width / 1.55f, 440), Color.Black);
                    spriteBatch.End();
                    break;
                case GameState.Tutorial:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    spriteBatch.DrawString(font, lang.TutorialFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    spriteBatch.DrawString(font, lang.TutorialFreeText2, new Vector2(graphicsDevice.Viewport.Width / 3f, 90), Color.Black);
                    spriteBatch.End();
                    break;
            }
        }

        public void ControlZ()
        {
            if (AllTimeAlgOrder.Length > 0)
            {
                int num = 0;
                int length = AllTimeAlgOrder.Length - 1;
                bool counterClockWise = false;
                if (AllTimeAlgOrder[0] != 'I' && ((AllTimeAlgOrder[length - num] == 'I') || (AllTimeAlgOrder[length - num] == 'i')))
                {
                    num += 1;
                    counterClockWise = true;
                }
                if (AllTimeAlgOrder[length - num] == 'L')
                {
                    cube.Rotate(Vector3.Left, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'R')
                {
                    cube.Rotate(Vector3.Right, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'U')
                {
                    cube.Rotate(Vector3.Up, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'D')
                {
                    cube.Rotate(Vector3.Down, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'F')
                {
                    cube.Rotate(Vector3.Forward, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'B')
                {
                    cube.Rotate(Vector3.Backward, counterClockWise, AlgOrder);
                }
            }
            else
            {
                Debug.WriteLine("AllTimeAlgOrder is 0!!!");
            }
        }
        public void ControlY()
        {
            if (YAlgOrder.Length > 0)
            {
                int num = 0;
                int length = YAlgOrder.Length - 1;
                bool counterClockWise = true;
                if (YAlgOrder.Length > 1)
                {
                    if ((YAlgOrder[1] == 'I') || (YAlgOrder[1] == 'i'))
                    {
                        num += 1;
                        counterClockWise = false;
                    }
                }
                if (YAlgOrder[num] == 'L')
                {
                    cube.Rotate(Vector3.Left, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'R')
                {
                    cube.Rotate(Vector3.Right, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'U')
                {
                    cube.Rotate(Vector3.Up, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'D')
                {
                    cube.Rotate(Vector3.Down, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'F')
                {
                    cube.Rotate(Vector3.Forward, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'B')
                {
                    cube.Rotate(Vector3.Backward, counterClockWise, AlgOrder);
                }
            }
            else
            {
                Debug.WriteLine("YAlgOrder is 0!!!");
            }
        }
        public string VectorToChar(Vector3 real)
        {
            if (real == Vector3.Left)
            {
                return "L";
            }
            if (real == Vector3.Right)
            {
                return "R";
            }
            if (real == Vector3.Backward)
            {
                return "B";
            }
            if (real == Vector3.Forward)
            {
                return "F";
            }
            if (real == Vector3.Up)
            {
                return "U";
            }
            if (real == Vector3.Down)
            {
                return "D";
            }
            Debug.WriteLine("VectorToChar returned null");
            return "";

        }

        public Vector3 CharToVector(string real)
        {
            if ((real == "l") || (real == "L"))
            {
                return Vector3.Left;
            }
            if ((real == "r") || (real == "R"))
            {
                return Vector3.Right;
            }
            if ((real == "b") || (real == "B"))
            {
                return Vector3.Backward;
            }
            if ((real == "f") || (real == "F"))
            {
                return Vector3.Forward;
            }
            if ((real == "u") || (real == "U"))
            {
                return Vector3.Up;
            }
            if ((real == "d") || (real == "D"))
            {
                return Vector3.Down;
            }
            Debug.WriteLine("CharToVector returned null");
            return Vector3.Zero;
        }

        public void DebugBorders(string a)
        {
            string b = "~~~~~~~~~~~~~~";
            if (a.Length < (b.Length * 2))
            {
                for (int i = 0; i < (a.Length) / 2; i++)
                {
                    b = b.Substring(1);
                }
                if ((a.Length % 2) == 1)
                {
                    b = b.Substring(1) + a + b;
                }
                else
                {
                    b += a + b;
                }
                Debug.WriteLine(b);
                //Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            else
            {
                Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// the main draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphicsDevice"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            SwitchDraw(spriteBatch);
        }

        /// <summary>
        /// the main update
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphics"></param>
        /// <param name="graphicsDeviceFromMain"></param>
        /// <param name="_music"></param>
        public void Update(GameTime gameTime, GraphicsDevice graphicsDeviceFromMain, Music _music)
        {
            graphicsDevice = graphicsDeviceFromMain;
            music = _music;
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            view = camera.View;
            cameraPos = Matrix.Invert(view).Translation;
            if (CurrentGameState == GameState.FreePlay)
            {
                solve.Update();
                SetCurrentFace();
                if (keyboardState.IsKeyDown(Keys.C) && oldKeyboardState.IsKeyUp(Keys.C))
                    shouldAllowCameraMovement = !shouldAllowCameraMovement;
                if (shouldAllowCameraMovement)
                    camera.CameraMovement(mouseState, oldMouseState);
                else
                    MainRayControl(mouseState);
                camera.Update();
                RotateWhichSide(keyboardState, oldKeyboardState, cameraPos);
                if (textbox.EnterPressed)
                {
                    AlgOrder += textbox.GetRealVectorBox;
                    AllTimeAlgOrder += textbox.GetRealVectorBox;
                }
            }
            mousePos = new Point(mouseState.X, mouseState.Y);
            SwitchUpdate(keyboardState, gameTime);
            OldState(ref mouseState, ref keyboardState);
            currentScale = Main.CubieSize * 3 / graphicsDevice.Viewport.AspectRatio / Main.OriginalScale;
        }

        /// <summary>
        /// switches the GameState to tutorial
        /// </summary>
        public void SwitchToTutorial()
        {
            CurrentGameState = GameState.Tutorial;
        }

        /// <summary>
        /// draws the model given
        /// </summary>
        /// <param name="cube"></param>
        /// <param name="objectWorldMatrix"></param>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        /// <param name="graphicsDevice"></param>
        public void DrawModel(Cube cube, Matrix objectWorldMatrix, Matrix view, Matrix projection)
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            for (int index = 0; index < cube.Model.Meshes.Count; index++)
            {
                ModelMesh mesh = cube.Model.Meshes[index];
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateScale(currentScale) * mesh.ParentBone.Transform * Matrix.CreateTranslation(Main.CubieSize, -Main.CubieSize, -Main.CubieSize) * cube.MeshTransforms[index] * objectWorldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();

            }
        }

        public void UpdateAlgo(string algo)
        {
            AlgOrder = algo;
        }

        #endregion

        #region Rays

        private void MainRayControl(MouseState mouseState)
        {
            DrawRay(mouseState);
            if (FindMeshOnClick(mouseState) != null)
            {
                changeDetected = true;
                centerOfClickedMesh = FindMeshOnClick(mouseState).Item2;
            }

            if (mouseState.LeftButton == ButtonState.Released && mousePosOnClick != new Point(0, 0) && changeDetected)
            {
                changeDetected = false;
                double diffX = mouseState.X - mousePosOnClick.X;
                double diffY = mousePosOnClick.Y - mouseState.Y;
                double angle = (Math.Atan(diffY / (diffX + 1))) * 180.0 / Math.PI;
                if (Math.Abs(diffX) > Math.Abs(diffY) && angle < 45 && angle > -45)
                {
                    if (diffX > 0)
                    {
                        //rotation right
                        if (centerOfClickedMesh.Y < 3)
                            AlgOrder += "dI";
                        else if (centerOfClickedMesh.Y > 4)
                        {
                            AlgOrder += "UI";
                        }
                    }
                    else
                    {
                        //rotation left
                        if (centerOfClickedMesh.Y < 3)
                            AlgOrder += "d";
                        else if (centerOfClickedMesh.Y > 4)
                        {
                            AlgOrder += "U";
                        }
                    }
                }
                //checks if it should rotate the left/right sides
                else if (Math.Abs(diffY) > Math.Abs(diffX) && (angle < -45 || angle > 45))
                {
                    foreach (string face in allCubeColors)
                    {
                        RotateLRorFb(diffY, face);
                    }
                }
            }
        }

        private void RotateLRorFb(double diffY, string givenFace)
        {
            if (currentFace == givenFace && faceClosestToRay == givenFace)
            {
                if (diffY > 0)
                {
                    AlgOrder += RotateWhichLayer(centerOfClickedMesh, currentFace, "down");
                }
                else
                {
                    AlgOrder += RotateWhichLayer(centerOfClickedMesh, currentFace, "up");
                }
            }
        }

        private void SetCurrentFace()
        {
            if (camera.IsFaceGreen(cameraPos))
                currentFace = "green";
            if (camera.IsFaceBlue(cameraPos))
                currentFace = "blue";
            if (camera.IsFaceWhite(cameraPos))
                currentFace = "white";
            if (camera.IsFaceYellow(cameraPos))
                currentFace = "yellow";
        }

        private Tuple<ModelMesh, Vector3> FindMeshOnClick(MouseState mouseState)
        {
            Tuple<ModelMesh, float, Vector3> closestMesh = null;
            Tuple<string, float> closestFace = new Tuple<string, float>("", int.MaxValue);
            for (int index = 0; index < cube.Model.Meshes.Count; index++)
            {
                ModelMesh mesh = cube.Model.Meshes[index];
                Vector3 meshCenter = mesh.BoundingSphere.Center;
                meshCenter = Vector3.Transform(meshCenter,
                    Matrix.CreateScale(currentScale) * mesh.ParentBone.Transform *
                    Matrix.CreateTranslation(Main.CubieSize, -Main.CubieSize, -Main.CubieSize) *
                    cube.MeshTransforms[index] * world);

                BoundingSphere bs = new BoundingSphere(meshCenter, Main.CubieSize / 2);
                //Debug.WriteLine(meshCenter);
                if (CheckIndex(index).Item2 && !previousDistanceToMesh.Equals(0))
                {
                    if (FindDistance(bs, mouseState, previousDistanceToMesh) < closestFace.Item2)
                    {
                        closestFace = new Tuple<string, float>(CheckIndex(index).Item1, FindDistance(bs, mouseState, previousDistanceToMesh));
                    }
                }
                if (currentRay.Intersects(bs).HasValue)
                {
                    float distance = currentRay.Intersects(bs).Value;
                    if (closestMesh != null)
                    {
                        if (distance < closestMesh.Item2)
                        {
                            closestMesh = new Tuple<ModelMesh, float, Vector3>(mesh, distance, meshCenter);
                        }
                    }
                    else
                        closestMesh = new Tuple<ModelMesh, float, Vector3>(mesh, distance, meshCenter);
                }
            }
            //Debug.WriteLine(faceClosestToRay + " here2");
            if (closestMesh == null)
                return null;
            previousDistanceToMesh = closestMesh.Item2;
            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                mousePosOnClick = new Point(mouseState.X, mouseState.Y);
                faceClosestToRay = closestFace.Item1;
                return new Tuple<ModelMesh, Vector3>(closestMesh.Item1, closestMesh.Item3);
            }
            if (mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Released)
                mousePosOnClick = new Point(0, 0);
            return null;
        }

        private float FindDistance(BoundingSphere bs, MouseState mouseState, float rayLength)
        {
            Vector3 nearPoint = graphicsDevice.Viewport.Unproject(new Vector3(mouseState.X, mouseState.Y, 0), projection, view, world);
            Vector3 intersectionPoint = nearPoint + direction * rayLength;
            Vector3 destination = bs.Center - intersectionPoint;
            destination.Normalize();
            Ray rayToTarget = new Ray(intersectionPoint, destination);
            if (rayToTarget.Intersects(bs).HasValue)
                return rayToTarget.Intersects(bs).Value;
            return int.MaxValue;
        }

        private Tuple<string, bool> CheckIndex(int i)
        {
            if (i == 1)
            {
                return new Tuple<string, bool>("green", true);
            }
            if (i == 3)
            {
                return new Tuple<string, bool>("blue", true);
            }
            if (i == 10)
            {
                return new Tuple<string, bool>("orange", true);
            }
            if (i == 13)
            {
                return new Tuple<string, bool>("white", true);
            }
            if (i == 21)
            {
                return new Tuple<string, bool>("yellow", true);
            }
            if (i == 25)
            {
                return new Tuple<string, bool>("red", true);
            }
            return new Tuple<string, bool>("", false);
        }

        private void DrawRay(MouseState mouseState)
        {
            Vector3 nearSource = new Vector3(mouseState.X, mouseState.Y, 0);
            Vector3 farSource = new Vector3(mouseState.X, mouseState.Y, 1);
            Vector3 nearPoint = graphicsDevice.Viewport.Unproject(nearSource, projection, view, world);
            Vector3 farPoint = graphicsDevice.Viewport.Unproject(farSource, projection, view, world);
            direction = farPoint - nearPoint;
            direction.Normalize();
            currentRay = new Ray(nearPoint, direction);
        }

        private string RotateWhichLayer(Vector3 meshCenter, string whichFace, string whichDirection)
        {
            if (whichFace == "green")
            {
                if (meshCenter.X < 0)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealLeft) + 'i';
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealLeft);
                }
                if (meshCenter.X > 1)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealRight);
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealRight) + 'i';
                }
            }
            else if (whichFace == "blue")
            {
                if (meshCenter.X > 0)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealLeft) + 'i';
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealLeft);
                }
                if (meshCenter.X < 1)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealRight);
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealRight) + 'i';
                }
            }
            else if (whichFace == "yellow")
            {
                if (meshCenter.Z > 1)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealLeft) + 'i';
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealLeft);
                }
                if (meshCenter.Z < 1)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealRight);
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealRight) + 'i';
                }
            }
            else if (whichFace == "white")
            {
                if (meshCenter.Z < 1)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealLeft) + 'i';
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealLeft);
                }
                if (meshCenter.Z > 1)
                {
                    if (whichDirection == "down")
                        return VectorToChar(camera.RealRight);
                    if (whichDirection == "up")
                        return VectorToChar(camera.RealRight) + 'i';
                }
            }
            return "";
        }


        #endregion

    }
}
