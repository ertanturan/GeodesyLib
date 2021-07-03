using System;

namespace GeodesyLib.Exceptions
{
    /// <summary>
    /// The type of exception
    /// </summary>
    public class InfiniteIntersectionException : Exception
    {
        public InfiniteIntersectionException(string message) : base(message)
        {
        }

     
        
    }
}