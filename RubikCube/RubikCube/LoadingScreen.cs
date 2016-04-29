using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RubikCube
{
    class LoadingScreen
    {
        //calls the font file
        readonly SpriteFont font;

        //counts the number of dots currently shown
        int totalDots = 1;

        //checks if the Draw should start drawing the dots
        public bool ShouldStart;

        //creates the limit of the timer, meaning after 500 ms, the timer will 
        //logically go off
        float timer = 500;

        /// <summary>
        /// constructor of the loading animation
        /// </summary>
        /// <param name="content">main content, needed in order to show text</param>
        public LoadingScreen(ContentManager content)
        {
            //load the font file
            font = content.Load<SpriteFont>("font");

            //sets the shouldStart as false
            ShouldStart = false;
        }

        /// <summary>
        /// The main draw feature of this class. in charge of all the drawing of the dots
        /// including the timer
        /// </summary>
        /// <param name="spriteBatch">Needs in order to draw the text</param>
        /// <param name="graphicsDevice">Needs in order to use the timer</param>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();

            //draw the word "loading" with the first dot
            spriteBatch.DrawString(font, "loading.", new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 14f), Color.Black);

            //sets the ShouldStart as true
            ShouldStart = true;

            //loops through drawing the amount of dots currently shown
            for (int i = 1; i < totalDots; i++)
            {

                //sets the max number of dots to j
                int j = 3;

                //if it reached the max number of dots, set j to go before the first dot
                if (i == 2)
                    j -= 4;

                //draw the dot
                spriteBatch.DrawString(font, ".", new Vector2(graphicsDevice.Viewport.Width / 1.68f - j, graphicsDevice.Viewport.Height / 14f), Color.Black);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// The main Update of the loading screen, in charge of updating the timer and the number of dots
        /// </summary>
        /// <param name="gameTime">needed to count the time</param>
        public void Update(GameTime gameTime)
        {
            //gets the time that passed between two frames
            float elpased = gameTime.ElapsedGameTime.Milliseconds;

            //subtract said number from the max time (500 ms as set above)
            timer -= elpased;

            //if timer reached bellow zero, the timer goes off
            if (timer < 0)
            {

                //if the number of dots is less than 3, add a dot
                if (totalDots < 3)
                {
                    //add a dot
                    totalDots++;
                }
                //reset the number of dots
                else
                    totalDots = 1;
                //reset the timer
                timer = 500;
            }
        }
    }
}
