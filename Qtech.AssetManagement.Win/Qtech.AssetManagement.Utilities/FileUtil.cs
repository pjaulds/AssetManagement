using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
//using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Utilities
{
    public static class FileUtil
    {
        //Open file in to a filestream and read data in a byte array.
        public static byte[] ReadFile(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes to read from file.
            //In this case we want to read entire file. So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public static byte[] BrowseImage()
        {
            //Ask user to select file.
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Jpeg Files (*.jpg)|*.jpg|PNG Files(*.png)|*.png|Bitmap Files(*.bmp)|*.bmp|GIF Files(*.gif)|*.gif|All Files(*)|*";
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.Cancel)
            {
                if (FileUtil.ReadFile(dlg.FileName).Length > 600000)//500kb
                {
                    MessageBox.Show("File should be less than or equal to 500KB in file size.", "Image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                else
                    return FileUtil.ReadFile(dlg.FileName);
            }
            else return null;
        }

        public static void BrowseImage(PictureBox pic)
        {
            //Ask user to select file.
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Jpeg Files (*.jpg)|*.jpg|PNG Files(*.png)|*.png|Bitmap Files(*.bmp)|*.bmp|GIF Files(*.gif)|*.gif|All Files(*)|*";
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.Cancel)
            {
                if (FileUtil.ReadFile(dlg.FileName).Length > 600000)
                    MessageBox.Show("File should be less than or equal to 500KB in file size.", "Image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    pic.Image = Image.FromStream(new System.IO.MemoryStream(FileUtil.ReadFile(dlg.FileName)));
            }
        }

        public static void BrowseImageNoLimit(PictureBox pic)
        {
            //Ask user to select file.
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Jpeg Files (*.jpg)|*.jpg|PNG Files(*.png)|*.png|Bitmap Files(*.bmp)|*.bmp|GIF Files(*.gif)|*.gif|All Files(*)|*";
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.Cancel)              
                    pic.Image = Image.FromStream(new System.IO.MemoryStream(FileUtil.ReadFile(dlg.FileName)));
        }

        public static Bitmap RotateImage(Image image, float angle, bool bNoClip)
        {
            // center of the image
            float rotateAtX = image.Width / 2;
            float rotateAtY = image.Height / 2;
            return RotateImage(image, rotateAtX, rotateAtY, angle, bNoClip);
        }

        public static Bitmap RotateImage(Image image, float rotateAtX, float rotateAtY, float angle, bool bNoClip)
        {
            int W, H, X, Y;
            if (bNoClip)
            {
                double dW = (double)image.Width;
                double dH = (double)image.Height;

                double degrees = Math.Abs(angle);
                if (degrees <= 90)
                {
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    W = (int)(dH * dSin + dW * dCos);
                    H = (int)(dW * dSin + dH * dCos);
                    X = (W - image.Width) / 2;
                    Y = (H - image.Height) / 2;
                }
                else
                {
                    degrees -= 90;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    W = (int)(dW * dSin + dH * dCos);
                    H = (int)(dH * dSin + dW * dCos);
                    X = (W - image.Width) / 2;
                    Y = (H - image.Height) / 2;
                }
            }
            else
            {
                W = image.Width;
                H = image.Height;
                X = 0;
                Y = 0;
            }

            //create a new empty bitmap to hold rotated image
            Bitmap bmpRet = new Bitmap(W, H);
            bmpRet.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(bmpRet);

            //Put the rotation point in the "center" of the image
            g.TranslateTransform(rotateAtX + X, rotateAtY + Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-rotateAtX - X, -rotateAtY - Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0 + X, 0 + Y));

            return bmpRet;
        }

        //public static void ReportLogo(ReportDocument rd)
        //{
        //    CompanyProfile header = CompanyProfileManager.GetList().First();
        //    rd.Database.Tables["ReportLogo"].SetDataSource(CompanyProfileManager.GetList());
        //    rd.ReportDefinition.Sections["HeaderSection"].ReportObjects["mReportLogo1"].Height = header.mHeight;
        //    rd.ReportDefinition.Sections["HeaderSection"].ReportObjects["mReportLogo1"].Width = header.mWidth;
        //}
    }
}
