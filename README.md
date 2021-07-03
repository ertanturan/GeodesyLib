# GeodesyLib


# Usage

<ul>
 
## Import

1. Go to [release](https://github.com/ertanturan/GeodesyLib/releases) page.
2. Download the lates release of the package.
3. Import it to your unity project.

## Code Documentation

A doxygen output for the documentation of the code is included and will be included at every release.
So, head to the releases page and find the document in the downloads section of the related version.
 
## Calculations
All of the calculations used in this library can be found [here](https://www.movable-type.co.uk/scripts/latlong.html).
 
## Examples

From Coordinate `Coordinate _from = new Coordinate(52.205, 0.119);` <br />
To Coordinate `Coordinate _to = new Coordinate(48.857, 2.351);`

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
 double distance = _from.HaversineDistance(_to);
 ```

 ```csharp 
 Coordinate result = _from.CalculateIntermediatePointByFraction(_to,fraction);
 ```
 
### Calculate Destination Point
 
 ```csharp 
 Coordinate result = _from.CalculateDestinationPoint(_from, _to);
 ```
 
### Get N Coordinated Between Two Coordinates
 
 ```csharp 
 Coordinate[] result = _from.GetNCoordinatesBetweenTwoCoordinates(_to, 50);
 ```

### Calculate Equirectangular Approximation
 
 ```csharp 
 double result = _from.CalculateEquirectangularApproximation(_to);
 ```
 

 ### Calculate Intersection Point
 
 ```csharp 
 Coordinate firstCoordinate = new Coordinate(51.8853, 0.2545);
            double firstBearing = 108.55d;

            Coordinate secondCoordinate = new Coordinate(49.0034, 2.5735);
            double secondBearing = 32.44d;

            //act

            Coordinate result = SphericalCalculations.CalculateIntersectionPoint(
                firstCoordinate, firstBearing,
                secondCoordinate, secondBearing);
 
 ```
 
 </ul>
</ul>

# Courtesy

Transcoded from `JavaScript originals` by *Chris Veness (C) 2005-2019*
and several `C++ classes` by *Charles F.F. Karney (C) 2008-2021 and
published under the same `MIT License`.
