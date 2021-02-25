using System;
using GeodesyLib.DataTypes;

namespace GeodesyLib
{
    public static class Calculations
    {
        // This uses the ‘haversine’ formula to calculate the great-circle distance between two points – that is,
        // the shortest distance over the earth’s surface – giving an ‘as-the-crow-flies’ distance between the points
        // (ignoring any hills they fly over, of course!).
        public static double HaversineDistance(Coordinate from , Coordinate to  )
        {
            
            double lat1 = Utility.ConvertToRadian(to.Lat);
            double lat2 = Utility.ConvertToRadian(from.Lat);

            double lon1 = Utility.ConvertToRadian(to.Lon);
            double lon2 = Utility.ConvertToRadian(from.Lon);

            double latDelta = lat2 - lat1;
            double lonDelta = lon2 - lon1;

            double a = Math.Pow(Math.Sin(latDelta / 2),2)  +
                       Math.Cos(lat1) * Math.Cos(lat2) * 
                       Math.Pow( Math.Sin(lonDelta / 2),2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = Constants.RADIUS * c;

            return d;
        }

        public static double SphericalLawOfCosines(Coordinate from, Coordinate to)
        {

            double lat1 = Utility.ConvertToRadian(from.Lat);
            double lat2 = Utility.ConvertToRadian(to.Lat);

            double lon1 = Utility.ConvertToRadian(from.Lon);
            double lon2 = Utility.ConvertToRadian(to.Lon);

            double lonDelta = lon2 - lon1;
            double d = Math.Acos(
                           Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Acos(lonDelta)) *
                       Constants.RADIUS;

            return 0;
        }


        public static double CalculateBearing(Coordinate from, Coordinate to)
        {
            return 0;
        }
        
        
    }
}