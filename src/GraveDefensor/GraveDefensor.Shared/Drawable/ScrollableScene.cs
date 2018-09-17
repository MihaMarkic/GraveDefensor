using GraveDefensor.Shared.Services.Implementation;

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
            return context.MousePosition.HasValue ? context.CreateHorizontalOffset(Offset) : context;
        }
    }
}
