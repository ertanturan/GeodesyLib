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
        public void RhumbDistance_WhenCalled_ReturnsExactResult()
        {
            //act

            double result = _from.CalculateRhumbDistance(_to);
            
            Assert.AreEqual(404.29,result,0.01);
            
        }
    }
}