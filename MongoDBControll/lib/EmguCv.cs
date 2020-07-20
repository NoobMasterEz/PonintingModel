using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV.CvEnum;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV.UI;
using System.Drawing.Imaging;
using SRSLib;

namespace MongoDBControll.lib
{
    class EmguCv 
    {
        //private Mat imagemat;
        private Matrix<int> raw8bit;
        private MethodStaticFomula fomula;
        private Image<Bgr, byte> jpg { set; get; }
        private Image<Bgr, byte> raw;
        private Image<Emgu.CV.Structure.Gray, byte> grayjpg;
        private Image<Emgu.CV.Structure.Gray, byte> grayraw;
        private Image<Emgu.CV.Structure.Gray, byte> thresholdimage;
        private Image<Gray, byte> gray;
        private Image<Gray, byte> thresh;

        public EmguCv(string file) 
        {
            
            FitsFile test = new FitsFile(file);
            MethodTranfrom  Method= new MethodTranfrom(test.GenerateImage());    
            this.fomula = new MethodStaticFomula();
            this.raw8bit = Method.Convert1628(); // Create Raw
            this.jpg = Genarate2Jpg(Method.GetRaw()); // Create JPGE
            this.raw = this.raw8bit.Mat.ToImage<Bgr, byte>();

        }




        private Image<Bgr, byte> Genarate2Jpg(Matrix<ushort> image)
        {   /*
             * Createed by pee chaimg
             */
            ushort LowerValue, UpperValue;
            double LowerPercen, UpperPercen;

            FitsFile.GetStrecthProfile(out LowerPercen, out UpperPercen);
            FitsFile.GetUpperAndLowerShortBit(image,
                                              out LowerValue,
                                              out UpperValue,
                                              LowerPercen,
                                              UpperPercen);
            Matrix<ushort> imgJPG = FitsFile.StretchImageU16Bit(image, LowerValue, UpperValue);
            //Mat imgJPG = CvInvoke.Imread(namefile);
            
            return imgJPG.Mat.ToImage<Bgr, byte>();

        }

        public Image<Bgr, byte> ImageJPG
        {
            get => this.jpg;
        }
        /// <summary>
        ///  this fuction build 2 value 
        ///  - newimage (bgr)
        ///  - gray 
        /// </summary>
        /// <param name="namefile">location file </param>
        private void CreateImag()
        {

            this.grayjpg = this.jpg.Convert<Gray, Byte>();
            this.grayraw = this.raw.Convert<Gray, Byte>();


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">threshold value</param>
        /// <param name="max">maximum value to use with the THRESH_BINARY and THRESH_BINARY_INV thresholding types.</param>
        /// <param name="nametype">See in auto comple</param>
        /// <returns></returns>
        private Image<Emgu.CV.Structure.Gray, byte> Thresholding(Image<Emgu.CV.Structure.Gray, byte> gray, int min, int max, ThresholdType nametype)
        {

            this.thresholdimage = gray.CopyBlank(); //coppy gray to result thresholding
            ///
            CvInvoke.Threshold(gray, this.thresholdimage, min, max, nametype);
            return this.thresholdimage;
        }


        public Tuple<Image<Bgr, byte>, Image<Gray, byte>, Image<Gray, byte>, Image<Gray, byte>, VectorOfVectorOfPoint> SegmentionWatershed(int threshmin, bool flat, TypeImage check, MatchLib.PlateListType centerRa2000GuessRads)
        {
            CreateImag();
            //Mat3b src = imread("path_to_image");
            double[] min, max;
            Point[] pmin, pmax;


            if (check.Equals(TypeImage.JPG))
            {
                this.gray = this.grayjpg;
                this.thresh = this.grayjpg.CopyBlank();
            }
            else if (check.Equals(TypeImage.RAW))
            {
                this.gray = this.grayraw;
                this.thresh = this.grayraw.CopyBlank();
            }
            else
            {
                Tuple.Create(this.jpg, this.jpg, 0);

            }

            //cvtColor(src, gray, COLOR_BGR2GRAY);


            Image<Gray, float> thresh2 = new Image<Gray, float>(gray.Width, gray.Height);

            //threshold(gray, thresh, 0, 255, THRESH_BINARY | THRESH_OTSU);
            if (flat)
                CvInvoke.Threshold(gray.SmoothGaussian(3), thresh, threshmin, 255, ThresholdType.Binary | ThresholdType.Otsu);
            else
                CvInvoke.Threshold(gray, thresh, threshmin, 255, ThresholdType.Binary | ThresholdType.Otsu);

            // noise removal
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(3, 3), new Point(1, 1));

            //Mat1b opening;
            //morphologyEx(thresh, opening, MORPH_OPEN, kernel, Point(-1, -1), 2);
            Image<Gray, byte> opening = thresh.MorphologyEx(MorphOp.Open, kernel, new Point(1, 1), 1, BorderType.Default, new MCvScalar(255));
            //Image<Gray, byte> background = thresh.MorphologyEx(MorphOp.Dilate, kernel, new Point(-1, -1), 2, BorderType.Default, new MCvScalar(255));
            //background = ~background;
            //ImageViewer.Show(opening);
            Image<Gray, byte> sure_bg = opening.Dilate(3);
            Image<Gray, float> dist_transform = new Image<Gray, float>(opening.Width, opening.Height);
            CvInvoke.DistanceTransform(opening, dist_transform, null, DistType.L2, 3);



            dist_transform.MinMax(out min, out max, out pmin, out pmax);
            //Console.WriteLine(max[0]);
            CvInvoke.Threshold(dist_transform, thresh2, max[0]*0.05, 255, 0);
            Image<Gray, Byte> dist_8u = thresh2.Convert<Gray, Byte>();
            //ImageViewer.Show(dist_8u);
            
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //Mat hierarchy = new Mat() ;

            CvInvoke.FindContours(dist_8u, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            Console.WriteLine("[INFO](JPG)->{0}", contours.Size);

            //Image<Bgr, int> marker = new Image<Bgr, int>(dist_transform.Width, dist_transform.Height);

            //Image<Gray, int> one = new Image<Gray, int>(marker.Cols, marker.Rows, new Gray(255));

            //CvInvoke.BitwiseOr(one, marker, marker, background);
            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle r = CvInvoke.BoundingRectangle(contours[i]);
                double[] result_trafrom =MethodStaticFomula.Trafrom2Polar(r.X, r.Y, 2048, 2048); // tranfrom2 polar
                double[] result = MethodStaticFomula.InvertStandardCoordi(result_trafrom[0],result_trafrom[1], 244.2, 33.77); // invert2position 

                //Console.WriteLine("x={0}, y={1}", p[0], p[1]);
                Console.WriteLine(MatchLib.FindNearestExtracted(r.X, r.Y, centerRa2000GuessRads));
                this.jpg.Draw(r, new Bgr(Color.Red));
                    
                    //CvInvoke.Circle(this.jpg, this.fomula.CenterOfCircle(r), r.Width / 2,new MCvScalar(0,0,255));
                
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine("[INFO](Invert)={0},{1}", result[0], result[1]);
                Console.WriteLine("[{0},{1}],", result[0], result[1]);

                Console.ResetColor();

            }
            for(int k=0;k<= centerRa2000GuessRads.NumPlate;k++)
            {
                
                
                CvInvoke.Circle(this.jpg, new Point(Convert.ToInt32(centerRa2000GuessRads.Plate[k].Xcen), Convert.ToInt32(centerRa2000GuessRads.Plate[k].Ycen)), 3, new Bgr(Color.Green).MCvScalar);

            }
            //Image<Bgr, byte> dbg = new Image<Bgr, byte>(marker.Cols, marker.Rows);
            //CvInvoke.CvtColor(opening, dbg, ColorConversion.Gray2Bgr);
            //CvInvoke.Watershed(dbg, marker);

            return Tuple.Create(this.jpg, opening, sure_bg, dist_8u, contours);
                                                                                
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="method">Currently, the only implemented method is CV_HOUGH_GRADIENT</param>
        /// <param name="dp">Resolution of the accumulator used to detect centers of the circles. For example, if it is 1, the accumulator will have the same resolution as the input image, if it is 2 - accumulator will have twice smaller width and height, etc</param>
        /// <param name="minDist">Minimum distance between centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed</param>
        /// <param name="param1">The first method-specific parameter. In case of CV_HOUGH_GRADIENT it is the higher threshold of the two passed to Canny edge detector (the lower one will be twice smaller).</param>
        /// <param name="param2">Second method-specific parameter. In case of CV_HOUGH_GRADIENT , it is the accumulator threshold for the circle centers at the detection stage. The smaller it is, the more false circles may be detected. Circles, corresponding to the larger accumulator values, will be returned first.</param>
        /// <param name="minRadius">Minimum circle radius.</param>
        /// <param name="maxRadius">Maximum circle radius.</param>
        /// <returns>Image<Emgu.CV.Structure.Bgr, byte></returns>
        public Image<Emgu.CV.Structure.Bgr, byte> HouCircles(HoughType method, double dp, double minDist, double param1 = 100, double param2 = 100, int minRadius = 0, int maxRadius = 0)
        {

            CircleF[] circles;
            circles = CvInvoke.HoughCircles(this.thresholdimage,
                                                     method,
                                                     dp,
                                                     minDist,
                                                     param1,
                                                     param2,
                                                     minRadius,
                                                     maxRadius);
            Console.WriteLine(circles.Count());

            foreach (CircleF circle in circles)
            {
                this.jpg.Draw(circle, new Bgr(Color.Red), 2);
            }

            return this.jpg;
        }

        public void Show(Image<Gray, float> image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            ImageViewer.Show(image);
        }

    }

}
