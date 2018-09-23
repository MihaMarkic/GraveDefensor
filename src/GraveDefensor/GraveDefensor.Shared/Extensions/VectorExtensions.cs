using Microsoft.Xna.Framework;

namespace GraveDefensor.Shared.Extensions
{
    public static class VectorExtensions
    {
        public static Point AsPoint(this Vector2 vector) => new Point((int)vector.X, (int)vector.Y);
    }
}
