using GraveDefensor.Engine.Services.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Righthand.Immutable;

namespace GraveDefensor.Shared.Services.Implementation
{
    public readonly struct UpdateContext
    {
        public GameTime GameTime { get; }
        public MouseState MouseState { get; }
        public IObjectPool ObjectPool { get; }

        public UpdateContext(GameTime gameTime, MouseState mouseState, IObjectPool objectPool)
        {
            GameTime = gameTime;
            MouseState = mouseState;
            ObjectPool = objectPool;
        }

        public UpdateContext Clone(Param<GameTime>? gameTime = null, Param<MouseState>? mouseState = null, Param<IObjectPool>? objectPool = null)
        {
            return new UpdateContext(gameTime.HasValue ? gameTime.Value.Value : GameTime,
				mouseState.HasValue ? mouseState.Value.Value : MouseState,
				objectPool.HasValue ? objectPool.Value.Value : ObjectPool);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (UpdateContext)obj;
            return Equals(GameTime, o.GameTime) && Equals(MouseState, o.MouseState) && Equals(ObjectPool, o.ObjectPool);}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = base.GetHashCode();
				hash = hash * 37 + (GameTime != null ? GameTime.GetHashCode() : 0);
				hash = hash * 37 + MouseState.GetHashCode();
				hash = hash * 37 + (ObjectPool != null ? ObjectPool.GetHashCode() : 0);
				return hash;
			}
        }
    }
}
