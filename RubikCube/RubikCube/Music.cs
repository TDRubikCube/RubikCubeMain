using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RubikCube
{
    public class Music
    {
        //create the default song in case the user didnt put songs
        private readonly Song defaultSong;

        //marks whether its muted or not
        public bool IsMuted = false;

        //add music and play it
        private AddMusic add;

        //old mouse state
        private MediaState oldState;

        /// <summary>
        /// Plays the first song the user added, or the default song if he didnt add
        /// </summary>
        /// <param name="content"></param>
        public Music(ContentManager content)
        {
            //initialize the addmusic popup
            add = new AddMusic();

            //set the default song
            defaultSong = content.Load<Song>("music/bg-music");
            //this is in "try" since it is expreimental and unstable
            try
            {
                //checks if the "songs" directory exists
                if (Directory.Exists("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs"))
                {
                    //get all the files from the songs dir
                    string[] files =
                        Directory.GetFiles("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs",
                            "*", SearchOption.TopDirectoryOnly);

                    //number of songs 
                    int fCount = files.Length;

                    //if any songs exist
                    if (fCount > 0)
                    {
                        //get the name of the song (the "files" list gives full address, so this gets only the name of the song itself)
                        string name = files[0].Substring(36 + Environment.UserName.Length,
                            files[0].Length - (36 + Environment.UserName.Length));

                        //creates a new constructor of the song class
                        var ctor = typeof(Song).GetConstructor(
                            BindingFlags.NonPublic | BindingFlags.Instance, null, // set the neccessary definitions of the class as the ones specific for the song class
                            new[] { typeof(string), typeof(string), typeof(int) }, null); // allows to give the sone name, the path to it, and the duration of the song

                        //set the current song
                        Song currSong =
                            (Song)
                                ctor.Invoke(new object[] // call the constructor made earlier
                                {
                                    name, // give the name of the current song
                                    @"c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" + name, // sets the path to the song
                                    0 // set the duration as 0 because it wont be used anywhere
                                });

                        //play the song
                        MediaPlayer.Play(currSong);
                    }
                    else
                    {
                        //play the default song
                        MediaPlayer.Play(defaultSong);
                    }
                }
                else
                {
                    //play the default song
                    MediaPlayer.Play(defaultSong);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// the main update which plays the songs from the addMusic popup
        /// </summary>
        public void Update()
        {
            if (add.ShouldPlay)
            {
                //this is in "try" since it is expreimental and unstable
                try
                {
                    //creates a new constructor of the song class
                    var ctor = typeof(Song).GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Instance, null, // set the neccessary definitions of the class as the ones specific for the song class
                        new[] { typeof(string), typeof(string), typeof(int) }, null); // allows to give the sone name, the path to it, and the duration of the song

                    //set the current song
                    Song currSong =
                        (Song)
                            ctor.Invoke(new object[] // call the constructor made earlier
                                {
                                    add.CurrentSong, //sets the name according to the name of the song in the popup
                                    @"c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" + // sets the path to the song
                                    add.CurrentSong,
                                    0 // sets the song duration as 0 since its not used anywhere
                                });

                    //play the song
                    MediaPlayer.Play(currSong);
                }
                catch (Exception)
                { }

                //disable the flag that marks the start of playing
                add.ShouldPlay = false;
            }
            //set the oldmousestate
            oldState = MediaPlayer.State;
        }

        /// <summary>
        /// allows SwitchGameState to call the addMusic popup
        /// </summary>
        public void AddMusic()
        {
            //create a new add
            add.Dispose();
            add = new AddMusic();

            //if its not shown than show it
            if (!add.Visible)
                add.Show();
        }
    }
}
