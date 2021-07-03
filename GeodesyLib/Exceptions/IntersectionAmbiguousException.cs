using System;

namespace GeodesyLib.Exceptions
{
    /// <summary>
    /// The type of exception being thrown when the intersection coordinate is ambiguous/can't be determined.
    /// </summary>
    public class IntersectionAmbiguousException : Exception
    {
        public IntersectionAmbiguousException(string message) : base(message)
        {
        }

        public IntersectionAmbiguousException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}