using System;

namespace GeodesyLib.Exceptions
{
    /// <summary>
    /// The type of exception being thrown when infinite intersection
    /// occurs while trying to find intersection coordinate.
    /// </summary>
    public class InfiniteIntersectionException : Exception
    {
        public InfiniteIntersectionException(string message) : base(message)
        {
        }
    }
}