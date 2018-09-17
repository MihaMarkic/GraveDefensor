using GraveDefensor.Shared.Drawable;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;
using System;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Test.Drawable
{
    public class EnemyTest: BaseTest<MockEnemy>
    {

        [TestFixture]
        public class Init: EnemyTest
        {
            [Test]
            public void AfterInitIsActiveIsFalse()
            {
                Target.Init(Substitute.For<IInitContext>(),
                new MockEnemySettings
                {
                    Id = "Enemy",
                    Speed = 40
                },
                new Settings.Path
                {
                    Points = new Settings.Point[]
                    {
                            new Settings.Point { X = 0, Y = 0},
                            new Settings.Point { X = 100, Y = 0},
                    }
                });

                Assert.That(Target.IsActive, Is.False);
            }
        }
        [TestFixture]
        public class Start: EnemyTest
        {
            [SetUp]
            public new void SetUp()
            {
                Target.Init(Substitute.For<IInitContext>(),
                    new MockEnemySettings
                    {
                        Id = "Enemy",
                        Speed = 40
                    },
                    new Settings.Path
                    {
                        Points = new Settings.Point[]
                        {
                            new Settings.Point { X = 0, Y = 0},
                            new Settings.Point { X = 100, Y = 0},
                            new Settings.Point { X = 120, Y = 20},
                            new Settings.Point { X = 120, Y = 50}
                        }
                    });
                base.SetUp();
            }
            [Test]
            public void AfterStart_ActiveIsTrue()
            {
                Target.Start();
                
                Assert.That(Target.IsActive, Is.True);
            }
            [Test]
            public void AfterStart_LastPointIsZero()
            {
                Target.Start();

                Assert.That(Target.LastPoint, Is.Zero);
            }
            [Test]
            public void AfterStart_SegmentLengthIsCorrect()
            {
                Target.Start();

                Assert.That(Target.SegmentLength, Is.EqualTo(100));
            }
            [Test]
            public void AfterStart_AngleIsCorrect()
            {
                Target.Start();

                Assert.That(Target.Angle, Is.Zero);
            }
        }
        [TestFixture]
        public class Update: EnemyTest
        {
            [SetUp]
            public new void SetUp()
            {
                Target.Init(Substitute.For<IInitContext>(),
                    new MockEnemySettings
                    {
                        Id = "Enemy",
                        Speed = 1000
                    },
                    new Settings.Path
                    {
                        Points = new Settings.Point[]
                        {
                            new Settings.Point { X = 0, Y = 0},
                            new Settings.Point { X = 100, Y = 0},
                            new Settings.Point { X = 110, Y = 10},
                            new Settings.Point { X = 110, Y = 40}
                        }
                    });
                base.SetUp();
            }
            public static UpdateContext CreateUpdateContext(int elapsed)
            {
                return new UpdateContext(new GameTime(default, TimeSpan.FromMilliseconds(elapsed)), default, null);
            }
            
            [Test]
            public void WhenNotActiveAndUpdate_CenterIsZero()
            {
                Target.Update(CreateUpdateContext(2));

                Assert.That(Target.Center, Is.EqualTo(Vector2.Zero));
            }
            [Test]
            public void WhenHalfWay_CenterIsHalf()
            {
                Target.Start();

                Target.Update(CreateUpdateContext(50));

                Assert.That(Target.Center, Is.EqualTo(new Vector2(50, 0)));
            }
            [Test]
            public void WhenNextSegmentHalfWay_CenterIsHalfOfNextSegment()
            {
                Target.Start();
                Target.Update(CreateUpdateContext(50));

                Target.Update(CreateUpdateContext(50 + 10));

                float pos = 10 / (float)Math.Sqrt(200);

                Assert.That(Target.Center, Is.EqualTo(new Vector2(100 + 10 * pos, 10 * pos)));
            }
            [Test]
            public void WhenOnNextSegment_AngleIsCorrect()
            {
                Target.Start();

                Target.Update(CreateUpdateContext(110));

                float pos = 10 / (float)Math.Sqrt(200);

                Assert.That(Target.Angle, Is.EqualTo((float)(Math.PI / 4)));
            }
        }
    }

    public  class MockEnemy: Enemy
    {

    }

    public class MockEnemySettings: Settings.Enemy
    {

    }
}
