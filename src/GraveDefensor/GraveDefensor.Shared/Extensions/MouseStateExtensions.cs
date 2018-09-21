using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework.Input
{
    public static class MouseStateExtensions
    {
        public static MouseState OffsetHorizontally(this MouseState mouseState, int offset)
        {
            return new MouseState(mouseState.X + offset, mouseState.Y, mouseState.ScrollWheelValue,
                mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);
        }
        public static Point AsPoint(this MouseState mouseState) => new Point(mouseState.X, mouseState.Y);
    }
}
