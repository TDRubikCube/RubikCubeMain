using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RubikCube
{
    class ButtonSetUp
    {
        #region buttons declration
        //tutorial button from main menu
        public Button BtnTutorial;

        //free play button from main menu
        public Button BtnFreePlay;

        //options button from main menu
        public Button BtnOptions;

        //mute button
        public Button BtnMute;

        //numute button
        public Button BtnUnMute;

        //USA flag representing the switch to english
        public Button BtnEnglish;

        //isral flag representing the switch to hebrew
        public Button BtnHebrew;

        //russia flag representing the switch to russian
        public Button BtnRussian;

        //the scramble button which activates the cube scramble
        public Button BtnScramble;

        //the reset button
        public Button BtnSolve;
        #endregion

        /// <summary>
        /// Define every property of each button
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="content"></param>
        public ButtonSetUp(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, ContentManager content)
        {
            //sets the texture for each of the buttons
            #region texture initialize
            Texture2D scrambleButton = content.Load<Texture2D>("pics/scramble");
            Texture2D solveButton = content.Load<Texture2D>("pics/solved");
            Texture2D tutorialButton = content.Load<Texture2D>("pics/Tutorial");
            Texture2D freePlayButton = content.Load<Texture2D>("pics/FreePlay");
            Texture2D optionsButton = content.Load<Texture2D>("pics/Options");
            Texture2D muteButton = content.Load<Texture2D>("pics/Mute");
            Texture2D unMuteButton = content.Load<Texture2D>("pics/unMute");
            Texture2D englishButton = content.Load<Texture2D>("pics/english");
            Texture2D hebrewButton = content.Load<Texture2D>("pics/hebrew");
            Texture2D russianButton = content.Load<Texture2D>("pics/russian");
            #endregion

            //sets the positon and size of each of the buttons (whenever the size is not specified it will be defined as the default size)
            #region buttons initialize
            //tutorial button
            BtnTutorial = new Button(tutorialButton, graphics.GraphicsDevice);
            BtnTutorial.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 2.5f, graphicsDevice.Viewport.Height / 4.5f));

            //freeplay button
            BtnFreePlay = new Button(freePlayButton, graphics.GraphicsDevice);
            BtnFreePlay.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 2.5f, graphicsDevice.Viewport.Height / 2.3f));

            //options button
            BtnOptions = new Button(optionsButton, graphics.GraphicsDevice);
            BtnOptions.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 2.7f, graphicsDevice.Viewport.Height / 1.5f));
            BtnOptions.Size = new Vector2(graphicsDevice.Viewport.Width / 4f, graphicsDevice.Viewport.Height / 4f);

            //mute button
            BtnMute = new Button(muteButton, graphics.GraphicsDevice);
            BtnMute.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 1.09f, graphicsDevice.Viewport.Height / 30f));
            BtnMute.Size = new Vector2(graphicsDevice.Viewport.Width / 12f, graphicsDevice.Viewport.Height / 11f);

            //unMute button
            BtnUnMute = new Button(unMuteButton, graphics.GraphicsDevice);
            BtnUnMute.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 1.09f, graphicsDevice.Viewport.Height / 30f));
            BtnUnMute.Size = new Vector2(graphicsDevice.Viewport.Width / 12.8f, graphicsDevice.Viewport.Height / 11.8f);

            //english button
            BtnEnglish = new Button(englishButton, graphics.GraphicsDevice);
            BtnEnglish.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 2.5f, graphicsDevice.Viewport.Height / 1.25f));
            BtnEnglish.Size = new Vector2(graphicsDevice.Viewport.Width / 10f, graphicsDevice.Viewport.Height / 10f);

            //hebrew button
            BtnHebrew = new Button(hebrewButton, graphics.GraphicsDevice);
            BtnHebrew.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 1.9f, graphicsDevice.Viewport.Height / 1.25f));
            BtnHebrew.Size = new Vector2(graphicsDevice.Viewport.Width / 10f, graphicsDevice.Viewport.Height / 10f);

            //russian button
            BtnRussian = new Button(russianButton, graphics.GraphicsDevice);
            BtnRussian.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 1.55f, graphicsDevice.Viewport.Height / 1.25f));
            BtnRussian.Size = new Vector2(graphicsDevice.Viewport.Width / 10f, graphicsDevice.Viewport.Height / 10f);

            //scramble button
            BtnScramble = new Button(scrambleButton, graphics.GraphicsDevice);
            BtnScramble.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 20f, graphicsDevice.Viewport.Height / 1.25f));
            BtnScramble.Size = new Vector2(graphicsDevice.Viewport.Width / 6f, graphicsDevice.Viewport.Height / 5f);

            //solved button
            BtnSolve = new Button(solveButton, graphics.GraphicsDevice);
            BtnSolve.SetPosition(new Vector2(graphicsDevice.Viewport.Width / 4.5f, graphicsDevice.Viewport.Height / 1.25f));
            BtnSolve.Size = new Vector2(graphicsDevice.Viewport.Width / 7f, graphicsDevice.Viewport.Height / 5f);
            #endregion
        }
    }
}