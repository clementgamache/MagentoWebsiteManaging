using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuralPreview
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //mergeImages(@"C:\Users\User\Desktop\pic.png", @"C:\Users\User\Desktop\nursery-room-4-cropped-watermarked.png", @"C:\Users\User\Desktop\allo.png");
        }

        private void mergeImages(string backName, string frontName, string outName)
        {
            Bitmap back = new Bitmap(backName); 
            Bitmap front = new Bitmap(frontName);

            //Resize the front image so that its height fits with the image floor
            Rectangle alphaRectangle = getAlphaRectangle(front);
            double heightRatio = alphaRectangle.Height / (double)back.Height;
            double backRatioHW = ((double)back.Height) / ((double)back.Width);
            double newHeight = alphaRectangle.Height;
            double newWidth = newHeight / backRatioHW;
            back = new Bitmap(back, new Size((int)newWidth, (int)newHeight));

            //crop the back image so that it fits with the new back width
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle();
            cropArea.Size = new Size(back.Width, front.Height);
            cropArea.Location = new Point((front.Width - back.Width) / 2, 0);
            Bitmap f = new Bitmap(cropArea.Width, cropArea.Height);

            using (Graphics g = Graphics.FromImage(f))
            {
                g.DrawImage(front, new Rectangle(0, 0, f.Width, f.Height),
                                 cropArea,
                                 GraphicsUnit.Pixel);
            }

            var target = new Bitmap(f.Width, f.Height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(target);
            graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

            graphics.DrawImage(back, 0, alphaRectangle.Y);
            graphics.DrawImage(f, 0, 0);

            target.Save(outName, ImageFormat.Png);
            target.Dispose();
            front.Dispose();
            back.Dispose();
            f.Dispose();
        }

        Rectangle getAlphaRectangle(Bitmap image)
        {
            int minX, minY, maxX, maxY;
            minX = minY = 999999;
            maxX = maxY = -1;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    //MessageBox.Show(pixel.A.ToString());
                    if (pixel.A < 230)
                    {

                        minX = Math.Min(minX, i);
                        maxX = Math.Max(maxX, i);
                        minY = Math.Min(minY, j);
                        maxY = Math.Max(maxY, j);
                    }
                }
            }
            Rectangle ret = new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
            return ret;
        }

        private void run()
        {
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            string backPath = label1.Text;
            string frontPath = label2.Text;
            string destPath = label3.Text;
            DirectoryInfo dBack = new DirectoryInfo(backPath);
            DirectoryInfo dFront = new DirectoryInfo(frontPath);
            foreach (FileInfo fBack in dBack.GetFiles())
            {
                if (ImageExtensions.Contains(Path.GetExtension(fBack.FullName).ToUpperInvariant()))
                {
                    foreach (FileInfo fFront in dFront.GetFiles())
                    {
                        if (ImageExtensions.Contains(Path.GetExtension(fFront.FullName).ToUpperInvariant()))
                        {
                            string outPutFile = destPath + "\\" + 
                                Path.GetFileNameWithoutExtension(fFront.FullName) + "-" + 
                                Path.GetFileNameWithoutExtension(fBack.FullName) + ".png";
                            mergeImages(fBack.FullName, fFront.FullName, outPutFile);
                        }
                    }
                }
            }
        }

        private void updateLabels()
        {
            label1.Text = folderBrowserDialog1.SelectedPath;
            label2.Text = folderBrowserDialog2.SelectedPath;
            label3.Text = folderBrowserDialog3.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            updateLabels();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog2.ShowDialog();
            updateLabels();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog3.ShowDialog();
            updateLabels();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            run();
        }
    }
}
