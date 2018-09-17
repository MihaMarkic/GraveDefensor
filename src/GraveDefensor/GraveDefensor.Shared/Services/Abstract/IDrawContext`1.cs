using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraveDefensor.Shared.Service.Abstract
{
    public interface IDrawContext
    {
        void Draw(Texture2D texture, Vector2 position, Color color);
        void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color,
            float rotation, Vector2 origin, SpriteEffects effects, float layerDepth);
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
        void DrawLine(float x1, float y1, float x2, float y2, Color color);
        void DrawLine(float x1, float y1, float x2, float y2, Color color, float thickness);
        void DrawLine(Vector2 point1, Vector2 point2, Color color);
        void DrawLine(Vector2 point1, Vector2 point2, Color color, float thickness);
        void DrawLine(Vector2 point, float length, float angle, Color color);
        void DrawLine(Vector2 point, float length, float angle, Color color, float thickness);
    }
}
