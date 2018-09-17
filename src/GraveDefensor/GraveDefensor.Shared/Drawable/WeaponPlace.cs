using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using GraveDefensor.Windows.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraveDefensor.Shared.Drawable
{
    public class WeaponPlace: Drawable
    {
        Texture2D texture;
        public Vector2 Center { get; private set; }
        public Rectangle Bounds { get; private set; }
        public Vector2 Origin { get; private set; }
        public bool IsMouseHovering { get; private set; }
        public ColorTransitionAction MouseHoverColorTransition { get; private set; }

        public void Init(Engine.Settings.WeaponPlace settings)
        {
            Center = new Vector2(settings.Center.X, settings.Center.Y);
            Bounds = new Rectangle((int)Center.X - settings.Size.Width / 2, 
                (int)Center.Y - settings.Size.Height / 2, settings.Size.Width, settings.Size.Height);
            Origin = new Vector2(settings.Size.Width / 2, settings.Size.Height / 2);
        }
        public override void InitContent(IInitContentContext context)
        {
            texture = context.Load<Texture2D>("WeaponPlaceholder");
            MouseHoverColorTransition = Globals.ObjectPool.GetObject<ColorTransitionAction>().WithStartColor(Color.White);
            base.InitContent(context);
        }
        public override void Update(UpdateContext context)
        {
            UpdateMouseHover(context);
            base.Update(context);
        }

        private void UpdateMouseHover(UpdateContext context)
        {
            bool wasHovering = IsMouseHovering;
            IsMouseHovering = Bounds.Contains(context.MousePosition.Value);
            if (wasHovering ^ IsMouseHovering)
            {
                MouseHoverColorTransition.Start(MouseHoverColorTransition.Current, IsMouseHovering ? Color.Red : Color.White, 
                    context.GameTime.TotalGameTime, TimeSpan.FromMilliseconds(200));
            }
            MouseHoverColorTransition.Update(context.GameTime.TotalGameTime);
        }

        public override void Draw(IDrawContext context)
        {
            context.Draw(texture, Center, sourceRectangle:null, MouseHoverColorTransition.Current, 0, Origin, Vector2.One, SpriteEffects.None, 0);
            base.Draw(context);
        }
        public override void ReleaseResources(IObjectPool objectPool)
        {
            objectPool.ReleaseObject(MouseHoverColorTransition);
            base.ReleaseResources(objectPool);
        }
    }
}
