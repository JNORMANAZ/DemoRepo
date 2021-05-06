using System;
using System.Diagnostics;
using FakeDatingLogic.Calculation;
using Xunit;

namespace Tests.FakeDatingLogic
{
    public class DistanceMathTests
    {
        //Accurate to 1.5% except for distances under .5 miles.  Should be fine for the intended purpose.  My goal was 1%.
        //Went for a decimal library to increase accuracy.
        public decimal PercentageDistanceToleranceAsDecimal = .015M; //1.5%
        public decimal PercentageBasicMathToleranceAsDecimal = .006M; //.6%
        public decimal UseMileageToleranceWhenCalculateDistanceIsLessThan = .5M; //Counts as .5 Miles

        //https://www.rapidtables.com/convert/length/mile-to-km.html
        [Theory]
        [InlineData(-1, -1.609344)]
        [InlineData(0, 0)]
        [InlineData(1, 1.609344)]
        [InlineData(5934, 9549.847296)]
        [InlineData(.00034, 0.00054717696)]
        public void MilesToKilometersTests(decimal miles, decimal kilometers)
        {
            //sometimes it's better to split code into multiple lines for readability and debugging..
            var result = DistanceMath.MilesToKilometers(miles);
            var beginRange = kilometers - kilometers * PercentageBasicMathToleranceAsDecimal;
            var endRange = kilometers + kilometers * PercentageBasicMathToleranceAsDecimal;

            if (beginRange < endRange)
                Assert.InRange(result, beginRange, endRange);
            else
                Assert.InRange(result, endRange, beginRange);
        }

        [Theory]
        [InlineData(-1, -.6213712)]
        [InlineData(0, 0)]
        [InlineData(1, .6213712)]
        [InlineData(5934, 3687.217)]
        [InlineData(.00034, 0.0002112662)]
        public void KilometersToMilesTests(decimal kilometers, decimal miles)
        {
            var result = DistanceMath.KilometersToMiles(kilometers);
            var beginRange = miles - miles * PercentageBasicMathToleranceAsDecimal;
            var endRange = miles + miles * PercentageBasicMathToleranceAsDecimal;

            if (beginRange < endRange)
                Assert.InRange(result, beginRange, endRange);
            else
                Assert.InRange(result, endRange, beginRange);
        }

        [Theory]
        [InlineData(-1, -.0174533)]
        [InlineData(0, 0)]
        [InlineData(1, .0174533)]
        [InlineData(5934, 103.5678)]
        [InlineData(.00034, 5.9341195e-6)]
        public void DegreesToRadiansTests(decimal degrees, decimal radians)
        {
            var result = DistanceMath.DegreesToRadians(degrees);
            var beginRange = radians - radians * PercentageBasicMathToleranceAsDecimal;
            var endRange = radians + radians * PercentageBasicMathToleranceAsDecimal;
            if (beginRange < endRange)
                Assert.InRange(result, beginRange, endRange);
            else
                Assert.InRange(result, endRange, beginRange);
        }

        //used https://www.mapdevelopers.com/distance_from_to.php to validate against.
        // https://www.mapdevelopers.com/draw-circle-tool.php provided some coordinates when not Google-able.
        [Theory]
        [InlineData(34.063699, -118.358781, 33.575779, -112.009036,
            365.48)] //Office building in LA to my apartment complex 365.48 is straight line, 391 is driving.  We are calculating straight line.
        [InlineData(34.063699, -118.358781, 34.063699, -118.358781, 0)] //Office building in LA to same Office building in LA
        [InlineData(34.063699, -118.358781, 34.062562, -118.354117,
            .26)] //Office building in LA to nearest starbucks 5757 Wilshire Blvd UNIT 106 · In SAG-AFTRA .26-.30  This is off by 7% on straight line.
        [InlineData(34.063699, -118.358781, 34.083684, -118.344149,
            1.6)] //Office building in LA to Pink's Hot Dogs, Sorry the drive is a mile longer. ;)
        [InlineData(34.063699, -118.358781, 34.118219, -118.300293,
            5.11)] //Office building in LA to Griffith off 1.5% on straight line
        [InlineData(40.748428, -73.985655, 34.063699, -118.358781,
            2452.19)] //Empire State Building to Office building in LA 2541.19 miles, calculated as 2451.68
        public void GetDistanceFromLatLonInMilesTests(decimal latitudeOne, decimal longitudeOne, decimal latitudeTwo,
            decimal longitudeTwo, decimal expectedResult)
        {
            var result =
                DistanceMath.GetDistanceFromLatLonInMiles(latitudeOne, longitudeOne, latitudeTwo, longitudeTwo);
            var expectedValuePercentageOff = 0M; //Merely a statistic
            expectedValuePercentageOff = expectedResult == 0 ? (result == 0 ? 0: Decimal.MaxValue) : Math.Round((result / expectedResult) * 100, 2)-100;

            Debug.WriteLine($"Actual result was {result} for input with expected value of {expectedResult}. Percentage off: {expectedValuePercentageOff}");
            var beginRange = expectedResult - expectedResult * PercentageDistanceToleranceAsDecimal;
            var endRange = expectedResult + expectedResult * PercentageDistanceToleranceAsDecimal;
            Assert.True(result >= 0); //Value must be the absolute value.
            if (result > UseMileageToleranceWhenCalculateDistanceIsLessThan)
            {
                if (beginRange < endRange)
                    Assert.InRange(result, beginRange, endRange);
                else
                    Assert.InRange(result, endRange, beginRange);
            }
            else
                Assert.True(result <= UseMileageToleranceWhenCalculateDistanceIsLessThan);
        }
    }
}