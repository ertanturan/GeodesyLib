namespace GeodesyLib.DataTypes
{
    
    /// <summary>
    /// A geodetic datum or geodetic system
    /// (also: geodetic reference datum, geodetic reference system,
    /// or geodetic reference frame) is a global datum reference or
    /// reference frame for precisely measuring locations
    /// on Earth or other planetary body.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// In geography, latitude is a geographic coordinate that specifies
        /// the north–south position of a point on the Earth's surface.
        /// Latitude is an angle which ranges from 0° at the Equator to 90° at the poles.
        /// </summary>
        public double Latitude { get; }
        
        /// <summary>
        /// Longitude is a geographic coordinate that specifies the
        /// east–west position of a point on the Earth's surface,
        /// or the surface of a celestial body.
        /// </summary>
        public double Longitude { get; }

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        
    }
}