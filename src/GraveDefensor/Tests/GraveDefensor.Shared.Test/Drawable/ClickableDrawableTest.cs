using GraveDefensor.Shared.Drawable;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using System;

namespace GraveDefensor.Shared.Test.Drawable
{
    public class ClickableDrawableTest: BaseTest<MockClickableDrawable>
    {
        [TestFixture]
        public class Init: ClickableDrawableTest
        {
            [Test]
            public void AfterInit_StateIsNone()
            {
                Target.Init();

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.None));
            }
        }
        [TestFixture]
        public class Update: ClickableDrawableTest
        {
            internal static UpdateContext GetUpdateContext(ButtonState buttonState, int x = 0, int y = 0) => 
                new UpdateContext(new GameTime(),
                    new MouseState(x, y, default, leftButton: buttonState, default, default, default, default),
                 default);

            [SetUp]
            public new void SetUp()
            {
                Target.Init();
            }
            [Test]
            public void WhenButtonIsPressedAndOutsideBounds_ClickStateIsNone()
            {
                Target.Update(GetUpdateContext(ButtonState.Pressed));

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.None));
            }
            [Test]
            public void WhenButtonIsPressedAndOutsideBounds_ClickPositionIsNotStored()
            {
                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 1, y: 2));

                Assert.That(Target.ClickPosition, Is.EqualTo(new Point(0, 0)));
            }
            [Test]
            public void WhenButtonIsPressedAndInsideBounds_ClickStateIsPressed()
            {
                Target.OnIsClickWithinBoundaries = s => true;

                Target.Update(GetUpdateContext(ButtonState.Pressed));

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.Pressed));
            }
            [Test]
            public void WhenButtonIsPressedAndInsideBounds_ClickPositionIsStored()
            {
                Target.OnIsClickWithinBoundaries = s => true;

                Target.Update(GetUpdateContext(ButtonState.Pressed, x:1, y:2));

                Assert.That(Target.ClickPosition, Is.EqualTo(new Point(1, 2)));
            }
            [Test]
            public void WhenMovedWhilePressed_ClickStateIsMoved()
            {
                Target.OnIsClickWithinBoundaries = s => true;
                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 0, y: 0));

                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 1, y: 2));

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.Moved));
            }
            [Test]
            public void WhenNotMovedWhilePressed_ClickStateIsPressed()
            {
                Target.OnIsClickWithinBoundaries = s => true;
                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 1, y: 2));

                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 1, y: 2));

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.Pressed));
            }
            [Test]
            public void WhenButtonIsReleasedAfterPressed_ClickStateIsClicked()
            {
                Target.OnIsClickWithinBoundaries = s => true;
                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 1, y: 2));

                Target.Update(GetUpdateContext(ButtonState.Released, x: 1, y: 2));

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.Clicked));
            }
            [Test]
            public void InNextIterationAfterButtonWasClicked_ClickStateIsNone()
            {
                Target.OnIsClickWithinBoundaries = s => true;
                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 1, y: 2));
                Target.Update(GetUpdateContext(ButtonState.Released, x: 1, y: 2));

                Target.Update(GetUpdateContext(ButtonState.Released));

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.None));
            }
            [Test]
            public void WhenStateIsMovedAndButtonIsReleased_ClickStateIsNone()
            {
                Target.OnIsClickWithinBoundaries = s => true;
                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 1, y: 2));
                Target.Update(GetUpdateContext(ButtonState.Pressed, x: 5, y: 2));

                Target.Update(GetUpdateContext(ButtonState.Released));

                Assert.That(Target.ClickState, Is.EqualTo(ClickState.None));
            }
        }
    }

    public class MockClickableDrawable: ClickableDrawable
    {
        public Func<MouseState, bool> OnIsClickWithinBoundaries;

        public override bool IsClickWithinBoundaries(MouseState state) => OnIsClickWithinBoundaries?.Invoke(state) ?? false;
    }
}
