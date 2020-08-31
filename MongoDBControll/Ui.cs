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
//------math-----


//------DB-------
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
//---------------

//------Astro---
using AstroNETLib;
using SRSLib;
using Astro;
using Newtonsoft.Json;
using Accord.Collections;
using Accord.Math;
using System.Runtime.InteropServices.WindowsRuntime;
using ZedGraph;
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
        private IFindFluent<GaiaInfo11, GaiaInfo11> database;

        private JsonAstro jsonPlan;//json
        private JsonAstro jsondb;//json

        private List<TypeRaDec> data { get; set; }
        private const string catalogpath = @"G:\UCAC4\Kepler\";
        private GraphPane graph;
        private PointPairList spl1;
        private PointPairList spl2;



        public Ui()
        {
            InitializeComponent();
            mongoLib = new Mongolib("mongodb://127.0.0.1:27017", "GaiaData");
            mongoLib.NAMECOLLECTION = "GDR2Mag15";
            this.jsonPlan = new JsonAstro();
            this.jsondb = new JsonAstro();
            button1.Enabled = false;
            this.graph= zedGraphControl1.GraphPane;
            this.spl1 = new PointPairList();
            this.spl2 = new PointPairList();
            
        }

      
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            GC.Collect();

        }
        private List<TypeRaDec> CreateData2GridView(double[][] centerradac,double[][] RaDec, List<TypeKDTree> prediction, IDictionary<string, string> radec)
        {
            double ra = Angle.FromHms(radec["OBJCTRA"]).Degs;
            double dec = Angle.FromDms(radec["OBJCTDEC"]).Degs;
            var list = new List<TypeRaDec>();
            for (int i = 0; i < centerradac.Length; i++)
            {
                //Console.WriteLine("[{0},{1}],", Math.Abs( centerradac[i][0] - 4096),Math.Abs( centerradac[i][1]- 4096));
                double[] xy = MethodStaticFomula.StandardCoordi(centerradac[i][0], centerradac[i][1],ra,dec);
                
                list.Add(
                    new TypeRaDec()
                    {
                        X = prediction[i].Nearest[0],
                        Y = prediction[i].Nearest[1],
                        Xperdict = centerradac[i][0],
                        Yperdict = centerradac[i][1],
                        Ra= RaDec[i][0],
                        Dec = RaDec[i][1]
                    });
            }
            
            return list;
        }

        private void ZedGrahp(List<TypeKDTree> prediction, double[][] database)
        {
            this.graph.Title.Text = "Prediction";
            //PointPairList Oz2time = new PointPairList();
            foreach(var j in prediction)
            {
                spl1.Add(j.Nearest[0], j.Nearest[1]);
            }
            for (int i = 0; i < database.Length; i++)
            {
                spl2.Add(database[i][0], database[i][1]);
            }
            LineItem myCurve1 = this.graph.AddCurve("predict", spl1, Color.Blue, SymbolType.Plus);
            this.graph.AddCurve("DB", spl2, Color.Red, SymbolType.Star).Line.IsVisible=false;
            myCurve1.Line.IsVisible = false;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl1.Refresh();

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
                    MatchLib.SetCatalogLocation(catalogpath);
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
                    button1.Enabled = true;
                    //MatchLib.PlateMatch(ref centerRa2000GuessRads);
                    //MatchLib.PlateMatchImage(ref imageType, ref centerRa2000GuessRads);

                    // Create a new Bitmap object from the picture file on disk,
                    // and assign that to the PictureBox.Image property

                }
            }
        }

        private void raDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graph test = new Graph();
            test.ShowGraph(data);
            test.Show();
        }

        private double[] Convert2180(double a,double b)
        {

            Angle Ra = Angle.FromRads(a);
            Ra=Ra.RerangeDegs(-180, 180);
            Angle Dec = Angle.FromRads(b);
            Dec=Dec.RerangeDegs(-90, 90);

            return new double[] { Ra.Degs, Dec.Degs };

        }
     
        private void button1_Click(object sender, EventArgs e)
        {

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            
           
            Tuple<Image<Bgr, byte>, Image<Gray, byte>, IDictionary<string, string>, Image<Gray, byte>, VectorOfVectorOfPoint, List<int[]>, double[][]> result = this.objemgucv.SegmentionWatershed(10, false, TypeImage.JPG, centerRa2000GuessRads);
            double[] resultcvt= Convert2180(centerRa2000GuessRads.RARadCen,centerRa2000GuessRads.DecRadCen);
            
            database = (IFindFluent<GaiaInfo11, GaiaInfo11>)mongoLib.GeocenterSpherestring(resultcvt[0], resultcvt[1], 0.00396);
            //t = (IFindFluent<GaiaInfo11, GaiaInfo11>)mongoLib.GeocenterSpherestring(0, 0, 0.01);
            double[][] predictresultxy = mongoLib.XY2RaDec(data: result.Item6, radec: result.Item3);
            double[][] dbresultxy = mongoLib.RaDec2XY(database, result.Item3);
            double[][] RaDec = mongoLib.GetRaDec(data: database);
            imageBox5.Image = result.Item4;
            
            //label2.Text = Convert.ToString(centerRa2000GuessRads.NumPlate);
             

            //Setup kdt
            KDTreeCluster kdt = new KDTreeCluster(predictresultxy);
            kdt.SetupNode(kdt.tree.Root.Left.Right);
            int numberbar = 0 ;



            for (int i = 0; i < dbresultxy.Length ;i++ )
            {
                KDTreeNodeCollection<KDTreeNode<int>> kdtfilter = kdt.FindWithNeighbors(dbresultxy[i], 1);
                numberbar= ((i * 100) / dbresultxy.Length) + 2;
                progressBar1.Value = numberbar;
                label2.Text = String.Format("{0}%",numberbar.ToString());
                if (kdtfilter.Minimum != 0)
                    kdt.Add(
                        new TypeKDTree()
                        {
                            Distand = kdtfilter.Minimum,
                            Nearest = kdtfilter.Nearest.Position,
                            Father= dbresultxy[i]

                        }

                        ) ; 
                
            }
           
           
            //MethodStaticFomula.ErrorArcSec(mensq);
            label1.Text = Convert.ToString(kdt.Count);

            
            
            //plot graph
            ZedGrahp(kdt.Query(), dbresultxy);


            

            double[] maimaxmean=kdt.MinMaxMean;
            Console.WriteLine("[INFO][MAX]{0}, [MIN{1}, [MEAN]{2}, [COUNT]{3}]", maimaxmean[0], maimaxmean[1], maimaxmean[2],kdt.Count);
            List<TypeKDTree> sigma1 = MethodStaticFomula.CreateSigma(kdt.Query(), maimaxmean[2]);
            kdt.Show();
            Console.WriteLine("[INFO][X][RMS][{0}]",MethodStaticFomula.RMS(sigma1, 0));
            Console.WriteLine("[INFO][Y][RMS][{0}]", MethodStaticFomula.RMS(sigma1, 1));
            //data 
            data = CreateData2GridView(dbresultxy, RaDec, kdt.Query(), result.Item3);
            dataGridView1.DataSource = data;

            
            
            
            //---debug---
            //
            //string json = JsonConvert.SerializeObject(this.jsonPlan);
            //string databasejson = JsonConvert.SerializeObject(dbresult);
            //Console.WriteLine(databasejson);
            //JsonAstro.Save(@"C:\Users\specter\Desktop\Mongo\MongoDBControll\Json\plant.json", json);
            //JsonAstro.Save(@"C:\Users\specter\Desktop\Mongo\MongoDBControll\Json\db.json", databasejson);

            GC.Collect();
        }

        

        private void groupBox3_Enter(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void groupBox4_Enter(object sender, EventArgs e) { }

        private void imageBox5_Click(object sender, EventArgs e) { }

        private void Ui_Load(object sender, EventArgs e) { }

        private void imageBox1_Click(object sender, EventArgs e) { }

        private void imageBox2_Click(object sender, EventArgs e) { }

        private void groupBox1_Enter(object sender, EventArgs e) { }

        private void groupBox2_Enter(object sender, EventArgs e) { }

        private void groupBox4_Enter_1(object sender, EventArgs e){ }

        private void groupBox2_Enter_1(object sender, EventArgs e){ }

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e){ }

        private void progressBar1_Click(object sender, EventArgs e) { }

        private void chart1_Click(object sender, EventArgs e) { }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
            
        }

        private void garhpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        
    }
}