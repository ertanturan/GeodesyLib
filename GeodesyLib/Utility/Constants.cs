namespace GeodesyLib
{
    /// <summary>
    /// This class contains only constant values that were not meant to change.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Radius of the world in kilometers (KM)
        /// </summary>
        public const double RADIUS = 6371;
        
        /// <summary>
        /// Pi number. Just didn't want to use the one given by the default Math library because it's more precise this way.
        /// </summary>
        public const double PI = 3.141592653589793238462643383279d;
    }
    
}