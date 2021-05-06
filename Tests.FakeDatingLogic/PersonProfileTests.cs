using System;
using System.Diagnostics;
using System.Linq;
using FakeDatingLogic.Structures;
using Xunit;

namespace Tests.FakeDatingLogic
{
    public class PersonProfileTests
    {
        public PersonProfile personOne;

        private void SetupPersonOne()
        {
            personOne = new PersonProfile
            {
                Email = "james.norman.602@gmail.com",
                GenderIdentity = GenderIdentityEnum.M,
                FullName = "James Norman",
                LatitudeDecimalDegrees =
                    34.063699M, //Using a fixed point from elsewhere in the code, so I didn't have to recalculate results...very time consuming
                LongitudeDecimalDegrees =
                    -118.358781M //Using a fixed point from elsewhere in the code, so I didn't have to recalculate results...very time consuming
            };
        }


        [Fact]
        public void SemiNormalPersonWorks()
        {
            SetupPersonOne();
            var validation = personOne.Validate();
            Assert.False(validation.Any());
        }

        [Fact]
        public void EmptyEmailBad()
        {
            SetupPersonOne();
            personOne.Email = null;
            var validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = string.Empty;
            validation = personOne.Validate();
            Assert.True(validation.Any());
        }

        [Fact]
        public void EmailLengthBadSmall()
        {
            SetupPersonOne();
            personOne.Email = "1"; //Wouldn't pass formatting anyway, but
            var validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = "12"; //Wouldn't pass formatting anyway, but
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = "2 "; //Wouldn't pass formatting anyway, but
            validation = personOne.Validate();
            Assert.True(validation.Any());
        }

        [Fact]
        public void EmailLengthBadBig()
        {
            SetupPersonOne();
            personOne.Email =
                "A23456789012345678901234567890123456789012345678901234567890123456789012345678901234567890@short.com"; //100 characters should pass, but..
            var validation = personOne.Validate();
            Assert.Empty(validation);
            personOne.Email =
                "A23456789012345678901234567890123456789012345678901234567890123456789012345678901234567890@shortr.com"; //101 characters should not.
            validation = personOne.Validate();
            Assert.True(validation.Any());
        }

        [Fact]
        //Emails from https://stackoverflow.com/questions/2049502/what-characters-are-allowed-in-an-email-address
        public void EmailKnownBadExamples()
        {
            SetupPersonOne();
            personOne.Email = "Abc.example.com"; //(no @ character)
            var validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = "A@b@c@example.com"; // (only one @ is allowed outside quotation marks)
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email =
                "a\"b(c)d,e:f;gi[j\\k]l@example.com"; // (none of the special characters in this local part are allowed outside quotation marks)
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email =
                "just\"not\"right@example.com"; // (quoted strings must be dot separated or the only element making up the local part)
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email =
                "this is\"not\\allowed@example.com"; // (spaces, quotes, and backslashes may only exist when within quoted strings and preceded by a backslash)
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email =
                "this\\ still\\\"not\\allowed@example.com"; // (even if escaped (preceded by a backslash), spaces, quotes, and backslashes must still be contained by quotes)
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = "john..doe@example.com"; //  (double dot before @); (with caveat: Gmail lets this through)
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = "john.doe@example..com"; // (double dot after @)
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = " james.norman.602@gmail.com"; // a valid address with a leading space
            validation = personOne.Validate();
            Assert.True(validation.Any());
            personOne.Email = "james.norman.602@gmail.com "; // a valid address with a trailing space
            validation = personOne.Validate();
            Assert.True(validation.Any());
        }


        [Theory]
        [InlineData(33.575779, -112.009036,
            365.48)] //origin office building to my apartment complex 365.48 is straight line, 391 is driving.  We are calculating straight line.
        [InlineData(34.063699, -118.358781, 0)] //origin office building to origin office building
        [InlineData(34.062562, -118.354117,
            .26)] //origin office building to nearest starbucks 5757 Wilshire Blvd UNIT 106 · In SAG-AFTRA .26-.30  This is off by 7% on straight line.
        [InlineData(34.083684, -118.344149,
            1.6)] //origin office building to Pink's Hot Dogs, Sorry the drive is a mile longer. ;)
        [InlineData(34.118219, -118.300293,
            5.11)] //origin office building to Griffith off 1.5% on straight line
        [InlineData(40.748428, -73.985655,
            2452.19)] //Empire State Building to origin office building 2541.19 miles, calculated as 2451.68
        public void DistanceTests(decimal latitude, decimal longitude, decimal expectedResult)
        {
            var PercentageDistanceToleranceAsDecimal = .015M; //1.5%
            var UseMileageToleranceWhenCalculateDistanceIsLessThan = .5M; //Counts as .5 Miles
            SetupPersonOne();
            var result =
                personOne.DistanceFrom(latitude, longitude);
            var expectedValuePercentageOff = expectedResult == 0
                ? result == 0 ? 0 : decimal.MaxValue
                : Math.Round(result / expectedResult * 100, 2) - 100;

            Debug.WriteLine(
                $"Actual result was {result} for input with expected value of {expectedResult}. Percentage off: {expectedValuePercentageOff}");
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
            {
                Assert.True(result <= UseMileageToleranceWhenCalculateDistanceIsLessThan);
            }
        }

        [Theory]
        [InlineData(90.000000001, 0)]
        [InlineData(0, 180.000000001)]
        [InlineData(-90.000000001, 0)]
        [InlineData(0, -180.000000001)]
        public void DistancesOutOfRange(decimal latitude, decimal longitude)
        {
            SetupPersonOne();
            // ReSharper disable once ConvertToLocalFunction
            Action result = () => personOne.DistanceFrom(latitude, longitude);
            Assert.Throws<ArgumentOutOfRangeException>(result);
        }

        [Theory]
        [InlineData(90.00000000, 0)]
        [InlineData(0, 180.00000000)]
        [InlineData(-90.00000000, 0)]
        [InlineData(0, -180.00000000)]
        public void DistancesCloseToRange(decimal latitude, decimal longitude)
        {
            SetupPersonOne();
            // ReSharper disable once ConvertToLocalFunction
            _ = personOne.DistanceFrom(latitude, longitude);
            //here we only care if an exception was thrown.  Otherwise, it passes.
        }
    }
}