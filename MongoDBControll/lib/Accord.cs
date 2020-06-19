using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Imaging.Filters;
using System.Drawing;
using Accord.DataSets;

namespace MongoDBControll.lib

{
    class Accord
    {
        private const string Name = @"C:\\Users\\specter\\Desktop\\Mongo\\MongoDBControll\\lib\\image\2020_06_16T11.59.34.943Z_V.jpg";

        public void test()
        {
            TestImages t =new TestImages();
            Bitmap baboon = t.GetImage(Name);
        }
    }
}
