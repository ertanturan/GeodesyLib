﻿namespace GeodesyLib.DataTypes
{
    public class Coordinate
    {
        public double Lat { get; }
        public double Lon { get; }

        public Coordinate(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }

        
    }
}