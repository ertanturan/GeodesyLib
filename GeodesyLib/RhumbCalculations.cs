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
        /// <returns>Returns the rhumb distance between two points along a rhumb line</returns>
        public static double CalculateRhumbDistance(this Coordinate from, Coordinate to)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lon1 = from.Longitude.ConvertDegreeToRadian();

            double lat2 = to.Latitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

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

        /// <summary>
        /// A rhumb line is a straight line on a Mercator projection, with an angle on the projec­tion equal to the compass bearing.
        /// </summary>
        /// <param name="from">Starting point</param>
        /// <param name="to">End point</param>
        /// <returns>Returns the rhumb bearing/direction from one coordinate to another.</returns>
        public static double CalculateRhumbBearing(this Coordinate from, Coordinate to)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lon1 = from.Longitude.ConvertDegreeToRadian();

            double lat2 = to.Latitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

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

        /// <summary>
        /// Given a start point and a distance d along constant bearing θ, this will calculate the destina­tion point.
        /// If you maintain a constant bearing along a rhumb line, you will gradually spiral in towards one of the poles.
        /// </summary>
        /// <param name="from">Starting point</param>
        /// <param name="distance">Final point</param>
        /// <param name="bearing">Bearing/direction</param>
        /// <returns>Returns the rhumb destination coordinate.</returns>
        public static Coordinate CalculateRhumbDestination(this Coordinate from,
            double distance,
            double bearing)
        {
            double rBearing = bearing.ConvertDegreeToRadian();
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lon1 = from.Longitude.ConvertDegreeToRadian();

            double delta = distance / Constants.RADIUS;
            double deltaLat = delta * Math.Cos(rBearing);

            double lat2 = lat1 + deltaLat;

            double deltaResult = Math.Log(
                Math.Tan(lat2 / 2 + Constants.PI / 4) / Math.Tan(lat1 / 2 + Constants.PI / 4)
            );

            // E-W course becomes ill-conditioned with 0/0
            double q = Math.Abs(deltaResult) > 10e-12 ? deltaLat / deltaResult : Math.Cos(lat1);

            double deltaLon = delta * Math.Sin(rBearing) / q;
            double lon2 = lon1 + deltaLon;

            return new Coordinate(
                lat2.ConvertRadianToDegree(),
                lon2.ConvertRadianToDegree()
            );
        }


        /// <summary>
        /// This formula for calculating the ‘loxodromic midpoint’,
        /// the point half-way along a rhumb line between two points,
        /// is due to Robert Hill and Clive Tooth.
        /// </summary>
        /// <param name="from">Starting point</param>
        /// <param name="to">Final point</param>
        /// <returns>Returns the rhumb mid point between two coordinates.</returns>
        public static Coordinate CalculateRhumbMidPoint(this Coordinate from, Coordinate to)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lon1 = from.Longitude.ConvertDegreeToRadian();

            double lat2 = to.Latitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

            double deltaLon = lon2 - lon1;

            double latMid = (lat1 + lat2) / 2;
            double quarterPi = Constants.PI * 4;

            double f1 = Math.Tan(quarterPi + lat1 / 2);
            double f2 = Math.Tan(quarterPi + lat2 / 2);
            double f3 = Math.Tan(quarterPi + latMid / 2);

            double lonMid = (deltaLon * Math.Log(f3) + lon1 * Math.Log(f2) - lon2 * Math.Log(f1))
                            / Math.Log(f2 / f1);

            //longitude normalisation
            lonMid = (lonMid + 540) % 360 - 180;

            if (!double.IsFinite(lonMid))
            {
                lonMid = (lon1 + lon2) / 2;
            }

            return new Coordinate(latMid.ConvertRadianToDegree(),
                lonMid.ConvertRadianToDegree());
        }
        
    }
}