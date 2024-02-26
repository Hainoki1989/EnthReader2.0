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
using EnthParser;



namespace EnthReader2._0
{
    public partial class Form1 : Form
    {
        string selectedFileName;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {





            EnthParser.EnthParser FileParser = new EnthParser.EnthParser();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CAR Files (*.car)|*.car|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file name
                selectedFileName = openFileDialog.FileName;
                FileParser.LoadModelFile(selectedFileName);
            }

            /*OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadImageFromFile(openFileDialog.FileName);
            }*/
        }


        public void LoadImageFromFile(string fileName) 
        {
            int width = 64; Height = 32;

            byte[] picture = File.ReadAllBytes(fileName);
            int byteCount = 0;

            Bitmap bitmap = new Bitmap(width, Height);

            for(int y=0; y<Height; y++) 
            {
                for(int x=0; x<Width; x++) 
                {
                    try
                    {
                        Color pixelColor = Color.FromArgb((int)picture[byteCount], (int)picture[byteCount + 1], (int)picture[byteCount + 2], (int)picture[byteCount + 3]);

                        bitmap.SetPixel(x, y, pixelColor);
                    }
                    catch
                    {
                        break;
                    }
                    byteCount += 4;
                }
            }

            bitmap.Save("output.bmp");
        }
    }
}
