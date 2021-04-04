using GeodesyLib;
using GeodesyLib.DataTypes;
using NUnit.Framework;

namespace GeodesyLib_UnitTest
{
    public class RhumbCalculationTests
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
        public void CalculateRhumbDistance_WhenCalled_ReturnsExactResult()
        {
            //act

            double result = _from.CalculateRhumbDistance(_to);

            //assert
            Assert.AreEqual(404.29, result, 0.01);
        }

        [Test]
        public void CalculateRhumbInitialBearing_WhenCalled_ReturnsExactResult()
        {
            //act

            double result = _from.CalculateRhumbInitialBearing(_to);
            
            //assert
            
            Assert.AreEqual(157,result,0.1d);
            
        }

        [Test]
        [TestCase(100, 12, 53.084266, 0.4302)]
        public void CalculateRhumbDestination_WhenCalled_ReturnsExactResult(double distance,
            double bearing, double expectedLat , double expectedLon)
        {
            //act
            Coordinate result =  _from.CalculateRhumbDestination(distance,bearing);
            
            //assert
            Assert.AreEqual(expectedLat, result.Lat, 0.01d);
            Assert.AreEqual(expectedLon, result.Lon, 0.01d);
        }
        
    }
}