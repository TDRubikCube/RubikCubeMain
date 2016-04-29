using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RubikCube
{
    class Tutorial
    {
        //call the text class to draw the text
        private readonly Text lang;

        //create a font instance
        private readonly SpriteFont font;

        //call the graphics device
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// create a new instance of tutorail, made to explain the ways of using the program
        /// </summary>
        /// <param name="content"></param>
        public Tutorial(ContentManager content)
        {
            //initialize the text class
            lang = new Text();

            //load the font file
            font = content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// the main update
        /// </summary>
        /// <param name="graphicsDevicsFromMain"></param>
        public void Update(GraphicsDevice graphicsDevicsFromMain)
        {
            graphicsDevice = graphicsDevicsFromMain;
        }

        /// <summary>
        /// the main draw which shows the instructions
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //draw the title
            spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            
            //draw the instructions
            spriteBatch.DrawString(font, lang.TutorialFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
            spriteBatch.DrawString(font, lang.TutorialFreeText2, new Vector2(graphicsDevice.Viewport.Width / 3f, 90), Color.Black);
            spriteBatch.End();
        }
    }
}