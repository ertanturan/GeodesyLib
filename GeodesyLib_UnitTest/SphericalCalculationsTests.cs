using System;
using GeodesyLib;
using GeodesyLib.DataTypes;
using Moq;
using NUnit.Framework;

namespace GeodesyLib_UnitTest
{
    public class SphericalCalculationsTests
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
        public void HaversineDistance_WhenCalled_ReturnsSphericalDistance()
        {
            //act
            double result = _from.HaversineDistance(_to);

            //assert

            Assert.That(result, Is.EqualTo(404.27916398870167));
        }

        [Test]
        public void CalculateSphericalLawOfCosines_WhenCalled_ReturnsTheExactResult()
        {
            //act
            double result = _from.CalculateSphericalLawOfCosines(_to);

            //assert

            Assert.AreEqual(404.27916398870167, result, 0.00000000001d);
        }

        [Test]
        public void CalculateBearing_WhenCalled_ReturnsBearing()
        {
            //act
            double result = _from.CalculateBearing(_to
            );
            //assert
            Assert.That(result, Is.EqualTo(156.16658258152279));
        }

        [Test]
        public void CalculateMidPoint_WhenCalled_ReturnsMidPoint()
        {
            //act

            Coordinate result = _from.CalculateMidPoint(_to);

            Coordinate expectedResult = new Coordinate(50.53632687827433d,
                1.27461410068055d);

            //assert

            Assert.AreEqual(expectedResult.Latitude, result.Latitude, 
                0.000000001d);
            Assert.AreEqual(expectedResult.Longitude, result.Longitude,
                0.000000001d);
        }


        [Test]
        [TestCase(0, 52.205, 0.119)]
        [TestCase(1, 48.857, 2.351)]
        [TestCase(0.5, 50.53632687827433d, 
            1.2746141006782352d)]
        public void CalculateIntermediatePoint_WhenCalled_ReturnsIntermediateCoordinate(
            double fraction, double expectedLat, double expectedLon)
        {
            //act
            double distance = _from.HaversineDistance(_to);

            Coordinate result = _from.CalculateIntermediatePointByFraction(_to,
                fraction);


            //assert
            Assert.AreEqual(expectedLat, result.Latitude, 0.0000001d);
            Assert.AreEqual(expectedLon, result.Longitude, 0.0000001d);
        }

        [Test]
        [TestCase(100, 12, 53.084266, 0.4302)]
        public void CalculateDestinationPoint_WhenCalled_ReturnsDestinationCoordinate(
            double distance, double bearing,
            double expectedLat, double expectedLon)
        {
            //act

            Coordinate result = _from.CalculateDestinationPoint(distance, bearing);

            //assert
            Assert.AreEqual(expectedLat, result.Latitude, 0.0001d);
            Assert.AreEqual(expectedLon, result.Longitude, 0.0001d);
        }


        [Test]
        public void GetNAmountOfCoordinatesBetween_WhenCalled_ReturnsNAmountOfCoordinates()
        {
            //act

            Coordinate[] result = _from.Get_N_AmountOfCoordinatesBetween(_to, 50);


            Assert.That(result.Length, Is.EqualTo(50));
            Assert.AreEqual(_from.Latitude, result[0].Latitude, 
                0.0000001d);
            Assert.AreEqual(_from.Longitude, result[0].Longitude, 
                0.0000001d);
        }

        [Test]
        public void CalculateEquirectangularApproximation_WhenCalled_ReturnsApproximateResult()
        {
            //arrange
            
            //act
            double result = _from.CalculateEquirectangularApproximation(_to);

            //assert
            Assert.AreEqual(result,404.3,0.1);
        }
    }
}