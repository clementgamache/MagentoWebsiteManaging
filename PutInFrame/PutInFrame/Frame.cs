using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace PutInFrame
{
    class Frame
    {
        public System.Drawing.Bitmap image;
        public string path;

            

        public Frame(string file, double widthInch, double heightInch, double mouldingInch)
        {

            image = new System.Drawing.Bitmap(file);
            path = file;
            crop(widthInch, heightInch);
            putInFrame(widthInch, heightInch, mouldingInch);
          
                string name = System.IO.Path.GetFileName(path);
                string outputFileName = System.IO.Path.GetDirectoryName(path) + @"\framed\" +name;
                //File.Create(outputFileName).Dispose();
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        image.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                

        }

        public void setHeight(int height)
        {
            double factor = (double)height / (double)image.Height;
            Bitmap resized = new Bitmap(image, new System.Drawing.Size((int)((double)image.Width * factor), (int)((double)image.Height * factor)));
            image.Dispose();
            image = resized;
        }

        public void save()
        {
        }

        private void crop(double widthInch, double heightInch)
        {
            //crop
            double wantedRatioBS = Math.Max(widthInch, heightInch) / Math.Min(widthInch, heightInch);
            double realRatioHW = (double)(image.Height) / (double)(image.Width);
            double realRatioBS = realRatioHW >= 1 ? realRatioHW : (1.0 / realRatioHW);
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle();

            if (((wantedRatioBS < realRatioBS) && (realRatioHW >= 1.0)) || //Biggest is too big and biggest is height
                ((wantedRatioBS > realRatioBS) && (realRatioHW <= 1.0)))  //Smallest is too big and smallest is height
            {
                cropArea.Width = image.Width;
                cropArea.Height = (int)((double)image.Width / (realRatioHW <= 1.0 ? wantedRatioBS : (1.0 / wantedRatioBS)));
                cropArea.X = 0;
                cropArea.Y = (image.Height - cropArea.Height) / 2;
            }
            else if (((wantedRatioBS < realRatioBS) && (realRatioHW < 1.0)) ||
                    ((wantedRatioBS > realRatioBS) && (realRatioHW > 1.0)))
            {
                cropArea.Height = image.Height;
                cropArea.Width = (int)((double)image.Height / (realRatioHW > 1.0 ? wantedRatioBS : (1.0 / wantedRatioBS)));
                cropArea.Y = 0;
                cropArea.X = (image.Width - cropArea.Width) / 2;
            }
            else
            {
                cropArea.Location = new System.Drawing.Point(0, 0);
                cropArea.Size = image.Size;
            }
            System.Drawing.Bitmap im = image.Clone(cropArea, image.PixelFormat);
            image.Dispose();
            image = im;
        }

        private void putInFrame(double widthInch, double heightInch, double mouldingInch)
        {
            double d = (double)mouldingInch * Math.Max((double)image.Width, (double)image.Height) / Math.Max(widthInch, heightInch);
            d = Math.Ceiling(d);
            System.Drawing.Bitmap frame = new System.Drawing.Bitmap(image.Width + (int)(2 * d), image.Height + (int)(2 * d));
            using (Graphics g = Graphics.FromImage(frame))
            {
                g.FillRectangle(Brushes.Black, 0, 0, frame.Width, frame.Height);
                g.DrawImage(image, new Rectangle((int)d, (int)d, image.Width, image.Height));
            }
            image = frame;
        }

        public bool isHorizontal()
        {
            return image.Width > image.Height;
        }

    }
}
