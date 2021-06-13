using System.Diagnostics.CodeAnalysis;
using GeodesyLib.DataTypes;

namespace GeodesyLib
{
    public static class AdditionalCalculations
    {
        public static double CalculateCrossTrackDistance(this Coordinate predicted, 
            Coordinate actual)
        {
            
            double lat1 = predicted.Latitude.ConvertDegreeToRadian();
            
            
            return lat1;
        }
    }
}