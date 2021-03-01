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

        [Test]
        public void CalculateMidPoint_WhenCalled_ReturnsTheExactResult()
        {
            //act

            Coordinate result = Calculations.CalculateMidPoint(_from, _to);

            Coordinate expectedResult = new Coordinate( 50.53632687827433d,1.27461410068055d);
            
            //assert
            
            Assert.That(result.Lat,Is.EqualTo(expectedResult.Lat));
            Assert.That(result.Lon,Is.EqualTo(expectedResult.Lon));

        }
        
        
    }
}