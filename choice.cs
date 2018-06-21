using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mosaic
{
    public partial class choice : Form
    {
        public choice()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormMain.ch = 1;
            FormMain.c.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormMain.ch = 2;
            FormMain.c.Close();
        }
    }
}
