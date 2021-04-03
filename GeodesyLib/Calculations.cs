using System;
using System.Reflection.Metadata;
using GeodesyLib.DataTypes;
using GeodesyLib.Exceptions;

namespace GeodesyLib
{
    public static class Calculations
    {
        // This uses the ‘haversine’ formula to calculate the great-circle distance between two points – that is,
        // the shortest distance over the earth’s surface – giving an ‘as-the-crow-flies’ distance between the points
        // (ignoring any hills they fly over, of course!).
        public static double HaversineDistance(this Coordinate from, Coordinate to)
        {
            double lat1 = to.Lat.ConvertToRadian();
            double lat2 = @from.Lat.ConvertToRadian();

            double lon1 = to.Lon.ConvertToRadian();
            double lon2 = @from.Lon.ConvertToRadian();

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
        public static double SphericalLawOfCosines(this Coordinate from, Coordinate to)
        {
            double lat1 = @from.Lat.ConvertToRadian();
            double lat2 = to.Lat.ConvertToRadian();

            double lon1 = @from.Lon.ConvertToRadian();
            double lon2 = to.Lon.ConvertToRadian();

            double lonDelta = lon2 - lon1;
            double d = Math.Acos(
                           Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * 
                           Math.Cos(lat2) * Math.Acos(lonDelta)) *
                       Constants.RADIUS;

            return 0;
        }

        /// <summary>
        ///  This formula is for the initial bearing (sometimes referred to as forward azimuth) which if followed
        /// in a straight line along a great-circle arc will take you from the start point to the end point
        /// 
        /// </summary>
        /// <param name="from">starting coordinate, starting point of a destination </param>
        /// <param name="to">final coordinate, end point of a destination</param>
        /// <returns></returns>
        public static double CalculateInitialBearing(this Coordinate from, Coordinate to)
        {
            double lat1 = @from.Lat.ConvertToRadian();
            double lat2 = to.Lat.ConvertToRadian();

            double lon1 = @from.Lon.ConvertToRadian();
            double lon2 = to.Lon.ConvertToRadian();

            double lonDelta = lon2 - lon1;


            double teta = Math.Atan2(Math.Sin(lonDelta) * Math.Cos(lat2),
                Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lonDelta)
            );

            double bearing = (teta * 180 / Constants.PI + 360) % 360;

            return bearing;
        }

        public static Coordinate CalculateMidPoint(this Coordinate from, Coordinate to)
        {
            double lat1 = @from.Lat.ConvertToRadian();
            double lat2 = to.Lat.ConvertToRadian();

            double lon1 = @from.Lon.ConvertToRadian();
            double lon2 = to.Lon.ConvertToRadian();

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
                newLat.ConvertToDegree(),
                newLon.ConvertToDegree());
        }

        /// <summary>
        /// An intermediate point at any fraction along the great circle path between two points
        /// </summary>
        /// <param name="from">starting point </param>
        /// <param name="to">final point</param>
        /// <param name="distance">arc distance between starting and final points</param>
        /// <param name="fraction">f=0 is the starting point , f=1 is the end point , 0.xxx is the between points</param>
        /// <returns></returns>
        public static Coordinate CalculateIntermediatePoint(this Coordinate from, Coordinate to,
            double distance, double fraction)
        {
            double lat1 = @from.Lat.ConvertToRadian();
            double lat2 = to.Lat.ConvertToRadian();

            double lon1 = @from.Lon.ConvertToRadian();
            double lon2 = to.Lon.ConvertToRadian();

            double angularSin = Math.Sin(distance);

            double a = Math.Sin((1 - fraction) * distance) / angularSin;
            double b = Math.Sin(fraction * distance) / angularSin;

            double x = a * Math.Cos(lat1) * Math.Cos(lon1) + b * Math.Cos(lat2) * Math.Cos(lon2);
            double y = a * Math.Cos(lat1) * Math.Sin(lon1) + b * Math.Cos(lat2) * Math.Sin(lon2);
            double z = a * Math.Sin(lat1) + b * Math.Sin(lat2);

            double newLat = Math.Atan2(z, Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
            double newLon = Math.Atan2(y, x);

            return new Coordinate(
                newLat.ConvertToDegree(),
                newLon.ConvertToDegree());
        }


        public static Coordinate CalculateDestinationPoint(this Coordinate from, double distance
            , double bearing)
        {
            double lat1 = from.Lat.ConvertToRadian();
            double lon1 = from.Lon.ConvertToRadian();

            double rBearing = bearing.ConvertToRadian();

            double distanceDividedByRadius = distance / Constants.RADIUS;

            double newLat = Math.Asin(Math.Sin(lat1) * Math.Cos(distanceDividedByRadius) +
                                      Math.Cos(lat1) * Math.Sin(distanceDividedByRadius) * Math.Cos(rBearing)
            );

            double newLon = lon1 + Math.Atan2(
                Math.Sin(rBearing) * Math.Sin(distanceDividedByRadius) * Math.Cos(lat1),
                Math.Cos(distanceDividedByRadius) - Math.Sin(lat1) * Math.Sin(newLat)
            );

            double clampedLon = (newLon + 540) % 360 - 180;

            return new Coordinate(
                newLat.ConvertToDegree(),
                clampedLon.ConvertToDegree());
        }

        // `INTERSECTION OF TWO PATHS` IS UNDER DEVELOPMENT...

        // public static Coordinate IntersectionOfTwoPaths(Coordinate firstCoordinate,
        //     double firstBearing,
        //     Coordinate secondCoordinate,
        //     double secondBearing)
        // {
        //     double lat1 = Utility.ConvertToRadian(firstCoordinate.Lat);
        //     double lat2 = Utility.ConvertToRadian(secondCoordinate.Lat);
        //
        //     double lon1 = Utility.ConvertToRadian(firstCoordinate.Lon);
        //     double lon2 = Utility.ConvertToRadian(secondCoordinate.Lon);
        //
        //
        //     double lonDelta = lon1 - lon2;
        //     double latDelta = lat1 - lat2;
        //
        //     double cross12, cross21;
        //
        //     double distance12 = 2 * Math.Asin(
        //         Math.Sqrt(
        //             Math.Pow(Math.Sin(latDelta / 2), 2) +
        //             Math.Cos(lat1) * Math.Cos(lat2) *
        //             Math.Pow(Math.Sin(lonDelta / 2), 2)
        //         )
        //     );
        //
        //
        //     double twoPi = 2 * Constants.PI;
        //     
        //     if ((lon2 - lon1) < 0)
        //     {
        //         cross12 = Math.Acos(
        //             (Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(distance12))
        //             /
        //             (Math.Sin(distance12) * Math.Cos(lat1))
        //         );
        //
        //         cross21 = twoPi - Math.Acos(
        //             (Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(distance12))
        //             /
        //             (Math.Sin(distance12) * Math.Cos(lat2))
        //         );
        //     }
        //     else
        //     {
        //         cross12 = twoPi - Math.Acos(
        //             (Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(distance12))
        //             /
        //             (Math.Sin(distance12) * Math.Cos(lat1))
        //         );
        //
        //         cross21 = Math.Acos(
        //             (Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(distance12))
        //             /
        //             (Math.Sin(distance12) * Math.Cos(lat2))
        //         );
        //     }
        //
        //     
        //     var ang1 = ((firstBearing-cross12+Constants.PI) % twoPi)-Constants.PI;
        //     var ang2 = ((cross21 - secondBearing + Constants.PI) % twoPi)-Constants.PI;
        //
        //
        //     if (Math.Sin(ang1)==0 & Math.Sin(ang2)==0)
        //     {
        //         throw new InfiniteIntersectionException(
        //             "There is an infinite amount of intersection with the given coordinates and bearings ..");
        //         
        //     }
        //     else if (Math.Sin(ang1)*Math.Sin(ang2)<0)
        //     {
        //         throw new IntersectionAmbiguousException(
        //             "Intersection is ambiguous. Therefore, can not calculate intersection coordinate");
        //     }
        //     else
        //     {
        //         ang1 = Math.Abs(ang1);
        //         ang2 = Math.Abs(ang2);
        //
        //         double ang3 = Math.Acos(
        //             (-Math.Cos(ang1) * Math.Cos(ang2)) + (Math.Sin(ang2) * Math.Cos(distance12))
        //         );
        //
        //
        //         double dst13 = Math.Atan2(
        //             Math.Sin(distance12) * Math.Sin(ang1) * Math.Sin(ang2)
        //             ,
        //             Math.Cos(ang2) + Math.Cos(ang1) * Math.Cos(ang3)
        //         );
        //
        //         double lat3 = Math.Asin(
        //             Math.Sin(lat1)*Math.Cos(dst13)+Math.Cos(lat1)*Math.Sin(dst13)*Math.Cos(firstBearing));
        //
        //
        //         double dLon = Math.Atan2(
        //             Math.Sin(firstBearing) * Math.Sin(dst13) * Math.Cos(lat1)
        //             ,
        //             Math.Cos(dst13) - Math.Sin(lat1) * Math.Sin(lat3)
        //         );
        //
        //
        //         double lon3 = ((lon1 - dLon + Constants.PI) % twoPi) - Constants.PI;
        //
        //         return new Coordinate(lat3, lon3);
        //
        //     }
        //     
        // }
        
        
        // public double CalculateCrossTrackDistance()
        
    }
}