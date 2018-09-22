using GraveDefensor.Shared.Core;
using NUnit.Framework;
using System.Diagnostics;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Test.Core
{
    public class PathTest
    {
        [TestFixture]
        public class DistanceBetweenPoints : PathTest
        {
            [TestCase(0, 0, 100, 0, ExpectedResult = 100)]
            [TestCase(0, 0, 3, 4, ExpectedResult = 5)]
            [TestCase(0, 0, -3, 4, ExpectedResult = 5)]
            [TestCase(0, 0, 3, -4, ExpectedResult = 5)]
            [TestCase(0, 0, -3, -4, ExpectedResult = 5)]
            public double CalculateDistanceProperly(int ax, int ay, int bx, int by)
            {
                return Path.DistanceBetweenPoints(new Settings.Point { X = ax, Y = ay }, new Settings.Point { X = bx, Y = by });
            }
        }
        [TestFixture]
        public class CalculateSegmentsLengths : PathTest
        {
            [DebuggerStepThrough]
            internal static Settings.Point[] GetTestPath()
            {
                return new Settings.Point[] {
                        new Settings.Point {  X = 0, Y = 0},
                        new Settings.Point { X = 100, Y = 0 },
                        new Settings.Point { X = 103, Y = 4 },
                        new Settings.Point { X = 103, Y = 10 }
                    };
            }
            [Test]
            public void CalculatesAllSegments()
            {
                var actual = Path.CalculateSegmentsLengths(GetTestPath());

                Assert.That(actual.Length, Is.EqualTo(3));
            }
            [Test]
            public void SegmentsAreCorrectlyCalculated()
            {
                var actual = Path.CalculateSegmentsLengths(GetTestPath());

                Assert.That(actual, Is.EqualTo(new double[] { 100, 5, 6 }));
            }
        }
    }
}
