using System;
using System.Reflection.Metadata;
using GeodesyLib.DataTypes;

namespace GeodesyLib
{
    public static class RhumbCalculations
    {
        /// <summary>
        /// Since a rhumb line is a straight line on a Mercator projec­tion,
        /// the distance between two points along a rhumb line is the length of that line (by Pythagoras);
        /// but the distor­tion of the projec­tion needs to be compensated for.
        /// </summary>
        /// <param name="from">starting coordinate, starting point of a destination</param>
        /// <param name="to">final coordinate, end point of a destination</param>
        /// <returns>Returns the distance between two points along a rhumb line</returns>
        public static double CalculateRhumbDistance(this Coordinate from, Coordinate to)
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

            double q = Math.Abs(delta) > 10e-12
                ? latDelta / delta
                : Math.Cos(lat1); // E-W course becomes ill-conditioned with 0/0


            if (Math.Abs(lonDelta) > Constants.PI)
            {
                lonDelta = lonDelta > 0 ? -(2 * Constants.PI - lonDelta) : (2 * Constants.PI + lonDelta);
            }

            double dist = Math.Sqrt(latDelta * latDelta + q * q * lonDelta * lonDelta) * Constants.RADIUS;


            return dist;
        }

        public static double CalculateRhumbInitialBearing(this Coordinate from, Coordinate to)
        {
            double lat1 = from.Lat.ConvertToRadian();
            double lon1 = from.Lon.ConvertToRadian();

            double lat2 = to.Lat.ConvertToRadian();
            double lon2 = to.Lon.ConvertToRadian();

            double lonDelta = lon2 - lon1;

            double deltaResult = Math.Log(
                Math.Tan(Constants.PI / 4 + lat2 / 2) / Math.Tan(Constants.PI / 4 + lat1 / 2)
            );

            if (lonDelta > Constants.PI)
            {
                lonDelta = lonDelta > 0 ? -(2 * Constants.PI - lonDelta) : (2 * Constants.PI + lonDelta);
            }

            double bearing = Math.Atan2(lonDelta, deltaResult) * 180 / Constants.PI;

            return bearing;
        }


        public static Coordinate CalculateRhumbDestination(this Coordinate from,
            double distance,
            double bearing)
        {

            double rBearing = bearing.ConvertToRadian();
            double lat1 = from.Lat.ConvertToRadian();
            double lon1 = from.Lon.ConvertToRadian();

            double delta = distance / Constants.RADIUS;
            double deltaLat = delta * Math.Cos(rBearing);

            double lat2 = lat1 + deltaLat;

            double deltaResult = Math.Log(
                Math.Tan(lat2 / 2 + Constants.PI / 4) / Math.Tan(lat1 / 2 + Constants.PI / 4)
            );

            double q = Math.Abs(deltaResult) > 10e-12 ? deltaLat / deltaResult : Math.Cos(lat1); // E-W course becomes ill-conditioned with 0/0
            double deltaLon = delta * Math.Sin(rBearing) / q;
            double lon2 = lon1 + deltaLon;

            return new Coordinate(
                lat2.ConvertToDegree(),
                lon2.ConvertToDegree()
                );
        }
        
        
        
        
        
    }
}