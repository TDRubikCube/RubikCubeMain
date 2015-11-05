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

namespace RubikCube
{
    class Clocks
    {
        private float timer;
        private bool shouldStartStoper;
        private int mSeconds;
        private int seconds;
        private int minutes;
        private int hours;
        string displayHours;
        string displayMinutes;
        string displaySeconds;
        string displayMSseconds;
        private bool isStoperPaused;

        public void InitTimer(int intervel)
        {
            timer = intervel;            
        }

        public bool CallTimer(GameTime gameTime)
        {
            float elpased = gameTime.ElapsedGameTime.Milliseconds;
            timer -= elpased;
            if (timer < 0)
            {
                timer = 500;
                return true;
            }
            return false;
        }

        public void UpdateStoper(GameTime gameTime)
        {
            //init display
            displayHours = hours.ToString();
            displayMinutes = minutes.ToString();
            displaySeconds = seconds.ToString();
            displayMSseconds = mSeconds.ToString();

            //add zeroes to each value if the num of digits is smaller than its max value
            if (hours < 10)
                displayHours = '0' + hours.ToString();
            if (minutes < 10)
                displayMinutes = '0' + minutes.ToString();
            if (seconds < 10)
                displaySeconds = '0' + seconds.ToString();
            if (mSeconds < 100)
            {
                displayMSseconds = '0'.ToString();
                if (mSeconds < 10)
                {
                    displayMSseconds += '0';
                }
                displayMSseconds += mSeconds;
            }

            //start the logic of the stoper, adding time to each value accordingly
            if (shouldStartStoper)
            {
                mSeconds += gameTime.ElapsedGameTime.Milliseconds;
                if (mSeconds >= 1000)
                {
                    mSeconds = 0;
                    seconds++;
                }
                if (seconds >= 60)
                {
                    seconds = 0;
                    minutes++;
                }
                if (minutes >= 60)
                {
                    minutes = 0;
                    hours++;
                }
            }
        }

        public void DrawStoper(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            //draw the stoper
            spriteBatch.DrawString(font, displayHours + ":" + displayMinutes + ":" + displaySeconds + ":" + displayMSseconds, position, Color.Black);
        }

        public void StartStoper()
        {
            if (!isStoperPaused)
                shouldStartStoper = true;
        }

        public void StopStoper()
        {
            shouldStartStoper = false;
            mSeconds = 0;
            seconds = 0;
            minutes = 0;
            hours = 0;
        }

        public void PauseStoper()
        {
            isStoperPaused = true;
            shouldStartStoper = false;
        }

        public void ResumeStoper()
        {
            if (isStoperPaused)
            {
                shouldStartStoper = true;
                isStoperPaused = false;
            }
        }
    }
}
