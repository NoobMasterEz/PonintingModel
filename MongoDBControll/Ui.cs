//------UI-------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
//----------------

//------DB-------
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
//---------------

//------Astro---
using AstroNETLib;
using SRSLib;
using Astro;
//-------------
namespace MongoDBControll.lib
{
    public partial class Ui : Form
    {
        
        private EmguCv objemgucv;
        private Image<Bgr, byte> iplImage;
        private SRSLib.ImageLib.ImageType imageType;
        private MatchLib.PlateListType centerRa2000GuessRads;
        private Mongolib mongoLib;
        private IFindFluent<GaiaInfo11, GaiaInfo11> t;
        public Ui()
        {
            InitializeComponent();
            mongoLib = new Mongolib("mongodb://127.0.0.1:27017", "GaiaData");
            mongoLib.NAMECOLLECTION = "GDR2Mag11";


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
                    this.iplImage = this.objemgucv.ImageJPG;
                    imageBox1.Image = this.iplImage;
                    imageType = new SRSLib.ImageLib.ImageType();
                    SRSLib.ImageLib.OpenAnyImageType(dlg.FileName, ref imageType); //file fit path
                    MatchLib.SetCatalogLocation(@"G:\UCAC4\Kepler\");
                    centerRa2000GuessRads = new MatchLib.PlateListType()
                    {
                        Px = imageType.N1,
                        Py = imageType.N2,
                        XSize = (double)imageType.N1 * 1 / 206264.806,
                        YSize = (double)imageType.N2 * 1 / 206264.806,
                        HaveStartingCoords = false
                    };
                    
                    MatchLib.ExtractStars(ref imageType, ref centerRa2000GuessRads);
                    MatchLib.PlateMatch(ref centerRa2000GuessRads);

                    //MatchLib.PlateMatch(ref centerRa2000GuessRads);
                    //MatchLib.PlateMatchImage(ref imageType, ref centerRa2000GuessRads);

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
        private double[] Convert2180(double a,double b)
        {

            Angle Ra = Angle.FromDegs(a);
            Ra=Ra.RerangeDegs(-180, 180);
            Angle Dec = Angle.FromDegs(b);
            Dec=Dec.RerangeDegs(-90, 90);


          
            return new double[] { Ra.Degs, Dec.Degs };

        }
        private void button1_Click(object sender, EventArgs e)
        {
           
            Tuple<Image<Bgr, byte>, Image<Gray, byte>,Image<Gray, byte>,Image<Gray, byte>, VectorOfVectorOfPoint,Tuple<double,double>> result = this.objemgucv.SegmentionWatershed(7,false,TypeImage.JPG, centerRa2000GuessRads);
            double[] resultcvt= Convert2180(result.Item6.Item1,result.Item6.Item2);
            t = (IFindFluent<GaiaInfo11, GaiaInfo11>)mongoLib.GeocenterSpherestring(resultcvt[0], resultcvt[1], 0.00396);
            //t = (IFindFluent<GaiaInfo11, GaiaInfo11>)mongoLib.GeocenterSpherestring(0, 0, 0.01);

            imageBox2.Image = result.Item2;
            imageBox3.Image = result.Item3;
            imageBox4.Image = result.Item4;
            imageBox5.Image = result.Item1;
            label1.Text = Convert.ToString(result.Item5.Size);
            label2.Text = Convert.ToString(centerRa2000GuessRads.NumPlate);
            
            mongoLib.ComparData(t, centerRa2000GuessRads);
            GC.Collect();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void imageBox5_Click(object sender, EventArgs e)
        {

        }

        private void Ui_Load(object sender, EventArgs e)
        {

        }

      
    }
}