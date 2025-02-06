using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Utilities
{
    public partial class Message : Form
    {
        public Message()
        {
            InitializeComponent();
        }

        public String mMessage { get; set; }

        public bool mInformation
        {
            set
            {
                pictureBox1.Image = ImagesResource.Information;
            }
        }

        public bool mWarning
        {
            set
            {
                pictureBox1.Image = ImagesResource.Warning;
            }
        }

        private void Message_Load(object sender, EventArgs e)
        {
            Messagelabel.Text = mMessage;
            OKbutton.Focus();
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
