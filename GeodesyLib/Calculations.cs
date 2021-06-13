using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata;
using GeodesyLib.Attributes;
using GeodesyLib.DataTypes;
using GeodesyLib.Exceptions;

namespace GeodesyLib
{
    public static class Calculations
    {
        // This uses the ‘haversine’ formula to calculate the great-circle distance between two points – that is,
        // the shortest distance over the earth’s surface – giving an ‘as-the-crow-flies’ distance between the points
        // (ignoring any hills they fly over, of course!).
        public static double HaversineDistance([NotNull] this Coordinate from,
            [NotNull] Coordinate to)
        {
            double lat1 = to.Latitude.ConvertDegreeToRadian();
            double lat2 = from.Latitude.ConvertDegreeToRadian();

            double lon1 = to.Longitude.ConvertDegreeToRadian();
            double lon2 = from.Longitude.ConvertDegreeToRadian();

            double latDelta = lat2 - lat1;
            double lonDelta = lon2 - lon1;

            double a = Math.Pow(Math.Sin(latDelta / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(lonDelta / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = Constants.RADIUS * c;

            return d;
        }

        /// <summary>
        ///    This makes the simpler law of cosines a reasonable 1-line alternative to the haversine formula
        /// for many geodesy purposes (if not for astronomy). 
        /// The choice may be driven by programming language, processor, coding context, available trig
        /// functions (in different languages), etc – and, for very small distances an equirectangular approximation
        /// may be more suitable.
        /// </summary>
        /// <param name="from">starting coordinate, starting point of a destination </param>
        /// <param name="to">final coordinate, end point of a destination</param>
        /// <returns></returns>
        public static double CalculateSphericalLawOfCosines([NotNull] this Coordinate from,
            [NotNull] Coordinate to)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lat2 = to.Latitude.ConvertDegreeToRadian();

            double lon1 = from.Longitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

            double lonDelta = lon2 - lon1;

            double d = Math.Acos(
                           Math.Sin(lat1) * Math.Sin(lat2) +
                           Math.Cos(lat1) *
                           Math.Cos(lat2) * Math.Cos(lonDelta)) *
                       Constants.RADIUS;

            return d;
        }

        /// <summary>
        ///  This formula is for the initial bearing (sometimes referred to as forward azimuth) which if followed
        /// in a straight line along a great-circle arc will take you from the start point to the end point
        /// 
        /// </summary>
        /// <param name="from">starting coordinate, starting point of a destination </param>
        /// <param name="to">final coordinate, end point of a destination</param>
        /// <returns></returns>
        public static double CalculateBearing(this Coordinate from, Coordinate to)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lat2 = to.Latitude.ConvertDegreeToRadian();

            double lon1 = from.Longitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

            double lonDelta = lon2 - lon1;


            double teta = Math.Atan2(Math.Sin(lonDelta) * Math.Cos(lat2),
                Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lonDelta)
            );

            double bearing = (teta * 180 / Constants.PI + 360) % 360;

            return bearing;
        }

        public static Coordinate CalculateMidPoint([NotNull] this Coordinate from,
            [NotNull] Coordinate to)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lat2 = to.Latitude.ConvertDegreeToRadian();

            double lon1 = from.Longitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

            double lonDelta = lon2 - lon1;

            double bX = Math.Cos(lat2) * Math.Cos(lonDelta);
            double bY = Math.Cos(lat2) * Math.Sin(lonDelta);

            double cosBx = Math.Cos(lat1) + bX;

            double cosBxPow = Math.Pow(cosBx, 2);
            double bY2 = Math.Pow(bY, 2);


            double newLat = Math.Atan2(
                Math.Sin(lat1) + Math.Sin(lat2),
                Math.Sqrt(cosBxPow + bY2)
            );


            double newLon = lon1 + Math.Atan2(bY, Math.Cos(lat1) + bX);
            newLon = (newLon + 540) % 360 - 180;

            return new Coordinate(
                newLat.ConvertRadianToDegree(),
                newLon.ConvertRadianToDegree());
        }

        /// <summary>
        /// An intermediate point at any fraction along the great circle path between two points
        /// </summary>
        /// <param name="from">starting point </param>
        /// <param name="to">final point</param>
        /// <param name="distance">arc distance between starting and final points</param>
        /// <param name="fraction">f=0 is the starting point , f=1 is the end point ,
        /// 0.xxx is the between points</param>
        /// <returns></returns>
        public static Coordinate CalculateIntermediatePointByFraction([NotNull] this Coordinate from,
            [NotNull] Coordinate to, double fraction)
        {

            double angularDistance = from.HaversineDistance(to);
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lat2 = to.Latitude.ConvertDegreeToRadian();

            double lon1 = from.Longitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

            double angularSin = Math.Sin(angularDistance);

            double a = Math.Sin((1 - fraction) * angularDistance) / angularSin;
            double b = Math.Sin(fraction * angularDistance) / angularSin;

            double x = a * Math.Cos(lat1) * Math.Cos(lon1) + b * Math.Cos(lat2) * Math.Cos(lon2);
            double y = a * Math.Cos(lat1) * Math.Sin(lon1) + b * Math.Cos(lat2) * Math.Sin(lon2);
            double z = a * Math.Sin(lat1) + b * Math.Sin(lat2);

            double newLat = Math.Atan2(z, Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
            double newLon = Math.Atan2(y, x);

            return new Coordinate(
                newLat.ConvertRadianToDegree(),
                newLon.ConvertRadianToDegree());
        }

        public static Coordinate[] Get_N_AmountOfCoordinatesBetween(this Coordinate from,
            Coordinate to, int n)
        {
            Coordinate[] result = new Coordinate[n];
            
            double fraction = 1d / n;

            for (int i = 0; i < n; i++)
            {
                Coordinate fracturedCoordinate = from.CalculateIntermediatePointByFraction(
                    to,
                    fraction * i);

                result[i] = fracturedCoordinate;
            }
            
            for (int i = 0; i < result.Length; i++)
            {
                Console.WriteLine(result[i]);
            }

            return result;
        }


        public static Coordinate CalculateDestinationPoint([NotNull] this Coordinate from,
            double distance
            , double bearing)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lon1 = from.Longitude.ConvertDegreeToRadian();

            double rBearing = bearing.ConvertDegreeToRadian();

            double distanceDividedByRadius = distance / Constants.RADIUS;

            double newLat = Math.Asin(Math.Sin(lat1) * Math.Cos(distanceDividedByRadius) +
                                      Math.Cos(lat1) * Math.Sin(distanceDividedByRadius) *
                                      Math.Cos(rBearing)
            );

            double newLon = lon1 + Math.Atan2(
                Math.Sin(rBearing) * Math.Sin(distanceDividedByRadius) * Math.Cos(lat1),
                Math.Cos(distanceDividedByRadius) - Math.Sin(lat1) * Math.Sin(newLat)
            );

            double clampedLon = (newLon + 540) % 360 - 180;

            return new Coordinate(
                newLat.ConvertRadianToDegree(),
                clampedLon.ConvertRadianToDegree());
        }
        
    }
}