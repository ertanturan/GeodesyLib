# GeodesyLib


# Usage

<ul>
 
## Import

1. Go to [release](https://github.com/ertanturan/GeodesyLib/releases) page.
2. Download the lates release of the package.
3. Import it to your unity project.

## Documentation

A doxygen output for the documentation of the code is included and will be included at every release.
So, head to the releases page and find the document in the downloads section of the related version.

## Examples

From Coordinate `Coordinate _from = new Coordinate(52.205, 0.119);` <br />
 To Coordinate `Coordinate _to = new Coordinate(48.857, 2.351);`

<ul>
 
### Haversine Distance

`double result = _from.HaversineDistance(_to);`

### Bearing

`double result = _from.CalculateBearing(_to);`

### Spherical Law Of Cosines

`double result = _from.CalculateSphericalLawOfCosines(_to);`

### Calculate Mid Point

`Coordinate result = _from.CalculateMidPoint(_to);`

### Calculate Intermediate Point

` double distance = _from.HaversineDistance(_to);`

` Coordinate result = _from.CalculateIntermediatePointByFraction(_to,fraction);`

 
 </ul>
</ul>
# Courtesy

Transcoded from `JavaScript originals` by *Chris Veness (C) 2005-2019*
and several `C++ classes` by *Charles F.F. Karney (C) 2008-2021 and
published under the same `MIT License`.
