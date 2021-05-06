using System;
using System.Diagnostics;
using FakeDatingLogic.Calculation;
using Xunit;

//JMN - I did not write this code.  Credit is as below because I wanted more accurate results.  A bit beyond the scope but not costly in time.
// I did adapt the unit tests for XUnit though.
//https://github.com/raminrahimzada/CSharp-Helper-Classes/blob/master/Math/DecimalMath/DecimalMathUnitTests.cs

namespace Tests.FakeDatingLogic
{
    public class DecimalMathUnitTests
    {
        private static readonly decimal epsilon = 0.000000000001M;
        private static readonly int testCount = 1000;
        private static readonly Random Random = new();

        [Fact]
        public void TestMethodExp()
        {
            for (var i = 0; i < testCount; i++)
            {
                var d = Random.NextDouble();
                var d1 = (decimal) d;
                d = Math.Exp(d);
                d1 = DecimalMath.Exp(d1);
                Debug.WriteLine("d=" + d);
                Debug.WriteLine("d1=" + d1);
                Debug.Assert(DecimalMath.Abs((decimal) d - d1) < epsilon);
            }
        }

        [Fact]
        public void TestMethodAsin()
        {
            for (var i = 0; i < testCount; i++)
            {
                var d = Random.NextDouble();
                var d1 = (decimal) d;
                d = Math.Asin(d);
                d1 = DecimalMath.Asin(d1);
                Debug.Assert(DecimalMath.Abs((decimal) d - d1) < epsilon);
            }
        }

        [Fact]
        public void TestMethodAcos()
        {
            for (var i = 0; i < testCount; i++)
            {
                var d = Random.NextDouble();
                var d1 = (decimal) d;
                d = Math.Acos(d);
                d1 = DecimalMath.Acos(d1);
                Debug.Assert(DecimalMath.Abs((decimal) d - d1) < epsilon);
            }
        }

        [Fact]
        public void TestMethodAtan()
        {
            for (var i = 0; i < testCount; i++)
            {
                var d = Random.NextDouble();
                var d1 = (decimal) d;
                d = Math.Atan(d);
                d1 = DecimalMath.ATan(d1);
                Debug.Assert(DecimalMath.Abs((decimal) d - d1) < epsilon);
            }
        }

        [Fact]
        public void TestMethodSin()
        {
            for (var i = 0; i < testCount; i++)
            {
                var d = Random.NextDouble();
                var d1 = (decimal) d;
                d = Math.Sin(d);
                d1 = DecimalMath.Sin(d1);
                Debug.Assert(DecimalMath.Abs((decimal) d - d1) < epsilon);
            }
        }

        [Fact]
        public void TestMethodCos()
        {
            for (var i = 0; i < testCount; i++)
            {
                var d = Random.NextDouble();
                var d1 = (decimal) d;
                d = Math.Cos(d);
                d1 = DecimalMath.Cos(d1);
                Debug.Assert(DecimalMath.Abs((decimal) d - d1) < epsilon);
            }
        }

        [Fact]
        public void TestMethodAtan2()
        {
            for (var i = 0; i < testCount; i++)
            {
                var x = Random.NextDouble();
                var y = Random.NextDouble();
                var dx = (decimal) x;
                var dy = (decimal) y;
                var d = Math.Atan2(y, x);
                var z = DecimalMath.Atan2(dy, dx);
                Debug.Assert(DecimalMath.Abs((decimal) d - z) < epsilon);
            }
        }

        [Fact]
        public void TestMethodPow001()
        {
            double x = 10;
            double y = -5;
            var result = Math.Pow(x, y);

            Assert.Equal(1E-05, result);

            decimal dx = 10;
            decimal dy = -5;
            var dResult = DecimalMath.Power(dx, dy);

            Assert.Equal(0.00001m, dResult);
        }

        [Fact]
        public void TestMethodPow002()
        {
            double x = 10;
            double y = 5;
            var result = Math.Pow(x, y);

            Assert.Equal(100000, result);

            decimal dx = 10;
            decimal dy = 5;
            var dResult = DecimalMath.Power(dx, dy);

            Assert.Equal(100000m, dResult);
        }
    }
}