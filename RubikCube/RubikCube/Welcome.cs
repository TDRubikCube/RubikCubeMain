using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RubikCube
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
