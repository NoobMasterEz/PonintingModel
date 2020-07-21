using Emgu.CV;
using Emgu.CV.Structure;
using nom.tam.fits;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBControll.lib
{
     class MethodTranfrom
    {
        private Matrix<ushort> image;
    
        public MethodTranfrom(Matrix<ushort> imageinput)
        {
            this.image = imageinput;
        }

        
        public Matrix<ushort> GetRaw()
        {
            return this.image;
        }
        public Matrix<int> Convert1628()
        {
            Matrix<int> result = new Matrix<int>(this.image.Rows, this.image.Cols);
            Parallel.For(0, this.image.Cols, (i) =>
            {
                for (int j = 0; j < this.image.Rows; j++)
                {
                    result.Data[i, j] = (int)this.image.Data[i, j] / 255;
                }
            });

            return result;
        }
    }


    class MethodStaticFomula
    {
        public Point CenterOfCircle(Rectangle rect) => new Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);

        public static double[] InvertStandardCoordi(double x, double y, double ra, double dec)
        {
            dec = dec * (Math.PI / 180);
            ra = ra * (Math.PI / 180);

            double afa = ra + (Math.Atan(x / (Math.Cos(dec) - (y * Math.Sin(dec)))));
            double bata = Math.Asin(((Math.Sin(dec) + (y * Math.Cos(dec))) / (Math.Sqrt(1 + Math.Pow(x, 2.0) + Math.Pow(y, 2.0)))));
            return new double[] { afa * (180 / Math.PI), bata * (180 / Math.PI) };
        }

        public static double[] StandardCoordi(double ra, double dec, double objra, double objdec)
        {
            /* α, right ascension
             * δ, declination
             */
            double btm = (Math.Sin(objdec) * Math.Sin(dec) + (Math.Cos(objdec - dec)));
            double x = (Math.Cos(dec) * Math.Sin(objra - ra)) / btm;
            double y = ((Math.Cos(objdec) * Math.Sin(dec)) - (Math.Sin(objdec) * Math.Cos(dec) * Math.Cos(objdec - dec))) / btm;

            return new double[] { x * (180 / Math.PI), y * (180 / Math.PI) };
        }

        public static double[] Trafrom2Polar(double X, double Y, int weight, int height)
        { 
            return new double[] { X-(0.5*weight),(-Y+(0.5*height))};
        }
    }

    }




