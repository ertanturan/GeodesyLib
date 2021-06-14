using System;

namespace GeodesyLib.Exceptions
{
    public class InfiniteIntersectionException : Exception
    {
        public InfiniteIntersectionException(string message) : base(message)
        {
        }

        public InfiniteIntersectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        
    }
}