using System;

namespace GeodesyLib.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = true)]
    public class OnlyBetweenZeroAndOne : Attribute
    {

        public double DoubleValue
        {
            get { return _doubleValue; }

            private set { _doubleValue = value; }

        }

        private double _doubleValue;


        public float FloatValue
        {
            get { return _floatValue; }

            private set { _floatValue = value; }

        }

        private float _floatValue;
        
        
    }
}