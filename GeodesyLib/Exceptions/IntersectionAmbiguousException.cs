using System;

namespace GeodesyLib.Exceptions
{
    public class IntersectionAmbiguousException : Exception
    {
        public IntersectionAmbiguousException(string message):base(message)
        {
            
        }

        public IntersectionAmbiguousException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}