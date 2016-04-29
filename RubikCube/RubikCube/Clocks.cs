using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RubikCube
{
    class Clocks
    {
        //the logical timer 
        private float timer;
        
        //the miliseconds of the stopper
        private int mSeconds;
        
        //the seconds of the stopper
        private int seconds;
        
        //the minutes of the stopper
        private int minutes;
        
        //the hours of the stopper
        private int hours;
        
        // the hours to dsiplay
        string displayHours;
        
        //the seconds to display
        string displayMinutes;
        
        //the seconds to display
        string displaySeconds;
        
        //the miliseconds to display
        string displayMSseconds;
        
        //marks if the stopper is paused
        private bool isStoperPaused;
        
        //marks whether the stopper should run
        private bool shouldStartStoper;

        /// <summary>
        /// sets the timer according to the intervel
        /// </summary>
        /// <param name="intervel">the intervel between "rings" of the timer</param>
        public void InitTimer(int intervel)
        {
            timer = intervel;            
        }

        /// <summary>
        /// the timer function
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
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
