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
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace RubikCube
{
    class Music
    {
        private readonly Song defaultSong;
        public bool IsMuted = false;
        public int MusicIndexClassic = 0;
        public int MusicIndexRock = 0;
        public string WhichGenre;
        private AddMusic add;
        private Song currSong;
        private MediaState oldState;

        public Music(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, ContentManager content)
        {
            WhichGenre = "default";
            add = new AddMusic();
            defaultSong = content.Load<Song>("music/bg-music");
            if (Directory.Exists("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs"))
            {
                string[] files = Directory.GetFiles("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs",
                    "*", SearchOption.TopDirectoryOnly);
                int fCount = files.Length;
                if (fCount > 0)
                {
                    string name = files[0].Substring(36 + Environment.UserName.Length,
                        files[0].Length - (36 + Environment.UserName.Length));
                    var ctor = typeof(Song).GetConstructor(
                             BindingFlags.NonPublic | BindingFlags.Instance, null,
                             new[] { typeof(string), typeof(string), typeof(int) }, null);
                    Song currSong1 =
                        (Song)
                            ctor.Invoke(new object[]
                                {
                                    add.currentSong,
                                    @"c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" +
                                    name,
                                    0
                                });
                    MediaPlayer.Play(currSong1);
                }
                else
                {
                    currSong = defaultSong;
                }
            }
            else
            {
                currSong = defaultSong;
            }
            //MediaPlayer.Play(currSong);
        }

        public void Update(MouseState mouseState, string whichGenre, bool justSwitched)
        {
            ChangeSongs(whichGenre, justSwitched);
            if (add.ShouldPlay)
            {
                try
                {
                    var ctor = typeof(Song).GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Instance, null,
                        new[] { typeof(string), typeof(string), typeof(int) }, null);
                    Song currSong =
                        (Song)
                            ctor.Invoke(new object[]
                                {
                                    add.currentSong,
                                    @"c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" +
                                    add.currentSong,
                                    0
                                });
                    Debug.WriteLineIf(MediaPlayer.State == MediaState.Playing, "here 1");
                    MediaPlayer.Play(currSong);
                    Debug.WriteLineIf(MediaPlayer.State == MediaState.Playing, "here 2");

                }
                catch (Exception)
                { }
                add.ShouldPlay = false;
            }
            oldState = MediaPlayer.State;
        }

        public void AddMusic()
        {
            if (!add.Visible)
                add.Show();
        }

        private void ChangeSongs(string whichGenre, bool justSwitched)
        {
            if (MediaPlayer.State != MediaState.Playing && MediaPlayer.State != MediaState.Paused)
            {
                if (whichGenre == "default")
                {
                    MediaPlayer.Play(defaultSong);
                }
            }
            if (MediaPlayer.State == MediaState.Stopped && oldState == MediaState.Playing)
            {

            }
        }
    }
}
