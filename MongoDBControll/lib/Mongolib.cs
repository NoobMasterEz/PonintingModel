using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MongoDBControll.lib
{
    
    class Mongolib 
    {
        private readonly MongoClient db_client;
        private string _namedatabase;
        private GaiaInfo11 type;
        private string _NAMECOLLECTION;
        public Mongolib(string data, string namedatabase)
        {
            // call Gaia info 
            type = new GaiaInfo11();
           
            //Construct

            this.db_client = new MongoClient(data);
            
            this.Namedatabase = namedatabase;
        }

        public string NAMECOLLECTION
        {
            /**
             *  :create fucation getter and setter for private variable
             */
            get { return this._NAMECOLLECTION; }
            set { this._NAMECOLLECTION = value; }
        }
        public string Namedatabase
        {
            /**
             *  :create fucation getter and setter for private variable
             */
            get { return this._namedatabase; }
            set { this._namedatabase = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <returns> type list </returns>
        private static IFindFluent<GaiaInfo11, GaiaInfo11> Get(IMongoCollection<GaiaInfo11> collection, FilterDefinition<GaiaInfo11> filter)
        {
            
            return collection.Find(filter); // get result from filter 
        }
   
        private static IMongoCollection<GaiaInfo11> CollectCollection(string NAMECOLLECTION, IMongoDatabase data)
        {

            
            return data.GetCollection<GaiaInfo11>(NAMECOLLECTION); // get collection
            
        }

        //private static FilterDefinition<GaiaInfo11> FilterGeoMultiple(int x, int y, double r)
        //{
        //    return Builders<GaiaInfo11>.Filter.And(Builders<GaiaInfo11>.Filter.GeoWithinCenterSphere(u => u.Location, x, y, r), Builders<GaiaInfo11>.Filter.GeoWithinCenterSphere(u => u.phot_g_mean_mag, x, y, r));
        //}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">lat</param>
        /// <param name="y">long</param>
        /// <param name="r"> radius</param>
        /// <returns>type FilterDefinition</returns>
        private static FilterDefinition<GaiaInfo11> FilterGeo(int x, int y, double r)
        {
            return Builders<GaiaInfo11>.Filter.GeoWithinCenterSphere(u => u.Location, x, y, r); // fuction filter center sh
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1">x low</param>
        /// <param name="y1">y low </param>
        /// <param name="x2">x hight</param>
        /// <param name="y2">y hight </param>
        /// <returns> type filterDefinition </returns>
        private static FilterDefinition<GaiaInfo11> FilterBox(double x1, double y1, double x2, double y2)
        {
            return Builders<GaiaInfo11>.Filter.GeoWithinBox(u => u.Location, x1, y1, x2, y2); // fuction filter box 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="distand"></param>
        /// <returns></returns>
        private static FilterDefinition<GaiaInfo11> FilterNear(GeoJsonPoint<GeoJson2DGeographicCoordinates> point, double distand)
        {
            return Builders<GaiaInfo11>.Filter.Near(u => u.Location, point, distand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        private static FilterDefinition<GaiaInfo11> FilterPolygon(double[,] polygon)
        {
            return Builders<GaiaInfo11>.Filter.GeoWithinPolygon(u => u.Location, polygon);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private  IMongoDatabase Database()
        {
           
            return this.db_client.GetDatabase(Namedatabase); //get data base
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"> data base </param>
        /// <returns>type boolen</returns>
        private bool CheckConnected(IMongoDatabase data)
        {
            return data.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
        }

        /// <summary>
        ///  
        /// </summary>
       
        /// <param name="x"> latitude</param>
        /// <param name="y">longitude</param>
        /// <param name="r">radius</param>
        /// <returns> type list </returns>
        public IFindFluent<GaiaInfo11, GaiaInfo11> GeocenterSpherestring(int x ,int y , double r)
        {
            /**
             * : putdata to argument name of collection and x , y, r .
             * : check data is connected from database .
             * : filter GeoWithCenter. 
             * : return type list  .
             *
             */

            IMongoDatabase data = Database();

            Console.WriteLine(string.Format("[Status {0}] -> GeocenterSpherestring Connect in data base name {1} ", CheckConnected(data), NAMECOLLECTION)); // check in connect to database 
            IMongoCollection<GaiaInfo11> collection = CollectCollection(NAMECOLLECTION, data);
            FilterDefinition<GaiaInfo11> filter = FilterGeo(x, y, r);

            return Get(collection, filter);
           


        }

            /// <summary>
            ///  
            /// </summary>
            /// <param name="x"> latitude</param>
            /// <param name="y">longitude</param>
            /// <param name="r">radius</param>
            /// <returns> type list </returns>
            //public IFindFluent<GaiaInfo11, GaiaInfo11> GeocenterSpherestringMuti( int x, int y, double r)
            //{
            //    /**
            //     * : putdata to argument name of collection and x , y, r .
            //     * : check data is connected from database .
            //     * : filter GeoWithCenter. 
            //     * : return type list  .
            //     *
            //     */

            //    IMongoDatabase data = Database();

            //    Console.WriteLine(string.Format("[Status {0}] -> GeocenterSpherestringMuti Connect in data base name {1} ", CheckConnected(data), NAMECOLLECTION)); // check in connect to database 
            //    IMongoCollection<GaiaInfo11> collection = CollectCollection(NAMECOLLECTION, data);
            //    FilterDefinition<GaiaInfo11> filter = FilterGeoMultiple(x, y, r);
            //    return Get(collection, filter);



            //}



            /// <summary>
            /// 
            /// </summary>
            /// <param name="x1">x low </param>
            /// <param name="y1">y low </param>
            /// <param name="x2">x high </param>
            /// <param name="y2">y high </param>
            /// <returns>type object</returns>
            public IFindFluent<GaiaInfo11, GaiaInfo11> GeowithBox( double x1,double y1,double x2,double y2)
        {
            IMongoDatabase data = Database();
            Console.WriteLine(string.Format("[Status {0}] -> GeowithBox Connect in data base name {1} ", CheckConnected(data), NAMECOLLECTION)); // check in connect to database 
            IMongoCollection<GaiaInfo11> collection = CollectCollection(NAMECOLLECTION, data);
            FilterDefinition<GaiaInfo11> filter = FilterBox(x1, y1, x2, y2);
            return Get(collection, filter);
        }

        public IFindFluent<GaiaInfo11, GaiaInfo11> GeoPolygon( double [,] polygon)
        {
            IMongoDatabase data = Database();
            Console.WriteLine(string.Format("[Status {0}] -> GeoPolygon Connect in data base name {1} ", CheckConnected(data), NAMECOLLECTION)); // check in connect to database 
            IMongoCollection<GaiaInfo11> collection = CollectCollection(NAMECOLLECTION, data);
            FilterDefinition<GaiaInfo11> filter = FilterPolygon(polygon);
            return Get(collection,filter);
        }

        public IFindFluent<GaiaInfo11, GaiaInfo11> Near(int pos1, int pos2, double distend)
        {
            IMongoDatabase data = Database();
            Console.WriteLine(string.Format("[Status {0}] -> Near Connect in data base name {1} ", CheckConnected(data), NAMECOLLECTION)); // check in connect to database 
            GeoJsonPoint<GeoJson2DGeographicCoordinates> point = GeoJson.Point(GeoJson.Geographic(pos1, pos2));
            IMongoCollection<GaiaInfo11> collection = CollectCollection(NAMECOLLECTION, data);
            FilterDefinition<GaiaInfo11> filter=FilterNear(point, distend);
            return Get(collection, filter);
        }

        public void DebugConsoleWite()
        {
            // DEBUG Getch(); C++ 

        #if DEBUG
                    Console.WriteLine("Press enter to close...");
                    Console.ReadLine();
        #endif
        }
    }
}
