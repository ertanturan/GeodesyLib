# GeodesyLib

This library contains the most used geodetic calculations both in spherical and rhumb way.

# Usage

<ul>
 
## Import

1. Go to [release](https://github.com/ertanturan/GeodesyLib/releases) page.
2. Download the lates release of the package.
3. Import it to your unity project.

## Code Documentation

A doxygen output for the documentation of the code is included and will be included at every release.
So, head to the releases page and find the document in the downloads section of the related version.
 
## Examples

From Coordinate 
 ```csharp 
 Coordinate  _from = new Coordinate(52.205, 0.119); 
 ```

To Coordinate 
 ```csharp 
 Coordinate _to = new Coordinate(48.857, 2.351);
 ```

<ul>
 
### Haversine Distance

 ```csharp 
 double result = _from.HaversineDistance(_to);
 ```

### Bearing

 ```csharp 
 double result = _from.CalculateBearing(_to);
 ```

### Spherical Law Of Cosines

 ```csharp 
 double result = _from.CalculateSphericalLawOfCosines(_to);
 ```

### Calculate Mid Point

 ```csharp 
 Coordinate result = _from.CalculateMidPoint(_to);
 ```

### Calculate Intermediate Point

 ```csharp 
 double fraction = 0.88; // Fraction should be between zero and 1 (0-1)
 ```

 ```csharp 
 Coordinate result = _from.CalculateIntermediatePointByFraction(_to,fraction);
 ```
 
### Calculate Destination Point
 
 ```csharp 
Coordinate result = _from.CalculateDestinationPoint(distance, bearing);
 ```
 
### Get N Coordinates Between Two Coordinates
 
 ```csharp 
 Coordinate[] result = _from.GetNCoordinatesBetweenTwoCoordinates(_to, 50);
 ```

### Calculate Equirectangular Approximation
 
 ```csharp 
 double result = _from.CalculateEquirectangularApproximation(_to);
 ```
 

 ### Calculate Intersection Point
 
 ```csharp 
 Coordinate firstCoordinate = new Coordinate(51.8853, 0.2545); // somewhere in UK
            double firstBearing = 108.55d;

            Coordinate secondCoordinate = new Coordinate(49.0034, 2.5735); // somewhere in France
            double secondBearing = 32.44d;

            //act

            Coordinate result = SphericalCalculations.CalculateIntersectionPoint(
                firstCoordinate, firstBearing,
                secondCoordinate, secondBearing);
 
 ```
 
 </ul>
</ul>

# Unit Tests

All the unit tests coded using NUnit framework. Unit test coverage is %100 .

# Courtesy

Transcoded from [JavaScript originals](https://github.com/chrisveness/geodesy) by *Chris Veness (C) 2005-2019* under the `MIT License`.

All of the calculations used in this library can be found [here](https://www.movable-type.co.uk/scripts/latlong.html).

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.


