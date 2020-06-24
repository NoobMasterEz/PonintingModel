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

namespace MongoDBControll.lib
{
    class EmguCv
    {
        //private Mat imagemat;
        private Image<Bgr, byte> newimage;
        private Image<Emgu.CV.Structure.Gray, byte> gray;
        private Image<Emgu.CV.Structure.Gray, byte> thresholdimage;
        
        public EmguCv()
        {

        }

        
        public Matrix<int> Convert1628(Matrix<ushort> imageinput)
        {
            Matrix<int> result = new Matrix<int>(imageinput.Rows, imageinput.Cols);
            for(int i =0; i < imageinput.Cols;i++)
            {
                for(int j =0; j < imageinput.Rows;j++)
                {
                    result.Data[i,j]=(int) imageinput.Data[i, j] / 255;
                }
            }
            
            return result;
        }



        /// <summary>
        ///  this fuction build 2 value 
        ///  - newimage (bgr)
        ///  - gray 
        /// </summary>
        /// <param name="namefile">location file </param>
        public void CreateImag(Image<Bgr, byte> file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            //Matrix<ushort> image = FitsFile.GenerateImage(file);

            //Matrix<int> image8bit = Convert1628(image);
            /*
            ushort LowerValue, UpperValue;
            double LowerPercen, UpperPercen;

            FitsFile.GetStrecthProfile(out LowerPercen, out UpperPercen);
            FitsFile.GetUpperAndLowerShortBit(image, out LowerValue, out UpperValue, LowerPercen, UpperPercen);
            Matrix<ushort> imgJPG = FitsFile.StretchImageU16Bit(image, LowerValue, UpperValue);
            Mat imgJPG = CvInvoke.Imread(namefile);
            Image<Bgr, byte> imagemat = imgJPG.ToImage<Bgr, byte>();
            Image<Bgr, byte> newlayer = image8bit.Mat.ToImage<Bgr, byte>();
           */
            //ImageViewer.Show(imagemat);
            Image<Bgr, byte> newlayer = file;
            this.newimage = newlayer;
            //this.newimage = imagemat; // img to bgr
            //this.gray = imagemat.ToImage<Gray, byte>(); // img to gray 
            this.gray = newlayer.Convert<Gray, Byte>();




        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">threshold value</param>
        /// <param name="max">maximum value to use with the THRESH_BINARY and THRESH_BINARY_INV thresholding types.</param>
        /// <param name="nametype">See in auto comple</param>
        /// <returns></returns>
        public Image<Emgu.CV.Structure.Gray, byte> Thresholding(int min,int max, ThresholdType nametype)
        {
            
            this.thresholdimage = this.gray.CopyBlank(); //coppy gray to result thresholding
            ///
            CvInvoke.Threshold(gray, this.thresholdimage, min, max, nametype);
            return this.thresholdimage;
        }


        public Tuple<Image<Bgr,byte>,int> SegmentionWatershed(int threshmin)
        {
            //Mat3b src = imread("path_to_image");
            double[] min, max;
            Point[] pmin, pmax;
            //cvtColor(src, gray, COLOR_BGR2GRAY);
            Image<Gray, byte> gray = this.gray;
            Image<Gray, byte> thresh=this.gray.CopyBlank();
            Image<Gray, float> thresh2 = new Image<Gray, float>(gray.Width, gray.Height);

            //threshold(gray, thresh, 0, 255, THRESH_BINARY | THRESH_OTSU);
            CvInvoke.Threshold(gray.SmoothGaussian(3), thresh, threshmin, 255, ThresholdType.Binary | ThresholdType.Otsu);

            
            // noise removal
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(1, 1));

            //Mat1b opening;
            //morphologyEx(thresh, opening, MORPH_OPEN, kernel, Point(-1, -1), 2);
            Image<Gray, byte> opening = thresh.MorphologyEx(MorphOp.Open, kernel, new Point(1, 1), 2, BorderType.Default, new MCvScalar(255));
            
            Image<Gray, byte> sure_bg = opening.Dilate(3);
            Image<Gray, float> dist_transform = new Image<Gray, float>(opening.Width, opening.Height);
            CvInvoke.DistanceTransform(opening, dist_transform, null, DistType.L2 , 5);

            

            dist_transform.MinMax(out min, out max, out pmin, out pmax);
            //Console.WriteLine(max[0]);
            CvInvoke.Threshold(dist_transform,thresh2, max[0]*0.1, 255,0);
            Image<Gray, Byte> dist_8u = thresh2.Convert<Gray, Byte>();
            //ImageViewer.Show(dist_8u);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //Mat hierarchy = new Mat() ;
            CvInvoke.FindContours(dist_8u, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
            Console.WriteLine(contours.Size);
            for (int i = 0; i < contours.Size; i++)
                {
                 Rectangle r = CvInvoke.BoundingRectangle(contours[i]);
                 this.newimage.Draw(r,new Bgr(Color.Red));
             
               }
            //ImageViewer.Show(this.newimage);

            return Tuple.Create(this.newimage, contours.Size);       
                
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
        public Image<Emgu.CV.Structure.Bgr, byte> HouCircles(HoughType method,double dp,double minDist,double param1 = 100,double param2 = 100,int minRadius = 0,int maxRadius = 0)
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
            /*
            foreach (CircleF circle in circles)
            {
                this.newimage.Draw(circle, new Bgr(Color.Red), 2);
            }
            */
            //ImageViewer.Show(this.newimage);
            return newimage;
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
