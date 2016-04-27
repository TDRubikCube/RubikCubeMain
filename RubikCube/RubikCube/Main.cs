using System;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace RubikCube
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        //defines classes and other xna-unique vars
        #region classes & xna.vars declaration

        //in charge of the graphics of the game
        readonly GraphicsDeviceManager graphics;

        //allows to draw images and text
        SpriteBatch spriteBatch;

        //calls the music class, in charge of playing music and adding music
        Music music;

        //calls the clocks class, in charge of logical timer and the graphic stopper
        private Clocks clocks;

        //calls the loading screen logic, which is the screen the user sees while the loading thread loads all content
        LoadingScreen loading;
        
        //create the thread that loads all content
        Thread loadingThread;

        //calls the ButtunSetUp class in charge of defining properties of each of the buttons in the game
        ButtonSetUp button;

        //calls the SwitchGameState class, in charge of controlling the different states of the game and of
        //each uniqe feature each state has as well as sending information between them
        SwitchGameState gameState;

        //calls the SaveGame class, in charge of saving the state of the cube and various other details
        private SaveGame save;

        private Tutorial tutorial;

        #endregion

        //defines normal vars (anything thats not on the above region)
        #region normal vars

        //checks if the loading thread finished
        public bool IsDoneLoading;

        //checks if the thread finished loading on this frame
        bool justFinshed;

        //checks if the current run of the game is the first one of the user
        private bool isFirstTime = true;

        //define the constant scale of the cube, and the size of one cubie
        public const float OriginalScale = 3.4524f;
        public const float CubieSize = 1.918f;

        #endregion

        /// <summary>
        /// main constructor of the game
        /// </summary>
        public Main()
        {
            //define the graphics used for the game
            graphics = new GraphicsDeviceManager(this);

            //defines the content directory from which the game will load content
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //unlimited fps
            IsFixedTimeStep = false;

            //turns the mouse visible withing the game
            IsMouseVisible = true;
            
            //doesnt allow the user to resize
            Window.AllowUserResizing = false;
            
            //changes the title of the game's window
            Window.Title = "Best Rubik's Cube Game";
            
            //initalizes the loading screen class
            loading = new LoadingScreen(Content);
            
            //starts the loading thread
            loadingThread = new Thread(Load);
            loadingThread.Start();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //loads the basic spriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //sets the mouse handle
            Mouse.WindowHandle = Window.Handle;

            //loading logic
            //checks if the thread has finihed
            if (!loadingThread.IsAlive)
            {
                //checks if the thread finished on this frame
                if ((justFinshed) && (!IsDoneLoading))
                {
                    //creates new instance of SaveGame with the specified folder as the path to save to
                    save = new SaveGame("C:/Users/" + Environment.UserName + "/Documents/RubikCube/save.xml", "root");

                    //marks loading as complete
                    IsDoneLoading = true;
                }

                //load bools from save file
                foreach (Tuple<string, string> b in save.LoadBools())
                {
                    //changes the isFirstTime acording to the save file
                    if (b.Item1 == "isFirstTime")
                        isFirstTime = Convert.ToBoolean(b.Item2);
                }

                // checks if the loading & the Clocks are done
                if (justFinshed && clocks.CallTimer(gameTime))
                {
                    //turn music back on
                    MediaPlayer.Resume();
                    //if its the first time the user uses the program, saves it to the save file
                    if (isFirstTime)
                    {
                        save.AddBool("isFirstTime", "false");
                    }
                    //marks the fact that this is no longer the frame on which the loading finished
                    justFinshed = false;
                }

                //neccesary updates
                button.BtnMute.Update(true, gameTime);
                button.BtnUnMute.Update(true, gameTime);
                MuteEvent();
                gameState.Update(gameTime, GraphicsDevice, music);
                if (gameState.ShouldActivateTutorial)
                {
                    tutorial.Update(gameTime);
                }
            }
            else
            {
                //loading animation
                loading.Update(gameTime);

                //pause media player to prevent bugs
                if (MediaPlayer.State != MediaState.Paused)
                    MediaPlayer.Pause();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //turn the background white
            GraphicsDevice.Clear(Color.White);
            //if the loading finished
            if (!loadingThread.IsAlive)
            {
                //draw main game components
                gameState.Draw(spriteBatch);

                if (gameState.ShouldActivateTutorial)
                {
                    tutorial.Draw(spriteBatch);
                }

                spriteBatch.Begin();
                //draw mute/unmute button
                if (music.IsMuted)
                    button.BtnMute.Draw(spriteBatch);
                else
                    button.BtnUnMute.Draw(spriteBatch);

                spriteBatch.End();
            }
            //draw loading animation
            else
                loading.Draw(spriteBatch, GraphicsDevice);
            base.Draw(gameTime);
        }

        //contains all methods used on these class
        #region methods

        /// <summary>
        /// load all files
        /// </summary>
        private void Load()
        {
            //loads the main classes
            gameState = new SwitchGameState(GraphicsDevice, graphics, Content, music);
            music = new Music(graphics, GraphicsDevice, Content);
            button = new ButtonSetUp(GraphicsDevice, Content);
            clocks = new Clocks();
            tutorial = new Tutorial(GraphicsDevice,gameState,Content,music);
            
            //sets the timer to 200 ms
            clocks.InitTimer(200);
            try
            {
                //this is in "try" since its experimantal

                //set the path to the script
                string path = "C:/Users/" + Environment.UserName + "/Desktop/script.vbs";
                
                // creates a new process
                Process runScript = new Process();
                
                //set the script path into the process
                runScript.StartInfo.FileName = (@path);
                
                //run the script
                runScript.Start();
                
                //wait for the script to finish
                runScript.WaitForExit();
                
                //close the process
                runScript.Close();
            }
            catch (Exception)
            {

            }

            //marks that the loading thread finished this frame
            justFinshed = true;
        }

        /// <summary>
        /// mute/unMute button logic
        /// </summary>
        private void MuteEvent()
        {
            //checks if the user clicked on the unMute button while the music was playing
            if (button.BtnUnMute.IsClicked && MediaPlayer.State == MediaState.Playing)
            {
                //muthe music
                music.IsMuted = true;
                MediaPlayer.Pause();
            }
            //checks if the user clicked on the mute button while the music was paused
            else if (button.BtnMute.IsClicked && MediaPlayer.State == MediaState.Paused)
            {
                //resume music
                music.IsMuted = false;
                MediaPlayer.Resume();
            }
        }

        #endregion
    }
}