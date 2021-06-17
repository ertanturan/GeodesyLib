namespace GeodesyLib
{
    /// <summary>
    /// This Class only contains utility conversions and calculations.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Gets a value in `degree` and converts it to radian
        /// </summary>
        /// <param name="degree">Input value type.</param>
        /// <returns>Returns the input value in radians.</returns>
        public static double ConvertDegreeToRadian(this double degree)
        {
            return (Constants.PI / 180) * degree;
        }

        /// <summary>
        /// Gets a value in `radian` and converts it to degree
        /// </summary>
        /// <param name="radian">Input value type</param>
        /// <returns>Returns the input value in degrees.</returns>
        public static double ConvertRadianToDegree(this double radian)
        {
            return radian * (180 / Constants.PI);
        }

       
    }
}