using GeodesyLib;
using NUnit.Framework;

namespace GeodesyLib_UnitTest
{
    /// <summary>
    /// Unit test class for `Utility` class.
    /// Used NUnit.
    /// </summary>
    public class UtilityTests
    {
        [Test]
        [TestCase(0,0)]
        [TestCase(180,3.14159)]
        [TestCase(270,4.71239)]
        [TestCase(360,6.28319)]
        [TestCase(-200,-3.49066)]
        public void ConvertDegreeToRadian_WhenCalled_ReturnsRadian(
            double degree, double expectedResultAsRadian)
        {
            //act
            double result = degree.ConvertDegreeToRadian();
            //assert
            Assert.AreEqual(expectedResultAsRadian,result,0.00001d);
        }

        [Test]
        [TestCase(3.14159,180)]
        [TestCase(4.71239,270)]
        [TestCase(-3.49066,-200)]
        [TestCase(6.28319,360)]
        public void ConvertRadianToDegree_WhenCalled_ReturnsDegree(
            double radian, double expectedResultAsDegree)
        {
            //act
            double result = radian.ConvertRadianToDegree();
            //assert
            Assert.AreEqual(expectedResultAsDegree,result,0.001d);
        }


    
    }
}