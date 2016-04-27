using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

/*
using System.Of[A].Down; */

namespace RubikCube
{
    //defines the 4 main states of the game, each of which is an option of a "GameState"
    public enum GameState
    {
        MainMenu, Tutorial, FreePlay, Options
    }

    public class SwitchGameState
    {
        #region classes & xna.vars declaration

        //creates an instance of buttonSetUp
        readonly ButtonSetUp button;

        private Tutorial tutorial;

        //creates an instance of Camera
        private readonly Camera camera;

        //creates an instance of Clocks
        private readonly Clocks clocks;

        //creates an instance of cube
        readonly Cube cube;

        //creates a graphicsDevice, in order for it to be accessible in the whole class
        private GraphicsDevice graphicsDevice;

        //creates a keyboardState to save the keyboardState that was in the last frame
        KeyboardState oldKeyboardState;

        //creates the projection matrix
        readonly Matrix projection;

        //creates the view matrix
        Matrix view;

        //creates the world matrix
        readonly Matrix world;

        //creates a mouseState to save the mouseState that was in the last frame        
        MouseState oldMouseState;

        //creates an instance of music
        private Music music;

        //creates a point which will represent the position of the mouse
        Point mousePos;

        //creates an instace of Text
        readonly Text lang;

        //creates an instance of SelfSolve
        readonly SelfSolve solve;

        //creates an instance of TextBox
        readonly TextBox textbox;

        //creates a 3d vector represting the position of the camera
        private Vector3 cameraPos;

        //creates a ray that will represent the current ray projected
        Ray currentRay;

        //creates 2 font instances
        readonly SpriteFont font;
        readonly SpriteFont mono;

        //creates a 3d vector representing the location of the center of the mesh clicked
        Vector3 centerOfClickedMesh = Vector3.Zero;

        #endregion

        #region normal vars

        //the face shown to the camera, which is the "front" face
        string currentFace = "";

        //distance to previous mesh checked
        private float previousDistanceToMesh;

        //the face which is closest to the point where the ray hit
        private string faceClosestToRay = "";

        //the scale of the cube
        private float currentScale;

        //the order of the algorithms
        public string AlgOrder = "";

        //sets whether the camera should move or not    
        bool shouldAllowCameraMovement = true;

        //history of all algorithms
        public string AllTimeAlgOrder = "";

        //history of ctrl Z
        public string YAlgOrder = "";

        //number of rotations left (used for debugging only)
        public int RotationsLeft;

        //checks if should rotate or not (used for the scramble function)
        public bool ShouldRotate;

        //defines the current game state
        public GameState CurrentGameState;

        //checks if the mesh the ray currently colided with, has changed
        private bool changeDetected;

        //the mouse position at the moment of pressing the mouse (when trying to rotate the cube)
        private Point mousePosOnClick;

        //direction of the current ray
        private Vector3 direction;

        // defines all the cube colors
        private readonly List<string> allCubeColors;

        //marks whethear the stopeer should be visible/hidden
        private bool shouldShowStopper;

        //marks whether the stopper should run or not
        private bool shouldRunStopper;

        //texture used for the background of letters in the textbx
        readonly Texture2D textTex;

        #endregion

        /// <summary>
        /// in charge of the logic of what info such as buttons and text to show in each state of the game
        /// also sends info between different classes and states
        /// </summary>
        /// <param name="graphicsDeviceFromMain">takes the graphicsDevice from Main</param>
        /// <param name="graphics">takes the graphicsDeviceManager from Main</param>
        /// <param name="content">takes the contentManager from Main</param>
        /// <param name="_music">takes the Music from Main</param>
        public SwitchGameState(GraphicsDevice graphicsDeviceFromMain, GraphicsDeviceManager graphics, ContentManager content, Music _music)
        {
            //sets the stopper to run and be shown
            shouldRunStopper = true;
            shouldShowStopper = true;
            //sets the graphics device as the one from main
            graphicsDevice = graphicsDeviceFromMain;
            //sets the cube colors (without the top and bottom)
            allCubeColors = new List<string> { "white", "yellow", "green", "blue" };

            //class initialize
            lang = new Text();
            cube = new Cube();
            camera = new Camera();
            clocks = new Clocks();
            solve = new SelfSolve(cube);
            music = _music;
            textbox = new TextBox(cube, content);
            button = new ButtonSetUp(graphicsDevice, content);
            //loads the cube model
            cube.Model = content.Load<Model>("rubik");
            //loads the texture for the background of the letters in the textbox
            textTex = content.Load<Texture2D>("pics/TextSquere");

            //loads the font files
            font = content.Load<SpriteFont>("font");
            mono = content.Load<SpriteFont>("mono");

            //sets where the model is in relation to the whole world
            world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            //sets the camera matrix
            view = camera.View;
            //determines properties of the camera such as field of view, min distance to object...
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphicsDevice.Viewport.AspectRatio, 10f, 200f);

        }

        public Dictionary<string, bool> CheckKeysTTR { get; set; }
        public bool IsUsingKeyboard { get; set; }

        #region private methods

        /// <summary>
        /// the main rotation function
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="oldKeyboardState"></param>
        /// <param name="cameraPos"></param>
        private void RotateWhichSide(KeyboardState keyboardState, KeyboardState oldKeyboardState, Vector3 cameraPos)
        {
            //make the camera find all the "real vectors"
            camera.RealRotate(cameraPos);
            //checks whether it should rotate or not
            if (cube.Angle <= -100)
            {
                //checks for any kind of useless comands and removes the letter of the command that just finished
                RemoveLettersAfterRotation();

                //sets the number of rotations left in total  (used for debug) and other debug stuff
                StuffToDebugAfterRotation();

                //reset the angle of rotation, since the rotation is done
                cube.Angle = 0;
            }

            //more debug stuff 
            CheckStuffInDebug(keyboardState, oldKeyboardState, cameraPos);

            //check for a press of ctrl+y or ctrl +z
            CheckPressOfCtrl(keyboardState, oldKeyboardState);

            //check all possible rotations
            CheckForRotation();
        }

        /// <summary>
        /// removes any useless commands and deletes the last command that just finished
        /// </summary>
        private void RemoveLettersAfterRotation()
        {
            //sets the limit to the loop (nneded since the var used to set limit will change during the loop)
            int limit = AllTimeAlgOrder.Length - 1;

            //replace any double ii
            for (int i = 0; i < limit; i++)
            {
                AllTimeAlgOrder = AllTimeAlgOrder.Replace("II", "I");
            }

            //removes any useless i
            if (AllTimeAlgOrder.Length > 0)
            {
                if (AllTimeAlgOrder[0] == 'I')
                {
                    AllTimeAlgOrder = AllTimeAlgOrder.Substring(1, AllTimeAlgOrder.Length - 1);
                }
            }

            //checks if the first letter is 'Z'
            if ((AlgOrder[0] == 'Z') || (AlgOrder[0] == 'z'))
            {
                //inserts the one that z erased and puts it into Yalgo
                char lastCommnad = AllTimeAlgOrder[AllTimeAlgOrder.Length - 1];

                if ((lastCommnad == 'I') || (lastCommnad == 'i'))
                {
                    YAlgOrder += lastCommnad;

                    //deletes the last letter in alltimealgo if its I
                    AllTimeAlgOrder = AllTimeAlgOrder.Substring(0, AllTimeAlgOrder.Length - 1);

                    //sets the new last command in alltimealgo
                    lastCommnad = AllTimeAlgOrder[AllTimeAlgOrder.Length - 1];
                }

                //adds to Yalgo the last letter, and removes the last in alltimealgo
                YAlgOrder += lastCommnad;
                AllTimeAlgOrder = AllTimeAlgOrder.Substring(0, AllTimeAlgOrder.Length - 1);
            }
            //checks if the first letter is y
            if ((AlgOrder[0] == 'Y') || (AlgOrder[0] == 'y'))
            {
                //add the last letter in yalgo, into alltimealgo
                AllTimeAlgOrder += YAlgOrder[YAlgOrder.Length - 1];
                //removes the last letter in yalgo
                YAlgOrder = YAlgOrder.Substring(0, YAlgOrder.Length - 1);
                //checks for an I
                if (YAlgOrder.Length > 0)
                {
                    if ((YAlgOrder.Last() == 'I') || (YAlgOrder.Last() == 'i'))
                    {
                        //adds to alltimealgo the last letter in yalgo
                        AllTimeAlgOrder += YAlgOrder.Last();
                        //removes the last letter in yalgo
                        YAlgOrder = YAlgOrder.Substring(0, YAlgOrder.Length - 1);
                    }
                }
            }

            //checks for an i
            if (AlgOrder.Length > 1)
            {
                if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                {
                    //removes the i from algo, as well as the last command
                    AlgOrder = AlgOrder.Substring(2);
                }
                else
                {
                    //removes the last command from algo
                    AlgOrder = AlgOrder.Substring(1);
                }
            }
            else
            {
                //if algo has just 1 letter in it, than make algo empty
                AlgOrder = "";
            }
        }

        /// <summary>
        /// check for all possible torations
        /// </summary>
        private void CheckForRotation()
        {
            //makes sure that algo is not empty
            if (AlgOrder.Length > 0)
            {
                //checks for ctrl+z
                if ((AlgOrder[0] == 'Z') || (AlgOrder[0] == 'z'))
                {
                    //makes sure alltimealgo is not empty since ctrl+z depands on it
                    if (AllTimeAlgOrder.Length > 0)
                    {
                        //make the turns using the following method
                        ControlZ();
                    }
                    else
                    {
                        //this is in case of an error, it will make algorder empty (in most cases)
                        AlgOrder = AlgOrder.Substring(1);
                    }
                }
                //checks for ctrl+y
                else if ((AlgOrder[0] == 'Y') || (AlgOrder[0] == 'y'))
                {
                    //makes sure that yalgo is not empty
                    if (YAlgOrder.Length > 0)
                    {
                        //calls the method that make the turns
                        ControlY();
                    }
                    else
                    {
                        //this is in case of an error, it makes algorder empty(in mose cases)
                        AlgOrder = AlgOrder.Substring(1);
                    }
                }
                else
                {
                    //check all other letters
                    CheckAlgOrder();
                }
            }
        }

        /// <summary>
        /// check for press of ctrl+z or ctrl +y
        /// </summary>
        /// <param name="keyboardState">current keyboard state</param>
        /// <param name="oldKeyboardState">old keyboard state</param>
        private void CheckPressOfCtrl(KeyboardState keyboardState, KeyboardState oldKeyboardState)
        {
            if ((keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl)))
            {
                //checks for a press of z
                if (keyboardState.IsKeyUp(Keys.Z) && oldKeyboardState.IsKeyDown(Keys.Z) && AllTimeAlgOrder.Length > 0)
                {
                    AlgOrder += "Z";
                }
                //checks for a press of y
                else if (keyboardState.IsKeyUp(Keys.Y) && oldKeyboardState.IsKeyDown(Keys.Y) && YAlgOrder.Length > 0)
                {
                    AlgOrder += "y";
                }
            }
        }

        /// <summary>
        /// some debug options for the programmer
        /// </summary>
        /// <param name="keyboardState">current keyboard state</param>
        /// <param name="oldKeyboardState">old keyboard state</param>
        /// <param name="cameraPos">the camera position as a 3d vector</param>
        private void CheckStuffInDebug(KeyboardState keyboardState, KeyboardState oldKeyboardState, Vector3 cameraPos)
        {
            //if user pressed q
            if (keyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                //make a /n line
                DebugBorders("");
            }
            //if user pressed w
            if (keyboardState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W))
            {
                //debug all the following info
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
        }

        /// <summary>
        /// make some changes to rotations left and debug other info
        /// </summary>
        private void StuffToDebugAfterRotation()
        {
            //set rotations left
            RotationsLeft = AlgOrder.Length;
            //check for algorder length without the I's
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
            //debug some info
            Debug.WriteLine("Number of rotations left:" + RotationsLeft);
            Debug.WriteLine("AllTimeAlgOrder " + AllTimeAlgOrder);
            Debug.WriteLine("YAlgOrder " + YAlgOrder);
        }

        /// <summary>
        /// checks if the first command in algorder is any of the 6 sides
        /// </summary>
        private void CheckAlgOrder()
        {
            // a list containing all sides as letters
            List<string> allSides = new List<string> { "L", "R", "U", "D", "F", "B" };
            //run through all the letters
            foreach (var s in allSides)
            {
                //if algorder is any of the letters than rotate that side
                if (AlgOrder[0].ToString() == s)
                {
                    //checks for an I
                    if (AlgOrder.Length > 1)
                    {
                        if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                        {
                            //rotate counterClockWise
                            cube.Rotate(CharToVector(s), false, AlgOrder);
                        }
                        else
                        {
                            //rotate clockWise
                            cube.Rotate(CharToVector(s), true, AlgOrder);
                        }
                    }
                    else
                    {
                        //rotate clockWise
                        cube.Rotate(CharToVector(s), true, AlgOrder);
                    }
                    YAlgOrder = "";
                }
            }
        }

        /// <summary>
        /// sets the oldState vars
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="keyboardState"></param>
        private void OldState(ref MouseState mouseState, ref KeyboardState keyboardState)
        {
            //set old states
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
            //update specific stuff for each state
            switch (CurrentGameState)
            {
                //if its the main menu
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                //if its the tutorial
                case GameState.Tutorial:
                    UpdateTutorial(keyboardState);
                    break;
                //if its the options
                case GameState.Options:
                    UpdateOptions(keyboardState, gameTime);
                    break;
                case GameState.FreePlay:
                    UpdateFreePlay(keyboardState, gameTime);
                    break;
            }
        }

        private void UpdateTutorial(KeyboardState keyboardState)
        {
            //check for click on the "escape" button which will return to the main menu
            if (keyboardState.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;
            if (IsUsingKeyboard)
            {
                if (keyboardState.IsKeyDown(Keys.U) && oldKeyboardState.IsKeyUp(Keys.U))
                {
                    CheckKeysTTR["Up"] = true;
                    AlgOrder += CharToVector("U");
                    AllTimeAlgOrder += CharToVector("U");
                }
                else if (keyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
                {
                    CheckKeysTTR["Down"] = true;
                    AlgOrder += CharToVector("D");
                    AllTimeAlgOrder += CharToVector("D");

                }
                else if (keyboardState.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R))
                {
                    CheckKeysTTR["Right"] = true;
                    AlgOrder += CharToVector("R");
                    AllTimeAlgOrder += CharToVector("R");

                }
                else if (keyboardState.IsKeyDown(Keys.L) && oldKeyboardState.IsKeyUp(Keys.L))
                {
                    CheckKeysTTR["Left"] = true;
                    AlgOrder += CharToVector("L");
                    AllTimeAlgOrder += CharToVector("L");
                }
                else if (keyboardState.IsKeyDown(Keys.F) && oldKeyboardState.IsKeyUp(Keys.F))
                {
                    CheckKeysTTR["Forward"] = true;
                    AlgOrder += CharToVector("F");
                    AllTimeAlgOrder += CharToVector("F");
                }
                else if (keyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
                {
                    CheckKeysTTR["Backwards"] = true;
                    AlgOrder += CharToVector("B");
                    AllTimeAlgOrder += CharToVector("B");
                }
            }
        }

        /// <summary>
        /// update of the freeplay state
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="gameTime"></param>
        private void UpdateFreePlay(KeyboardState keyboardState, GameTime gameTime)
        {
            //check for click on the "escape" button which will return to the main menu
            if (keyboardState.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;
            //check for click on the scramble button and flag it using the ShouldRotate bool
            if (button.BtnScramble.IsClicked) ShouldRotate = true;
            //check for click on the solve button
            if (button.BtnSolve.IsClicked)
            {
                //reset the stopper
                clocks.StopStoper();
                //set the angle of rotation to 0
                cube.Angle = 0;
                //stop the scrambling
                ShouldRotate = false;
                //reset algorder
                AlgOrder = "";
                //reset alltimealgo
                AllTimeAlgOrder = "";
                //reset yalgorder
                YAlgOrder = "";
                //reset the cube itself
                cube.Solve();
                //debug that a reset just happened
                DebugBorders("Reset!");
            }
            //necesary update of the textBox class
            textbox.Update(keyboardState, oldKeyboardState, gameTime, mono, AlgOrder, camera, cube);
            //checks whether should scramble or not
            if (ShouldRotate || solve.ShouldScramble)
            {
                //create the scramble sequenece
                cube.Scramble();
                //add the result to algorder
                AlgOrder += cube.ScrambleResult;
                //disable the flag that activates scramble
                ShouldRotate = false;
                solve.ShouldScramble = false;
            }
            //check for click on each of the stopper options
            CheckClickOnStopper();
            //update the stopper
            clocks.UpdateStoper(gameTime);
            //start the stopper
            if (shouldRunStopper)
                clocks.StartStoper();
            //update the scramble & solve buttons (needed to detect click)
            button.BtnScramble.Update(false, gameTime);
            button.BtnSolve.Update(false, gameTime);
        }

        /// <summary>
        /// udpate of the options state
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="gameTime"></param>
        private void UpdateOptions(KeyboardState keyboardState, GameTime gameTime)
        {
            //check for click on the russian button, and switch to russian if clicked
            if (button.BtnRussian.IsClicked) lang.Russian();
            //check for click on the hebrew button, and switch to hebrew if clicked
            if (button.BtnHebrew.IsClicked) lang.Hebrew();
            //check for click on the english button, and switch to english if clicked
            if (button.BtnEnglish.IsClicked) lang.English();
            //update all the said buttons (needed in order to check for click on them)
            button.BtnEnglish.Update(false, gameTime);
            button.BtnHebrew.Update(false, gameTime);
            button.BtnRussian.Update(false, gameTime);
            //check for click on the "add music" button
            CheckClickOnAddMusic();
            //update the music class
            music.Update();
            //check for click on the "escape" button which will return to the main menu
            if (keyboardState.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;
        }

        /// <summary>
        /// update of the main menu state
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateMainMenu(GameTime gameTime)
        {
            //check for click on the freePlay button
            if (button.BtnFreePlay.IsClicked)
            {
                //switch to freeplay
                CurrentGameState = GameState.FreePlay;
            }
            //check for click on the tutorial button
            if (button.BtnTutorial.IsClicked)
            {
                //switch to tutorial
                CurrentGameState = GameState.Tutorial;
            }
            //check for click on the options button
            if (button.BtnOptions.IsClicked)
            {
                //switch to options
                CurrentGameState = GameState.Options;
            }
            //update all the buttons (which checks for click every turn)
            button.BtnOptions.Update(false, gameTime);
            button.BtnTutorial.Update(false, gameTime);
            button.BtnFreePlay.Update(false, gameTime);
        }

        /// <summary>
        /// checks for click on the "add mouse" text in the options menu
        /// </summary>
        private void CheckClickOnAddMusic()
        {
            //create a rectangle which sorrunds the "add music" text
            Rectangle rect = new Rectangle((int)(graphicsDevice.Viewport.Width / 2f - font.MeasureString(lang.OptionsAddMusic).X / 2), 100, (int)font.MeasureString(lang.OptionsAddMusic).X, (int)font.MeasureString(lang.OptionsAddMusic).Y);
            //get the mouse state
            MouseState mouse = Mouse.GetState();
            //checks for click of the mouse
            if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                //checks if the mouse is inside the rectangle
                if (rect.Contains(mousePos))
                {
                    //open the addMusic popup
                    music.AddMusic();
                }
            }
        }

        /// <summary>
        /// check for click on any of the stopper options in the freeplay state
        /// </summary>
        private void CheckClickOnStopper()
        {
            //gets the mouse state
            MouseState mouse = Mouse.GetState();

            //get the mouse position
            Point mousePoint = new Point(mouse.X, mouse.Y);

            //sets the locaton of the begining of the rectangles
            int locX = (int)(graphicsDevice.Viewport.Width / 1.31);
            int locY = (int)(graphicsDevice.Viewport.Height / 2f);

            //create each of the rectangles 
            Rectangle showStopperRect = new Rectangle(locX, locY, (int)font.MeasureString(lang.FreePlayStopperShow).X, (int)font.MeasureString(lang.FreePlayStopperShow).Y);
            Rectangle pauseStopperRect = new Rectangle(locX, 50 + locY, (int)font.MeasureString(lang.FreePlayStopperPause).X, (int)font.MeasureString(lang.FreePlayStopperPause).Y);
            Rectangle resumeStopperRect = new Rectangle(locX, 100 + locY, (int)font.MeasureString(lang.FreePlayStopperResume).X, (int)font.MeasureString(lang.FreePlayStopperResume).Y);
            Rectangle resetStopperRect = new Rectangle(locX, 150 + locY, (int)font.MeasureString(lang.FreePlayStopperReset).X, (int)font.MeasureString(lang.FreePlayStopperReset).Y);

            //check if the mouse was clicked
            if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                //checks if the mouse was on the show/hide rectangle
                if (showStopperRect.Contains(mousePoint))
                {
                    //make the stopper show/hide, opposite of what it was before
                    shouldShowStopper = !shouldShowStopper;
                    //disable the stopper if it was running beforehand (case of hiding the stopper)
                    if (shouldRunStopper)
                    {
                        shouldRunStopper = false;
                        clocks.PauseStoper();
                    }
                    //make the stopper run again (case of showing the stopper)
                    else
                    {
                        shouldRunStopper = true;
                        clocks.ResumeStoper();
                    }
                }
                //check for click on the pause rectangle, as well as if the stopper is runing
                else if (pauseStopperRect.Contains(mousePoint) && shouldRunStopper)
                {
                    //pause the stopper
                    shouldRunStopper = false;
                    clocks.PauseStoper();
                }
                //checks for click on the resume rectangle, as well as if the stopper was not runing, and visible
                else if (resumeStopperRect.Contains(mousePoint) && !shouldRunStopper && shouldShowStopper)
                {
                    //reactivate the stopper
                    shouldRunStopper = true;
                    clocks.ResumeStoper();
                }
                //checks for click on the reset rectangle
                else if (resetStopperRect.Contains(mousePoint))
                {
                    //reset the stopper
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
                //if its the main menu
                case GameState.MainMenu:
                    DrawMainMenu(spriteBatch);
                    break;
                //if its free play
                case GameState.FreePlay:
                    DrawFreePlay(spriteBatch);
                    break;
                case GameState.Options:
                    DrawOptions(spriteBatch);
                    break;
                case GameState.Tutorial:
                    spriteBatch.Begin();
                    //draw the title
                    spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    //draw the instructions
                    spriteBatch.DrawString(font, lang.TutorialFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    spriteBatch.DrawString(font, lang.TutorialFreeText2, new Vector2(graphicsDevice.Viewport.Width / 3f, 90), Color.Black);
                    spriteBatch.End();
                    break;
            }
        }

        private void DrawOptions(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //draw the russian button
            button.BtnRussian.Draw(spriteBatch);
            //draw the hebrew button
            button.BtnHebrew.Draw(spriteBatch);
            //draw the english button
            button.BtnEnglish.Draw(spriteBatch);
            //draw the title
            spriteBatch.DrawString(font, lang.OptionsTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            //draw the "add music"
            spriteBatch.DrawString(font, lang.OptionsAddMusic,
                new Vector2(graphicsDevice.Viewport.Width / 2f - font.MeasureString(lang.OptionsAddMusic).X / 2, 100), Color.Black);
            //draw the word "english" above the button
            spriteBatch.DrawString(font, "English", new Vector2(graphicsDevice.Viewport.Width / 2.5f, 440), Color.Black);
            //draw the word "hebrew" above the button
            spriteBatch.DrawString(font, "ת י ר ב ע", new Vector2(graphicsDevice.Viewport.Width / 1.85f, 440), Color.Black);
            //draw the word "russian" above the button
            spriteBatch.DrawString(font, "Russian", new Vector2(graphicsDevice.Viewport.Width / 1.55f, 440), Color.Black);
            spriteBatch.End();
        }

        private void DrawMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            //draw the title
            spriteBatch.DrawString(font, lang.MainTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            //draw the tutorial button
            button.BtnTutorial.Draw(spriteBatch);
            //draw the options button
            button.BtnOptions.Draw(spriteBatch);
            //draw the free play button
            button.BtnFreePlay.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawFreePlay(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //draw the title
            spriteBatch.DrawString(font, lang.FreePlayTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            //draw the "scramble"
            spriteBatch.DrawString(font, lang.FreePlayScramble,
                new Vector2(graphicsDevice.Viewport.Width / 13f, graphicsDevice.Viewport.Height / 1.4f), Color.Black);
            //draw the "solve"
            spriteBatch.DrawString(font, lang.FreePlaySolve,
                new Vector2(graphicsDevice.Viewport.Width / 4f, graphicsDevice.Viewport.Height / 1.4f), Color.Black);
            //draw the show/hide stopper
            spriteBatch.DrawString(font, lang.FreePlayStopperShow,
                new Vector2(graphicsDevice.Viewport.Width / 1.35f, graphicsDevice.Viewport.Height / 2f), Color.Black);
            //draw the pause stopper                    
            spriteBatch.DrawString(font, lang.FreePlayStopperPause,
                new Vector2(graphicsDevice.Viewport.Width / 1.35f, 50 + graphicsDevice.Viewport.Height / 2f), Color.Black);
            //draw the resume stopper
            spriteBatch.DrawString(font, lang.FreePlayStopperResume,
                new Vector2(graphicsDevice.Viewport.Width / 1.35f, 100 + graphicsDevice.Viewport.Height / 2f), Color.Black);
            //draw the reset stopper
            spriteBatch.DrawString(font, lang.FreePlayStopperReset,
                new Vector2(graphicsDevice.Viewport.Width / 1.35f, 150 + graphicsDevice.Viewport.Height / 2f), Color.Black);
            //draw the stppper
            if (shouldShowStopper)
                clocks.DrawStoper(spriteBatch, font, new Vector2(graphicsDevice.Viewport.Width / 3f, 30));
            //draw the scramble button
            button.BtnScramble.Draw(spriteBatch);
            //draw the solve button
            button.BtnSolve.Draw(spriteBatch);
            spriteBatch.End();
            //draw the cube itself
            DrawModel(cube, world, view, projection);
            //draw the textbox and anything in it
            textbox.Draw(spriteBatch, mono, font, textTex);
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
            //call the specific draw for each game state
            SwitchDraw(spriteBatch);
        }

        /// <summary>
        /// the main update
        /// </summary>
        /// <param name="gameTime">the gameTime from Main</param>
        /// <param name="graphicsDeviceFromMain">the graphicsDevice from main</param>
        /// <param name="_music">the music from main</param>
        public void Update(GameTime gameTime, GraphicsDevice graphicsDeviceFromMain, Music _music)
        {
            //set the graphics device from main
            graphicsDevice = graphicsDeviceFromMain;
            //set the music from main            
            music = _music;
            //get the keyboard and mouse states
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            //set the view matrix to be the one from the camera class
            view = camera.View;
            //convert the position of the camera to a 3d vector
            cameraPos = Matrix.Invert(view).Translation;
            //
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

        public void ControlZ()
        {
            if (AllTimeAlgOrder.Length > 0)
            {
                Debug.WriteLine(cube.Angle);
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
                Debug.WriteLine(cube.Angle);
                int num = 0;
                int length = YAlgOrder.Length - 1;
                bool counterClockWise = true;
                if (YAlgOrder.Length > 1)
                {
                    if (((YAlgOrder[YAlgOrder.Length - 2] == 'I') || (YAlgOrder[YAlgOrder.Length - 2] == 'i')))
                    {
                        counterClockWise = false;
                    }
                }
                if (YAlgOrder[length] == 'L')
                {
                    cube.Rotate(Vector3.Left, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[length] == 'R')
                {
                    cube.Rotate(Vector3.Right, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[length] == 'U')
                {
                    cube.Rotate(Vector3.Up, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[length] == 'D')
                {
                    cube.Rotate(Vector3.Down, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[length] == 'F')
                {
                    cube.Rotate(Vector3.Forward, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[length] == 'B')
                {
                    cube.Rotate(Vector3.Backward, counterClockWise, AlgOrder);
                }
                //if (cube.Angle == -100 && YAlgOrder.Length == 3)
                //{
                //    if (YAlgOrder[1] == 'I' || YAlgOrder[1] == 'I')
                //        YAlgOrder = YAlgOrder[0].ToString() + YAlgOrder[2];
                //}
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
                            AlgOrder += "DI";
                        else if (centerOfClickedMesh.Y > 4)
                        {
                            AlgOrder += "UI";
                        }
                    }
                    else
                    {
                        //rotation left
                        if (centerOfClickedMesh.Y < 3)
                            AlgOrder += "D";
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