namespace GeodesyLib.DataTypes
{
    public class Coordinate
    {
        public double Lat { get; private set; }
        public double Lon { get; private set; }
        public double Alt { get; private set; }

        public Coordinate(double lat, double lon, double alt)
        {
            Lat = lat;
            Lon = lon;
            Alt = alt;
        }
        
    }
}