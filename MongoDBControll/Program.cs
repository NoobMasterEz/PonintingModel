using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MongoDB.Driver;
using Emgu.CV;
using Emgu.CV.CvEnum;
namespace MongoDBControll.lib
{
    class Program
    {
        private const string Namefile = @"C:\\Users\\specter\\Desktop\\Mongo\\MongoDBControll\\lib\\image\2020_06_03T17.55.34.602Z_V.jpg";
        private const string Namefilfits = @"C:\\Users\\specter\\Desktop\\Mongo\\MongoDBControll\\lib\\image\2020_06_03T17.55.34.602Z_V.fits";

        public static string Namefile1 => Namefile;
        public static string Fits => Namefilfits;
        static void Main(string[] args)
        {
            //Mongolib mongoLib = new Mongolib("mongodb://192.168.200.126:27017","GaiaData");
            //double[,] polygon = new double[,] { { -1.97, 1.77 }, { -1.9928, 1.7193 }, { -1.9375, 1.8303 }, { -1.97, 1.77 } };
            //IFindFluent<GaiaInfo, GaiaInfo> result = mongoLib.GeocenterSpherestring(1, 1, 0.01);
            //Console.WriteLine(result.ToList().Count());

            
            EmguCv test1 = new EmguCv();
            test1.CreateImag(Fits);

            //Image<Emgu.CV.Structure.Gray, byte> step1 = test1.Thresholding(190, 255, Emgu.CV.CvEnum.ThresholdType.Binary | Emgu.CV.CvEnum.ThresholdType.Otsu);
            //test1.HouCircles(Emgu.CV.CvEnum.HoughType.Gradient,10,1,65,65,1,30);
            //test1.SegmentionWatershed();
            

#if DEBUG
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
#endif


        }
    }
}
