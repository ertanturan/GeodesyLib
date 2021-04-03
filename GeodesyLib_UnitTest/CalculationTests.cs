using System;
using GeodesyLib;
using GeodesyLib.DataTypes;
using Moq;
using NUnit.Framework;

namespace GeodesyLib_UnitTest
{
    public class CalculationTests
    {
        private Coordinate _from;
        private Coordinate _to;

        [SetUp]
        public void Setup()
        {
            _from = new Coordinate(52.205, 0.119);
            _to = new Coordinate(48.857, 2.351);
        }

        [Test]
        public void HaversineDistance_WhenCalled_ReturnsTheExactResult()
        {
            //act
            double result = _from.HaversineDistance(_to);

            //assert

            Assert.That(result, Is.EqualTo(404.27916398870167));
        }

        [Test]
        public void SphericalLawOfCosines_WhenCalled_ReturnsTheExactResult()
        {
            //act
            double result = _from.HaversineDistance(_to);

            //assert

            Assert.That(result, Is.EqualTo(404.27916398870167));
        }

        [Test]
        public void CalculateBearing_WhenCalled_ReturnsTheExactResult()
        {
            //act
            double result = _from.CalculateInitialBearing(_to
            );
            //assert
            Assert.That(result, Is.EqualTo(156.16658258152279));
        }

        [Test]
        public void CalculateMidPoint_WhenCalled_ReturnsTheExactResult()
        {
            //act

            Coordinate result = _from.CalculateMidPoint(_to);

            Coordinate expectedResult = new Coordinate(50.53632687827433d, 1.27461410068055d);

            //assert

            Assert.That(result.Lat, Is.EqualTo(expectedResult.Lat));
            Assert.That(result.Lon, Is.EqualTo(expectedResult.Lon));
        }


        [Test]
        [TestCase(0, 52.205, 0.119)]
        [TestCase(1, 48.857, 2.351)]
        public void CalculateIntermediatePoint_WhenCalled_ReturnsExactResult(double fraction,
            double expectedLat, double expectedLon)
        {
            //act
            double distance = _from.HaversineDistance(_to);

            Coordinate result = _from.CalculateIntermediatePoint(_to,
                distance, fraction);


            //assert
            Assert.AreEqual(expectedLat, result.Lat, 0.0000001d);
            Assert.AreEqual(expectedLon, result.Lon, 0.0000001d);
        }

        [Test]
        [TestCase(100, 12, 53.084266, 0.4302)]
        public void CalculateDestinationPoint_WhenCalled_ReturnsExactResult(double distance,
            double bearing,
            double expectedLat, double expectedLon)
        {
            //act

            Coordinate result = _from.CalculateDestinationPoint(distance, bearing);

            //assert
            Assert.AreEqual(expectedLat, result.Lat, 0.0001d);
            Assert.AreEqual(expectedLon, result.Lon, 0.0001d);
        }

        // `INTERSECTION OF TWO PATHS` IS UNDER DEVELOPMENT...
        //
        // [Test]
        // public void IntersectionOfTwoPaths_WhenCalled_ReturnsExactResult()
        // {
        //     //arrange
        //     double firstBearing = Utility.ConvertToRadian(108.55);
        //     double secondBearing = Utility.ConvertToRadian(32.44);
        //
        //     //act
        //
        //
        //     Coordinate result = Calculations.IntersectionOfTwoPaths(_from, firstBearing,
        //         _to, secondBearing);
        //
        //     //assert
        //
        //
        //     Assert.AreEqual(0,result.Lat,0.0001d);
        //     Assert.AreEqual(0,result.Lon,0.0001d);
        // }
        
    }
}