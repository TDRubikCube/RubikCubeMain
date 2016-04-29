using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace RubikCube
{
    public partial class AddMusic : Form
    {
        public AddMusic()
        {
            InitializeComponent();
        }

        /// <summary>
        /// the current song chosen
        /// </summary>
        public string CurrentSong { get; set; }

        /// <summary>
        /// click on the add song button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //show the dialog that allows to choose a song
            DialogResult result = openFileDialog1.ShowDialog();

            //if the user finished selecting
            if (result == DialogResult.OK)
            {
                //set the song name as the one the user chose
                CurrentSong = openFileDialog1.SafeFileName;

                //checks if the song was already added by the user
                if (!CheckIfFileExists())
                {
                    //creates a new file with the song name in the "songs" dir
                    FileStream fileStream =
                        File.Create("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" + CurrentSong);

                    //open the file the user chose
                    Stream stream = openFileDialog1.OpenFile();

                    //copty the file to the new file created
                    stream.CopyTo(fileStream);

                    //close the streams
                    stream.Close();
                    fileStream.Close();

                    //add to the list the song which the user added
                    comboBox1.Items.Add(CurrentSong);
                }
            }
        }

        /// <summary>
        /// checks if the file already exists
        /// </summary>
        /// <returns></returns>
        private bool CheckIfFileExists()
        {
            return File.Exists("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" + CurrentSong);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        /// <summary>
        /// when clicked the play song button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //set the current song according to the list of songs
            CurrentSong = Convert.ToString(comboBox1.SelectedItem);

            //flag that it should play the song
            ShouldPlay = true;
        }

        /// <summary>
        /// makrs whether it should play the song or not
        /// </summary>
        public bool ShouldPlay { get; set; }

        /// <summary>
        /// when just loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMusic_Load(object sender, EventArgs e)
        {
            //checks if the "songs" dir exists, and if it doesnt, create it
            if (!Directory.Exists("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs"))
                Directory.CreateDirectory("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs");

            //get the files in the songs dir
            string[] files = Directory.GetFiles("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs", "*", SearchOption.TopDirectoryOnly);

            //get the number of files

            int fCount = files.Length;

            //add the songs to the list of songs
            for (int i = 0; i < fCount; i++)
            {
                //get the name of the songs (since it gives the full path, the subsrtring changes it to just the name)
                string name = files[i].Substring(36 + Environment.UserName.Length,
                        files[i].Length - (36 + Environment.UserName.Length));

                //add the song to the list
                comboBox1.Items.Add(name);
            }
        }
    }
}
