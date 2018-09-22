using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GraveDefensor.Shared.Drawable
{
    public abstract class ClickableDrawable : Drawable
    {
        public bool IsEnabled { get; protected set; }
        public ClickState ClickState { get; private set; }
        public Point ClickPosition { get; private set; }
        protected void Init()
        {
            ClickState = ClickState.None;
            IsEnabled = false;
        }
        public override void Update(UpdateContext context)
        {
            if (!IsEnabled)
            {
                ClickState = ClickState.None;
            }
            else
            {
                switch (ClickState)
                {
                    case ClickState.None:
                        if (context.MouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (IsClickWithinBoundaries(context.MouseState))
                            {
                                ClickPosition = new Point(context.MouseState.X, context.MouseState.Y);
                                ClickState = ClickState.Pressed;
                            }
                        }
                        break;
                    case ClickState.Pressed:
                        if (context.MouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (ClickPosition != new Point(context.MouseState.X, context.MouseState.Y))
                            {
                                ClickState = ClickState.Moved;
                            }
                        }
                        else
                        {
                            ClickState = ClickState.Clicked;
                        }
                        break;
                    case ClickState.Clicked:
                        ClickState = ClickState.None;
                        break;
                    case ClickState.Moved:
                        if (context.MouseState.LeftButton == ButtonState.Released)
                        {
                            ClickState = ClickState.None;
                        }
                        break;
                }
            }
            base.Update(context);
        }
        public virtual bool IsClickWithinBoundaries(MouseState state) => false;
    }

    public enum ClickState
    {
        None,
        Pressed,
        Clicked,
        Moved
    }
}
