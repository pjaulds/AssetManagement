using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Utilities
{
    public partial class ImageBrowserForm : Form
    {
        public ImageBrowserForm()
        {
            InitializeComponent();
        }

        private Image actualImage;
        public byte[] mPhoto
        {
            get
            {
                return FileUtil.ImageToByte(pictureBox1.Image);
            }
            set
            {
                pictureBox1.Image = null;
                if (value != null)
                {
                    if (value.Length != 0)
                    {
                        pictureBox1.Image = Image.FromStream(new System.IO.MemoryStream(value));
                        actualImage = pictureBox1.Image;
                    }
                }
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            FileUtil.BrowseImage(pictureBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private double ZOOMFACTOR = 1.25;	// = 25% smaller or larger
        private int MINMAX = 5;

        private void ZoomIn()
        {
            if ((pictureBox1.Width < (MINMAX * panel5.Width)) &&
                (pictureBox1.Height < (MINMAX * panel5.Height)))
            {
                pictureBox1.Width = Convert.ToInt32(pictureBox1.Width * ZOOMFACTOR);
                pictureBox1.Height = Convert.ToInt32(pictureBox1.Height * ZOOMFACTOR);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void ZoomOut()
        {
            if ((pictureBox1.Width > (panel5.Width / MINMAX)) &&
                (pictureBox1.Height > (panel5.Height / MINMAX)))
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Width = Convert.ToInt32(pictureBox1.Width / ZOOMFACTOR);
                pictureBox1.Height = Convert.ToInt32(pictureBox1.Height / ZOOMFACTOR);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (actualImage != null) pictureBox1.Image = actualImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = FileUtil.RotateImage(pictureBox1.Image, 90, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            Close();
        }
    }
}
