using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RubikCube
{
    /// <summary>
    /// Responsible for all clocks, timers and stopers in the program
    /// </summary>
    class Clocks
    {
        //The logical timer 
        private float timer;
        
        //The miliseconds of the stopper
        private int mSeconds;
        
        //The seconds of the stopper
        private int seconds;
        
        //The minutes of the stopper
        private int minutes;
        
        //The hours of the stopper
        private int hours;
        
        //The hours to dsiplay
        string displayHours;
        
        //The seconds to display
        string displayMinutes;
        
        //The seconds to display
        string displaySeconds;
        
        //The miliseconds to display
        string displayMSseconds;
        
        //Marks if the stopper is paused
        private bool isStoperPaused;
        
        //Marks whether the stopper should run
        private bool shouldStartStoper;

        /// <summary>
        /// Sets the timer according to the intervel
        /// </summary>
        /// <param name="intervel">The intervel between "rings" of the timer</param>
        public void InitTimer(int intervel)
        {
            timer = intervel;            
        }

        /// <summary>
        /// The timer function
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool CallTimer(GameTime gameTime)
        {
            float elpased = gameTime.ElapsedGameTime.Milliseconds; //Sets elpased, depending on gameTime 
           
            //decrease time from the timer
            timer -= elpased;

            //if the timer finished
            if (timer < 0)
            {
                //reset and mark as finished
                timer = 500;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the stoper tyo show the right time
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateStoper(GameTime gameTime)
        {
            //Init display
            displayHours = hours.ToString();
            displayMinutes = minutes.ToString();
            displaySeconds = seconds.ToString();
            displayMSseconds = mSeconds.ToString();

            //Add zeroes to each value if the num of digits is smaller than its max value
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

            //Start the logic of the stoper, adding time to each value accordingly
            if (shouldStartStoper)
            {
                mSeconds += gameTime.ElapsedGameTime.Milliseconds;
                //Adds seconds
                if (mSeconds >= 1000)
                {
                    mSeconds = 0;
                    seconds++;
                }
                //Adds minutes
                if (seconds >= 60)
                {
                    seconds = 0;
                    minutes++;
                }
                //Adds hours
                if (minutes >= 60)
                {
                    minutes = 0;
                    hours++;
                }
            }
        }
        /// <summary>
        /// draws the stoper
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="font">Regular Font</param>
        /// <param name="position">The position of the stoper</param>
        public void DrawStoper(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            spriteBatch.DrawString(font, displayHours + ":" + displayMinutes + ":" + displaySeconds + ":" + displayMSseconds, position, Color.Black);
        }
        /// <summary>
        /// Starts the stoper
        /// </summary>
        public void StartStoper()
        {
            if (!isStoperPaused)
                shouldStartStoper = true;
        }

        /// <summary>
        /// Stops the stoper, resets all of its values
        /// </summary>
        public void StopStoper()
        {
            shouldStartStoper = false;
            mSeconds = 0;
            seconds = 0;
            minutes = 0;
            hours = 0;
        }
        /// <summary>
        /// Pauses the stoper
        /// </summary>
        public void PauseStoper()
        {
            isStoperPaused = true;
            shouldStartStoper = false;
        }
        /// <summary>
        /// Resumes the stoper after it was paused
        /// </summary>
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
