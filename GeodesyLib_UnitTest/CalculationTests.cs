using System;
using GeodesyLib;
using GeodesyLib.DataTypes;
using NUnit.Framework;

namespace GeodesyLib_UnitTest
{
    public class CalculationTests
    {


        [Test]
        public void HaversineDistance_WhenCalled_ReturnsTheExactResult()
        {
            
            //arrange
            Coordinate from = new Coordinate(52.205, 0.119);
            Coordinate to = new Coordinate(48.857, 2.351);

            //act
            double result= Calculations.HaversineDistance(from, to);
           
            //assert
            
            Assert.That(result,Is.EqualTo(404.27916398870167));

        }

    }
}