using System;

namespace FakeDatingLogic.Calculation
{
    /// I'm not great with geometry and beyond so the Wikipedia article gave me issues.  I chose rather than be stuck, to do this.  We can discuss
    /// whether this would work for you later in real life.  I used decimal whenever possible because performance between double and decimal is not that
    /// great for the volume we are doing.  To scale, we might consider using double and losing accuracy.
    public static class DistanceMath
    {
        public const decimal PiDecimal = 3.14159265358979323846264338327950288419716939937510M;

        //Adapted from Javascript in https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        //thankfully the Math libraries are identical in the function names.
        public static decimal GetDistanceFromLatLonInMiles(decimal latitudeFirst, decimal longitudeFirst,
            decimal latitudeSecond, decimal longitudeSecond)
        {
            var radiusOfEarthInMiles = 3958; // Radius of the earth in miles, adapted and verified.
            var halfDifferenceOfLatitudeInRadians = DegreesToRadians(latitudeSecond - latitudeFirst) / 2;
            var halfDifferenceOfLongitudeInRadians = DegreesToRadians(longitudeSecond - longitudeFirst) / 2;
            //I'd name a,c better if I knew what they represented.  As I said, the wikipedia formula gave me issues.
            //Unit tests here definitely help (they're needed anyway, this is just an additional reason.)
            var a =
                DecimalMath.Sin(halfDifferenceOfLatitudeInRadians) *
                DecimalMath.Sin(halfDifferenceOfLatitudeInRadians) +
                DecimalMath.Cos(DegreesToRadians(latitudeFirst)) * DecimalMath.Cos(DegreesToRadians(latitudeSecond)) *
                DecimalMath.Sin(halfDifferenceOfLongitudeInRadians) *
                DecimalMath.Sin(halfDifferenceOfLongitudeInRadians);
            var c = 2 * DecimalMath.Atan2(DecimalMath.Sqrt(a), DecimalMath.Sqrt(1 - a));
            var distanceInMiles = radiusOfEarthInMiles * c; // Distance in miles
            return
                Math.Abs(distanceInMiles); //Original function did not account for this.  We only care about distance, not the direction (+/-) of distance.
        }

        public static decimal DegreesToRadians(decimal degrees)
        {
            return degrees * (decimal) (Math.PI / 180);
        }

        //Since these can be used for other things later, I chose not to use absolute values here.
        //And yes, this may be "premature optimization" but for the amount of effort in writing and testing....
        public static decimal MilesToKilometers(decimal miles)
        {
            return miles * (decimal) 1.60934;
        }

        public static decimal KilometersToMiles(decimal kilometers)
        {
            return kilometers * (decimal) .625;
        }

        public static decimal MilesToFeet(decimal miles)
        {
            return miles * 5280;
        }
    }
}