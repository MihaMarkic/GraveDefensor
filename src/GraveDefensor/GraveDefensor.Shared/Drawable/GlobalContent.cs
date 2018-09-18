using GraveDefensor.Engine.Core;
using GraveDefensor.Shared.Service.Abstract;
using Microsoft.Xna.Framework.Graphics;

namespace GraveDefensor.Shared.Drawable
{
    public class GlobalContent: DisposableObject
    {
        public static GlobalContent Default { get; private set; }
        public SpriteFont HudFont { get; }
        protected GlobalContent(IInitContentContext context)
        {
            HudFont = context.Load<SpriteFont>("Fonts/Hud");
        }
        public static void Init(IInitContentContext context)
        {
            Default = new GlobalContent(context);
        }
        public static void Unload()
        {
            Default.Dispose();
        }
    }
}
