using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RubikCube
{
    public partial class AddMusic : Form
    {
        public AddMusic()
        {
            InitializeComponent();
        }

        public string currentSong { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                currentSong = openFileDialog1.SafeFileName;
                if (!CheckIfFileExists(currentSong))
                {
                    FileStream fileStream =
                        File.Create("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" + currentSong);
                    Stream stream = openFileDialog1.OpenFile();
                    stream.CopyTo(fileStream);
                    stream.Close();
                    fileStream.Close();
                    comboBox1.Items.Add(currentSong);
                }
            }
        }

        private bool CheckIfFileExists(string song)
        {
            return File.Exists("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs/" + currentSong);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentSong = Convert.ToString(comboBox1.SelectedItem);
            ShouldPlay = true;
        }

        public bool ShouldPlay { get; set; }

        private void AddMusic_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            if (!Directory.Exists("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs"))
                Directory.CreateDirectory("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs");
            string[] files = Directory.GetFiles("c:/users/" + Environment.UserName + "/Documents/RubikCube/Songs", "*", SearchOption.TopDirectoryOnly);
            int fCount = files.Length;
            for (int i = 0; i <fCount ; i++)
            {
                string name = files[i].Substring(36 + Environment.UserName.Length,
                        files[i].Length - (36 + Environment.UserName.Length));
                comboBox1.Items.Add(name);
            }

        }
    }
}
