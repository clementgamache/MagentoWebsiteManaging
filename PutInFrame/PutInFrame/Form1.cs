using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PutInFrame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string path = folderBrowserDialog1.SelectedPath;
            if (path.Length > 1)
            {
                label1.Text = path;
            }
            else
            {
                label1.Text = "No file chosen...";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pathName = folderBrowserDialog1.SelectedPath;
            string frameWidth = textBox1.Text;
            string frameHeight = textBox2.Text;
            string mouldingWidth = textBox3.Text;
            double width, height, moulding;
            try
            {
                width = getOrThrow(frameWidth);
                height = getOrThrow(frameHeight);
                moulding = getOrThrow(mouldingWidth);

                DirectoryInfo dF = new DirectoryInfo(pathName);
                System.IO.Directory.CreateDirectory(pathName + @"\framed");
                foreach (FileInfo f in dF.GetFiles())
                {
                    string filePath = f.FullName;
                    Frame frame = new Frame(filePath, width, height, moulding);
                }
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private double getOrThrow(string value)
        {
            double val = 0;
            if (!double.TryParse(value, out val))
                throw new Exception("Expected numeric value, but received \"" + value + "\"");
            return val;
        }
    }
}
