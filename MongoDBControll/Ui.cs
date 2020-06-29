using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace MongoDBControll.lib
{
    public partial class Ui : Form
    {
        private EmguCv objemgucv;
        private Image<Bgr, byte> iplImage;
        public Ui()
        {
            InitializeComponent();
            
        }
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            GC.Collect();
        }
        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "jpg files (*.fits)|*.fits";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.objemgucv = new EmguCv(dlg.FileName);
                    this.iplImage = CvInvoke.Imread(dlg.FileName).ToImage<Bgr, Byte>();
                    imageBox1.Image = this.iplImage;
                    // Create a new Bitmap object from the picture file on disk,
                    // and assign that to the PictureBox.Image property

                }
            }
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void imageBox2_Click(object sender, EventArgs e)
        {

        }
      

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            Tuple<Image<Bgr, byte>, int> result = this.objemgucv.SegmentionWatershed(7,true,"RAW");
            imageBox2.Image = result.Item1;
          
  label1.Text = Convert.ToString(result.Item2);
            GC.Collect();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}