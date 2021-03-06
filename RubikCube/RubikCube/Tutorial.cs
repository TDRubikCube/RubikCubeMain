﻿using System;
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

namespace RubikCube
{
    class Tutorial
    {
        private readonly Text lang;
        private readonly SpriteFont font;
        private GraphicsDevice graphicsDevice;

        public Tutorial(ContentManager content)
        {
            var welcome = new Welcome();
            welcome.Show();
            lang = new Text();
            font = content.Load<SpriteFont>("font");
        }

        public void Update(GraphicsDevice graphicsDevicsFromMain)
        {
            graphicsDevice = graphicsDevicsFromMain;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
            spriteBatch.DrawString(font, lang.TutorialFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
            spriteBatch.DrawString(font, lang.TutorialFreeText2, new Vector2(graphicsDevice.Viewport.Width / 3f, 90), Color.Black);
            spriteBatch.End();
        }
    }
}
