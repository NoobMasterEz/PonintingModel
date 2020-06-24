using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Imaging.Filters;
using System.Drawing;
using Accord.DataSets;
using Accord.Imaging.Formats;

using OpenTK.Graphics.ES10;
using System.Drawing.Imaging;
using Emgu.CV.UI;

namespace MongoDBControll.lib

{
    class Accord
    {
        
        public void test(String Name)
        {
            
            Bitmap img = ImageDecoder.DecodeFromFile(Name);
            
        }
    }
}
