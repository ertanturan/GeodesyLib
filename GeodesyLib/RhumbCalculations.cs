using System;
using GeodesyLib.DataTypes;

namespace GeodesyLib
{
    public static class RhumbCalculations
    {
        public static double RhumbDistance(this Coordinate from, Coordinate to)
        {
            
            double lat1 = from.Lat.ConvertToRadian();
            double lon1 = from.Lon.ConvertToRadian();
            
            double lat2 = to.Lat.ConvertToRadian();
            double lon2 = to.Lon.ConvertToRadian();

            double delta = Math.Log(

                Math.Tan(
                    Math.PI / 4 + lat2 / 2) / Math.Tan(Math.PI / 4 + lat1 / 2));


            double latDelta = lat2 - lat1;
            double lonDelta = lon2 - lon1;
            
            double q = Math.Abs(delta) > 10e-12 ? latDelta / delta : Math.Cos(lat1); // E-W course becomes ill-conditioned with 0/0


            if (Math.Abs(lonDelta) > Constants.PI)
            {
                lonDelta = lonDelta>0 ? -(2*Constants.PI-lonDelta) : (2*Constants.PI+lonDelta);
            }
            
            double dist = Math.Sqrt(latDelta*latDelta + q*q*lonDelta*lonDelta) * Constants.RADIUS;


            return dist;
        }
    }
}