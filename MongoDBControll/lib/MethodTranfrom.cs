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
        public MethodTranfrom()
        {
        }

        public Point CenterOfCircle(Rectangle rect) => new Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);

     
    }
}

