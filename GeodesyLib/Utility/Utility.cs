namespace GeodesyLib
{
    //This Class includes utility conversions and calculations.
    public static class Utility
    {
        //Gets a value in `degree` and converts it to radian
        public static double ConvertDegreeToRadian(this double degree)
        {
            return (Constants.PI / 180) * degree;
        }

        //Gets a value in `radian` and converts it to degree
        public static double ConvertRadianToDegree(this double radian)
        {
            return radian * (180 / Constants.PI);
        }
    }
}