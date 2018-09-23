﻿using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Drawable.Weapons;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using GraveDefensor.Windows.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GraveDefensor.Shared.Drawable
{
    public class WeaponPod: ClickableDrawable
    {
        Texture2D texture;
        public Vector2 Center { get; private set; }
        public Rectangle Bounds { get; private set; }
        public Vector2 Origin { get; private set; }
        public bool IsMouseHovering { get; private set; }
        public ColorTransitionAction MouseHoverColorTransition { get; private set; }
        /// <summary>
        /// Weapon is assigned based to weapon picker.
        /// </summary>
        public IWeapon Weapon { get; set; }

        public void Init(Engine.Settings.WeaponPod settings)
        {
            Init();
            Center = new Vector2(settings.Center.X, settings.Center.Y);
            //Center = Vector2.Zero;
            Bounds = new Rectangle((int)Center.X - settings.Size.Width / 2, 
                (int)Center.Y - settings.Size.Height / 2, settings.Size.Width, settings.Size.Height);
            Origin = new Vector2(settings.Size.Width / 2, settings.Size.Height / 2);
            IsEnabled = true;
        }
        public override void InitContent(IInitContentContext context)
        {
            texture = context.Load<Texture2D>("WeaponPlaceholder");
            MouseHoverColorTransition = Globals.ObjectPool.GetObject<ColorTransitionAction>().WithStartColor(Color.White);
            base.InitContent(context);
        }
        public void Update(UpdateContext context, EnemyWave[] waves)
        {
            UpdateMouseHover(context);
            Weapon?.Update(context, waves);
            base.Update(context);
        }
        public override bool IsClickWithinBoundaries(MouseState state)
        {
            return Bounds.Contains(state.AsPoint());
        }
        private void UpdateMouseHover(UpdateContext context)
        {
            bool wasHovering = IsMouseHovering;
            IsMouseHovering = Bounds.Contains(new Point(context.MouseState.X, context.MouseState.Y));
            if (wasHovering ^ IsMouseHovering)
            {
                MouseHoverColorTransition.Start(MouseHoverColorTransition.Current, IsMouseHovering ? Color.Red : Color.White, 
                    context.GameTime.TotalGameTime, TimeSpan.FromMilliseconds(200));
            }
            MouseHoverColorTransition.Update(context.GameTime.TotalGameTime);
        }

        public override void Draw(IDrawContext context)
        {
            context.Draw(texture, Center, sourceRectangle: null, MouseHoverColorTransition.Current, 0, Origin, Vector2.One, SpriteEffects.None, 0);
            if (Globals.ShowMouseCoordinates)
            {
                context.DrawString(GlobalContent.Default.CoordinatesFont, $"{Bounds.Left}:{Bounds.Top}-{Bounds.Right}:{Bounds.Bottom}",
                    new Vector2(Bounds.Left, Bounds.Top - 20), Color.Yellow);
            }
            Weapon?.Draw(context);
            base.Draw(context);
        }
        public override void ReleaseResources(IObjectPool objectPool)
        {
            objectPool.ReleaseObject(Weapon);
            objectPool.ReleaseObject(MouseHoverColorTransition);
            base.ReleaseResources(objectPool);
        }
    }
}
