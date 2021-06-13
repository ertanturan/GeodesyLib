﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using GeodesyLib.DataTypes;
using GeodesyLib.Exceptions;

namespace GeodesyLib
{
    public static class SphericalCalculations
    {
        // This uses the ‘haversine’ formula to calculate the great-circle distance between
        // two points – that is,
        // the shortest distance over the earth’s surface – giving an ‘as-the-crow-flies’
        // distance between the points
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
        ///    This makes the simpler law of cosines a reasonable 1-line alternative
        /// to the haversine formula
        /// for many geodesy purposes (if not for astronomy). 
        /// The choice may be driven by programming language, processor,
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

        public static double CalculateEquirectangularApproximation(
            this Coordinate from, Coordinate to)
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

        public static Coordinate CalculateIntersectionPoint(
            Coordinate point1, double bearing1,
            Coordinate point2, double bearing2)
        {
            double lat1 = point1.Latitude.ConvertDegreeToRadian();
            double lat2 = point2.Latitude.ConvertDegreeToRadian();

            double lon1 = point1.Longitude.ConvertDegreeToRadian();
            double lon2 = point2.Longitude.ConvertDegreeToRadian();

            double rBearing1 = bearing1.ConvertDegreeToRadian();
            double rBearing2 = bearing2.ConvertDegreeToRadian();


            double deltaLat = lat2 - lat1;
            double deltaLon = lon2 - lon1;


            // const δ12 = 2 * Math.asin(Math.sqrt(Math.sin(Δφ/2) * Math.sin(Δφ/2)
            //                                     + Math.cos(φ1) * Math.cos(φ2) * Math.sin(Δλ/2) * Math.sin(Δλ/2)));
            // if (Math.abs(δ12) < Number.EPSILON) return new LatLonSpherical(p1.lat, p1.lon); // coincident points

            double dst12 = 2 * Math.Asin(Math.Sqrt(
                                             Math.Pow(Math.Sin(deltaLat / 2), 2))
                                         + Math.Cos(lat1) * Math.Cos(lat2) *
                                         Math.Pow(Math.Sin(deltaLon / 2), 2)
            );


            // const cosθa = (Math.sin(φ2) - Math.sin(φ1)*Math.cos(δ12)) / (Math.sin(δ12)*Math.cos(φ1));
            // const cosθb = (Math.sin(φ1) - Math.sin(φ2)*Math.cos(δ12)) / (Math.sin(δ12)*Math.cos(φ2));
            // const θa = Math.acos(Math.min(Math.max(cosθa, -1), 1)); // protect against rounding errors
            // const θb = Math.acos(Math.min(Math.max(cosθb, -1), 1)); // protect against rounding errors


            double cosLatA = (Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(dst12)) / (Math.Sin(dst12) * Math.Cos(lat1));
            double cosLatB = (Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(dst12)) / (Math.Sin(dst12) * Math.Cos(lat2));

            double latA = Math.Acos(Math.Min(Math.Max(cosLatA, -1), 1)); // protect against rounding errors
            double latB = Math.Acos(Math.Min(Math.Max(cosLatB, -1), 1)); // protect against rounding errors

            var pi = Constants.PI;


            // const θ12 = Math.sin(λ2-λ1)>0 ? θa : 2*π-θa;
            // const θ21 = Math.sin(λ2-λ1)>0 ? 2*π-θb : θb;

            double crs12 = Math.Sin(deltaLon) > 0 ? latA : 2 * pi - latA;
            double crs21 = Math.Sin(deltaLon) > 0 ? 2 * pi - latB : latB;

            // const α1 = θ13 - θ12; // angle 2-1-3
            // const α2 = θ21 - θ23; // angle 1-2-3

            double ang1 = rBearing1 - crs12;
            double ang2 = crs21 - rBearing2;


            if (Math.Sin(ang1) == 0 && Math.Sin(ang2) == 0)
            {
                throw new InfiniteIntersectionException("Infinity of intersections !");
            }

            if (Math.Sin(ang1) * Math.Sin(ang2) < 0)
            {
                throw new IntersectionAmbiguousException("Intersection ambiguous !");
            }


            ang1 = Math.Abs(ang1);
            ang2 = Math.Abs(ang2);
            
            // const cosα3 = -Math.cos(α1)*Math.cos(α2) + Math.sin(α1)*Math.sin(α2)*Math.cos(δ12);

            double ang3 =
                -Math.Cos(ang1) * Math.Cos(ang2) + Math.Sin(ang1) *
                Math.Sin(ang2) * Math.Cos(dst12);
           
            
            // const δ13 = Math.atan2(Math.sin(δ12)*Math.sin(α1)*Math.sin(α2), Math.cos(α2) + Math.cos(α1)*cosα3);
            
            double dst13 = Math.Atan2(
                Math.Sin(dst12) * Math.Sin(ang1) * Math.Sin(ang2),
                Math.Cos(ang2) + Math.Cos(ang1) * Math.Cos(ang3)
            );

            // const φ3 = Math.asin(
            // Math.min(Math.max(Math.sin(φ1)*Math.cos(δ13) + Math.cos(φ1)*Math.sin(δ13)*Math.cos(θ13), -1), 1)
            // );

            double newLat = Math.Asin(
                Math.Min(
                    Math.Max(Math.Sin(lat1) * Math.Cos(dst13) + Math.Cos(lat1) * Math.Sin(dst13) * Math.Cos(rBearing1),
                        -1), 1)

            );
            
            // const Δλ13 = Math.atan2(Math.sin(θ13)*Math.sin(δ13)*Math.cos(φ1), Math.cos(δ13) - Math.sin(φ1)*Math.sin(φ3));
            // const λ3 = λ1 + Δλ13;

            double dLon = Math.Atan2(
                Math.Sin(rBearing1) * Math.Sin(dst13) * Math.Cos(lat1)
                ,
                Math.Cos(dst13) - Math.Sin(lat1) * Math.Sin(newLat)
            );

            double newLon = lon1 + dLon;


            return new Coordinate(
                newLat.ConvertRadianToDegree(),
                newLon.ConvertRadianToDegree());
        }
    }
}