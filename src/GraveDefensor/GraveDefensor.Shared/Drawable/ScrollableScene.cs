using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework.Input;

namespace GraveDefensor.Shared.Drawable
{
    public abstract class ScrollableScene: Scene
    {
        /// <summary>
        /// Keeps track of horizontal offset.
        /// </summary>
        public int Offset { get; private set; }
        public UpdateContext OffsetUpdateContext(UpdateContext context)
        {
            return context.Clone(mouseState: context.MouseState.OffsetHorizontally(Offset));
        }
    }
}
