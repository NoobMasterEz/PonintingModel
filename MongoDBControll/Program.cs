using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.IO;
using MongoDB.Driver;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Windows.Forms;

namespace MongoDBControll.lib
{
    class Program
    {
        private const string Namefile = @"C:\\Users\\specter\\Desktop\\Mongo\\MongoDBControll\\lib\\image\2020_04_03T12.51.52.125Z_B.jpg";
        private const string Namefilfits = @"C:\\Users\\specter\\Desktop\\Mongo\\MongoDBControll\\lib\\image\2020_06_15T07.35.12.937Z_B.fits";

        public static string Namefile1 => Namefile;
        public static string Fits => Namefilfits;

        [STAThread]
        static void Main(string[] args)
        {
            //Data Base 
            //Mongolib mongoLib = new Mongolib("mongodb://192.168.200.126:27017","GaiaData");
            //double[,] polygon = new double[,] { { -1.97, 1.77 }, { -1.9928, 1.7193 }, { -1.9375, 1.8303 }, { -1.97, 1.77 } };
            //IFindFluent<GaiaInfo, GaiaInfo> result = mongoLib.GeocenterSpherestring(1, 1, 0.01);
            //Console.WriteLine(result.ToList().Count());

            //Image processing 
            
            EmguCv test1 = new EmguCv(Fits);
            test1.SegmentionWatershed(7, true);
            /*
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            test1.CreateImag(Fits);

            ///Image<Emgu.CV.Structure.Gray, byte> step1 = test1.Thresholding(9 , 255, Emgu.CV.CvEnum.ThresholdType.Binary );
            //test1.HouCircles(Emgu.CV.CvEnum.HoughType.Gradient,10,1,65,65,1,30);
            test1.SegmentionWatershed(7);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine("[INFO]->RunTime " + elapsedTime);
            */
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Ui());
#if DEBUG
            //Console.WriteLine("Press enter to close...");
            //Console.ReadLine();
#endif


        }
    }
}
