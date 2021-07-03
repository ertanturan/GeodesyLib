using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using GeodesyLib.DataTypes;
using GeodesyLib.Exceptions;

namespace GeodesyLib
{
    /// <summary>
    /// All these formulas are for calculations on the basis of a spherical earth (ignoring ellipsoidal effects) –
    /// which is accurate enough* for most purposes…
    /// [In fact, the earth is very slightly ellipsoidal; using a spherical model gives errors typically up to 0.3%]
    /// 
    /// Summary taken from : https://www.movable-type.co.uk/scripts/latlong.html
    /// 
    /// </summary>
    public static class SphericalCalculations
    {
        /// <summary>
        ///   This uses the ‘haversine’ formula to calculate the great-circle distance between
        ///   two points – that is,
        ///   the shortest distance over the earth’s surface – giving an ‘as-the-crow-flies’
        ///   distance between the points
        /// (ignoring any hills they fly over, of course!).
        /// </summary>
        /// <param name="from">starting coordinate, starting point of a destination </param>
        /// <param name="to">final coordinate, end point of a destination</param>
        /// <returns>Returns ellipsodial distance between two coordinates with %0.3 error .</returns>
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
        ///    This makes the simpler law of cosines a reasonable 1-line alternative
        /// to the haversine formula
        /// for many geodesy purposes (if not for astronomy). 
        /// The choice may be driven by programMing language, processor,
        /// coding context, available trig
        /// functions (in different languages), etc – and, for very small distances
        /// an equirectangular approximation
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
        ///  This formula is for the initial bearing
        /// (sometimes referred to as forward azimuth) which if followed
        /// in a straight line along a great-circle arc will take you
        /// from the start point to the end point
        /// 
        /// </summary>
        /// <param name="from">Starting point </param>
        /// <param name="to">Final point</param>
        /// <returns>double</returns>
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

        /// <summary>
        /// This is the half-way point along a great circle path between the two points.
        /// </summary>
        /// <param name="from">Starting point</param>
        /// <param name="to">Final point</param>
        /// <returns></returns>
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
        /// <returns>Returns coordinate between two coordinates by user-given fraction </returns>
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

        /// <summary>
        /// Returns an n length array of Coordinates between two coordinates. 
        /// </summary>
        /// <param name="from">Starting point</param>
        /// <param name="to">Final point</param>
        /// <param name="n">Amount of points needed between.</param>
        /// <returns>Coordinate array</returns>
        public static Coordinate[] GetNCoordinatesBetweenTwoCoordinates(this Coordinate from,
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

        /// <summary>
        /// Calculates and returns destination coordinate given starting coordinate,
        /// bearing and distance.
        /// </summary>
        /// <param name="from">Starting point</param>
        /// <param name="distance">End point</param>
        /// <param name="bearing">Direction</param>
        /// <returns>Returns the destination coordinate given starting coordinate,
        /// distance(km) and bearing</returns>
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

        /// <summary>
        /// When precision is less important and performance is more needed this function should be used to calculate distance between two coordinates.
        /// </summary>
        /// <param name="from">Starting point</param>
        /// <param name="to">Final point</param>
        /// <returns>The approximate distance between two coordinates.</returns>
        public static double CalculateEquirectangularApproximation(
            [NotNull] this Coordinate from, [NotNull] Coordinate to)
        {
            double lat1 = from.Latitude.ConvertDegreeToRadian();
            double lat2 = to.Latitude.ConvertDegreeToRadian();

            double lon1 = from.Longitude.ConvertDegreeToRadian();
            double lon2 = to.Longitude.ConvertDegreeToRadian();

            double x = (lon2 - lon1) * Math.Cos((lat1 + lat2) / 2);
            double y = lat2 - lat1;

            double d = Math.Sqrt(x * x + y * y) * Constants.RADIUS;
            return d;
        }

        /// <summary>
        /// This function finds the intersection of two paths with two coordinates and bearings given.
        /// </summary>
        /// <param name="point1">The first coordinate of the first starting point</param>
        /// <param name="bearing1">The bearing/direction of the first starting point</param>
        /// <param name="point2">The second coordinate of the second starting point</param>
        /// <param name="bearing2">The bearing/direction of the second starting point</param>
        /// <returns>Returns Intersection coordinate between two coordinates given bearings</returns>
        /// <exception cref="InfiniteIntersectionException">An error thrown only when there's infinite amount of intersection found.</exception>
        /// <exception cref="IntersectionAmbiguousException">An error thrown only when intersection can not be calculated for sure.</exception>
        public static Coordinate CalculateIntersectionPoint(
            [NotNull] Coordinate point1, double bearing1,
            [NotNull] Coordinate point2, double bearing2)
        {
            double lat1 = point1.Latitude.ConvertDegreeToRadian(),
                lon1 = point1.Longitude.ConvertDegreeToRadian();

            double lat2 = point2.Latitude.ConvertDegreeToRadian(),
                lon2 = point2.Longitude.ConvertDegreeToRadian();

            double teta13 = bearing1.ConvertDegreeToRadian(),
                teta23 = bearing2.ConvertDegreeToRadian();

            double deltaLat = lat2 - lat1, deltaLon = lon2 - lon1;


            // angular distance point1-p2
            double sigma12 = 2 * Math.Asin(Math.Sqrt(Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2)
                                                     + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(deltaLon / 2) *
                                                     Math.Sin(deltaLon / 2)));

            // initial/final bearings between points
            double cosTetaA = (Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(sigma12)) /
                              (Math.Sin(sigma12) * Math.Cos(lat1));

            double cosTetaB = (Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(sigma12)) /
                              (Math.Sin(sigma12) * Math.Cos(lat2));

            double tetaA = Math.Acos(Math.Min(Math.Max(cosTetaA, -1), 1)); // protect against rounding errors
            double tetaB = Math.Acos(Math.Min(Math.Max(cosTetaB, -1), 1)); // protect against rounding errors

            double teta12 = Math.Sin(lon2 - lon1) > 0 ? tetaA : 2 * Constants.PI - tetaA;
            double teta21 = Math.Sin(lon2 - lon1) > 0 ? 2 * Constants.PI - tetaB : tetaB;

            double angle1 = teta13 - teta12; // angle 2-1-3
            double angle2 = teta21 - teta23; // angle 1-2-3

            if (Math.Sin(angle1) == 0 && Math.Sin(angle2) == 0)
            {
                throw new InfiniteIntersectionException("Infinite Intersections !"); // infinite intersections
            }

            if (Math.Sin(angle1) * Math.Sin(angle2) < 0)
            {
                throw new IntersectionAmbiguousException(
                    "Intersection Ambiguous !"); // ambiguous intersection (antipodal?)
            }

            double cosAngle3 = -Math.Cos(angle1) * Math.Cos(angle2) + Math.Sin(angle1) *
                Math.Sin(angle2) * Math.Cos(sigma12);

            double sigma13 = Math.Atan2(Math.Sin(sigma12) * Math.Sin(angle1) * Math.Sin(angle2),
                Math.Cos(angle2) + Math.Cos(angle1) * cosAngle3);

            double lat3 =
                Math.Asin(Math.Min(
                    Math.Max(Math.Sin(lat1) * Math.Cos(sigma13) +
                             Math.Cos(lat1) * Math.Sin(sigma13) * Math.Cos(teta13), -1), 1));

            double deltaLon13 = Math.Atan2(Math.Sin(teta13) * Math.Sin(sigma13) * Math.Cos(lat1),
                Math.Cos(sigma13) - Math.Sin(lat1) * Math.Sin(lat3));
            double newLon = lon1 + deltaLon13;
            newLon = newLon.ConvertRadianToDegree();

            double newLat = lat3.ConvertRadianToDegree();

            return new Coordinate(newLat, newLon);
        }
        
    }
}