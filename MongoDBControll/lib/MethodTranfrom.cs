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
            for (int i = 0; i < this.image.Cols; i++)
            {
                for (int j = 0; j < this.image.Rows; j++)
                {
                    result.Data[i, j] = (int) this.image.Data[i, j] / 255;
                }
            }

            return result;
        }
    }


    class MethodStaticFomula
    {
        public Point CenterOfCircle(Rectangle rect) => new Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);

        public static double[] StandardCoordi(double ra, double dec, double objra, double objdec)
        {
            /* α, right ascension
             * δ, declination
             */
            double btm = (Math.Sin(objdec) * Math.Sin(dec) + (Math.Cos(dec - objdec)));
            double x = (Math.Cos(dec) * Math.Sin(ra - objra))/btm;
            double y = ((Math.Cos(objdec) * Math.Sin(dec)) - (Math.Sin(objdec) * Math.Cos(dec) * Math.Cos(objdec - dec))) / btm;
            
            return new double[] { x, y };  
        }
    }


}

