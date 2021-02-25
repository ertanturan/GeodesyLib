using System;
using GeodesyLib;
using GeodesyLib.DataTypes;
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
            double result = Calculations.HaversineDistance(_from, _to);

            //assert

            Assert.That(result, Is.EqualTo(404.27916398870167));
        }

        [Test]
        public void SphericalLawOfCosines_WhenCalled_ReturnsTheExactResult()
        {
            //act
            double result = Calculations.HaversineDistance(_from, _to);

            //assert

            Assert.That(result, Is.EqualTo(404.27916398870167));
            
        }

        [Test]
        public void CalculateBearing_WhenCalled_ReturnsTheExactResult()
        {
            //act
            double result = Calculations.CalculateInitialBearing(
             _from,_to
                );
            //assert
            Assert.That(result,Is.EqualTo(156.16658258152279));
            
        }
    }
}