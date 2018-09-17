using GraveDefensor.Engine.Services.Abstract;
using Microsoft.Xna.Framework;

namespace GraveDefensor.Shared.Services.Implementation
{
    public readonly struct UpdateContext
    {
        public GameTime GameTime { get; }
        /// <summary>
        /// Mouse position on scene. Important when scene is scrollable.
        /// </summary>
        public Vector2? MousePosition { get; }
        public IObjectPool ObjectPool { get; }
        public UpdateContext(GameTime gameTime, Vector2? mousePosition, IObjectPool objectPool)
        {
            GameTime = gameTime;
            MousePosition = mousePosition;
            ObjectPool = objectPool;
        }
        public UpdateContext CreateHorizontalOffset(int offset)
        {
            return new UpdateContext(GameTime, new Vector2(MousePosition.Value.X + offset, MousePosition.Value.Y), ObjectPool);
        }
    }
}
