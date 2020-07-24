using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBControll.lib
{

    class JsonAstro
    {
        private string _name;
        private int _weight;
        private int _height;
        private string _datetime;
        private double[][] _data;
        private double _numplate;
        public string name
        {
            get => _name;
            set => _name = value; //Name of fitfile 
        }
        public int weight
        {
            get => _weight;
            set => _weight = value; //size of image 
        }

        public int  height
        {
            get => _height;
            set => _height = value;//size of image 
        }
        public string datetime
        {
            get => _datetime;
            set => _datetime = value;//Date time stemp
        }
        public double[][] data
        {
            get => _data;
            set => _data = value;// data array1 dimison  
        }

        public double numplate
        {
            get => _numplate;
            set => _numplate = value;// data array1 dimison  
        }
        public static void Save(string path,string json)
        {
            System.IO.File.WriteAllText(path, json);
        }

    }

}
