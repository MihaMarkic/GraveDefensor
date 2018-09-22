﻿using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GraveDefensor.Shared.Drawable
{
    public abstract class Dialog: Drawable
    {
        public Point TopLeft { get; private set; }
        public  int Width { get; private set; }
        public Rectangle ContentBounds { get; private set; }
        public int? HeaderHeight { get; private set; }
        public int? FooterHeight { get; private set; }
        public DialogState State { get; private set; }
        public bool WasPressedOutside { get; private set; }
        public void Init(Point topLeft, int width, int contentHeight, int? headerHeight, int? footerHeight)
        {
            TopLeft = topLeft;
            Width = width;
            ContentBounds = new Rectangle(topLeft.X, topLeft.Y + headerHeight ?? 0, width, contentHeight);
            HeaderHeight = headerHeight;
            FooterHeight = footerHeight;
            State = DialogState.Init;
            WasPressedOutside = false;

        }
        public virtual void Update(UpdateContext context, int currentAmount)
        {
            Update(context);
        }
        public override void Update(UpdateContext context)
        {
            switch (State)
            {
                case DialogState.Init:
                    State = DialogState.Open;
                    break;
                case DialogState.Open:
                    if (context.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (!WasPressedOutside && !ContentBounds.Contains(context.MouseState.AsPoint()))
                        {
                            WasPressedOutside = true;
                        }
                    }
                    else if (WasPressedOutside)
                    {
                        State = DialogState.Closing;
                    }
                    break;
            }
            base.Update(context);
        }

        public override void Draw(IDrawContext context)
        {
            context.FillRectangle(ContentBounds, new Color(Color.Green, 0.2f));
            DrawContent(context);
            base.Draw(context);
        }

        internal abstract void DrawContent(IDrawContext context);
    }

    public enum DialogState
    {
        Init,
        Open,
        Closing
    }
}