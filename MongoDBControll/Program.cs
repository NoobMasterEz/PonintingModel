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
using SRSLib;

namespace MongoDBControll.lib
{
    class Program
    {
        ////private const string Namefile = @"C:\\Users\\specter\\Desktop\\Mongo\\MongoDBControll\\lib\\image\2020_04_03T12.51.52.125Z_B.jpg";
        ////private const string Namefilfits = @"C:\\Users\\specter\\Desktop\\Mongo\\MongoDBControll\\lib\\image\2020_07_10T09.15.45.839Z_Blue.wcs.fits";

        //public static string Namefile1 => Namefile;
        //public static string Fits => Namefilfits;

        [STAThread]
        static void Main(string[] args)
        {

            ////plant wap Ex
            //ImageLib.ImageType imageType = new ImageLib.ImageType();
            //ImageLib.OpenAnyImageType(Fits, ref imageType); //file fit path

            //MatchLib.PlateListType centerRa2000GuessRads = new MatchLib.PlateListType()
            //{
            //    Px = imageType.N1,
            //    Py = imageType.N2,
            //    XSize = (double)imageType.N1 * 1 / 206264.806,
            //    YSize = (double)imageType.N2 * 1 / 206264.806,
            //    HaveStartingCoords = true
            //};

            //MatchLib.ExtractStars(ref imageType, ref centerRa2000GuessRads);
            //for(int i =0; i<=  centerRa2000GuessRads.NumPlate; i++)
            //{
            //    Console.WriteLine("[{0},{1}],", centerRa2000GuessRads.Plate[i].Xcen, centerRa2000GuessRads.Plate[i].Ycen);
            //}

            /*
             
            //Data Base 
            Mongolib mongoLib = new Mongolib("mongodb://127.0.0.1:27017","GaiaData");
            mongoLib.NAMECOLLECTION = "GDR2Mag11";
            //double[,] polygon = new double[,] { { -1.97, 1.77 }, { -1.9928, 1.7193 }, { -1.9375, 1.8303 }, { -1.97, 1.77 } };
            IFindFluent<GaiaInfo11, GaiaInfo11> result = (IFindFluent<GaiaInfo11, GaiaInfo11>)mongoLib.GeocenterSpherestring(0, 0, 0.01);
            
   

            //Image processing 
            EmguCv test=new EmguCv(Fits);
            test.SegmentionWatershed(7, false,TypeImage.JPG);
            test.SegmentionWatershedRAW(7, false, TypeImage.RAW);
            double[] resultCal;
            
            foreach (var item in result.ToList())
            {
                resultCal=MethodStaticFomula.StandardCoordi(Convert.ToDouble(item.RA.ToString()), Convert.ToDouble(item.Dec.ToString()), 4.2620940334, 0.5893976884);
                
                //Astro.RaDec(Convert.ToDouble(item.RA.ToString()), Convert.ToDouble(item.Dec.ToString()),2020);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[INFO](Query From Database) RA={0}", Convert.ToDouble(item.RA.ToString()));
                Console.WriteLine("[INFO](Query From Database) DEC={0}", Convert.ToDouble(item.Dec.ToString()));
                Console.ResetColor();
            
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[INFO](Calculat)={0},{1}",resultCal[0], resultCal[1]);
                Console.ResetColor();
            }
            */
            /*
            #if DEBUG
                        Console.WriteLine("Press enter to close...");
                        Console.ReadLine();
            #endif
            */
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui());


        }
    }
}
