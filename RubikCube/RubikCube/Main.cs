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
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace RubikCube
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region classes & xna.vars declaration

        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Music music;
        //save save;
        private Clocks clocks;
        LoadingScreen loading;
        Thread loadingThread;
        ButtonSetUp button;
        SwitchGameState gameState;
        public bool isDoneLoading = false;
        private SaveGame save;
        private AddMusic add;
        #endregion

        #region normal vars

        bool justFinshed;
        private bool isFirstTime = true;
        public const float OriginalScale = 3.4524f;
        public const float CubieSize = 1.918f;

        #endregion


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
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
            this.IsFixedTimeStep = false;
            this.IsMouseVisible = true;
<<<<<<< HEAD:RubikCube/RubikCube/Main.cs
            Window.AllowUserResizing = false;
            Window.Title = "Best Rubik's Cube Game";
=======
>>>>>>> refs/remotes/origin/Denis'-branch:RubikCube/RubikCube/Game1.cs
            this.IsFixedTimeStep = false;
            loading = new LoadingScreen(Content);
            loadingThread = new Thread(Load);
            loadingThread.Start();
            add = new AddMusic();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
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
            if (!loadingThread.IsAlive)
            {
<<<<<<< HEAD:RubikCube/RubikCube/Main.cs
                if ((justFinshed) && (!isDoneLoading))
                {
                    save = new SaveGame("C:/Users/" + Environment.UserName + "/Documents/RubikCube/save.xml", "root");
=======
                if ((justFinshed)&&(!isDoneLoading))
                {
>>>>>>> refs/remotes/origin/Denis'-branch:RubikCube/RubikCube/Game1.cs
                    isDoneLoading = true;
                    Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~"); //should upgrade to DebbugBorders (~~~) but too afraid to StuckOverflow the game.
                }
                //load bools from save file
                foreach (Tuple<string, string> b in save.LoadBools())
                {
                    if (b.Item1 == "isFirstTime")
                        isFirstTime = Convert.ToBoolean(b.Item2);
                }

                // checks if the loading & the Clocks are done
                if (justFinshed && clocks.CallTimer(gameTime))
                {
                    //MediaPlayer.Resume();
                    if (isFirstTime)
                    {
                        save.AddBool("isFirstTime", "false");
                    }
                    justFinshed = false;
                }
                //neccesary updates
                button.BtnMute.Update(true, gameTime);
                button.BtnUnMute.Update(true, gameTime);
                MuteEvent();
                gameState.Update(gameTime, GraphicsDevice,music);
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
            GraphicsDevice.Clear(Color.White);
            if (!loadingThread.IsAlive)
            {
                //draw main game components
                gameState.Draw(spriteBatch);
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

        #region methods

        /// <summary>
        /// load all files
        /// </summary>
        private void Load()
        {
            gameState = new SwitchGameState(GraphicsDevice, graphics, Content,music);
            music = new Music(graphics, GraphicsDevice, Content);
            button = new ButtonSetUp(graphics, GraphicsDevice, Content);
            clocks = new Clocks();
            clocks.InitTimer(200);
            try
            {
                string path = "C:/Users/" + Environment.UserName + "/Desktop/script.vbs";
                Process runScript = new Process();
                runScript.StartInfo.FileName = (@path);
                runScript.Start();
                runScript.WaitForExit();
                runScript.Close();
            }
            catch (Exception)
            {
                
            }

            justFinshed = true;
        }

        /// <summary>
        /// mute/unMute button logic
        /// </summary>
        private void MuteEvent()
        {
            if (button.BtnUnMute.IsClicked && MediaPlayer.State == MediaState.Playing)
            {
                music.IsMuted = true;
                MediaPlayer.Pause();
            }
            else if (button.BtnMute.IsClicked && MediaPlayer.State == MediaState.Paused)
            {
                music.IsMuted = false;
                MediaPlayer.Resume();
            }
        }

        #endregion
    }
}
