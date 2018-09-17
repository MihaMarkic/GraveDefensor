using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Drawable
{
    public abstract class Enemy: Drawable
    {
        Texture2D texture;
        public Vector2 Center { get; private set; }
        public Vector2 OffsetToCenter { get; private set; }
        public float Angle { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int LastPoint { get; set; }
        public double Traversed { get; private set; }
        public double SegmentLength { get; set; }
        public bool IsActive { get; private set; }
        Settings.Enemy settings;
        Settings.Path path;
        Vector2[] pathPoints;
        public void Init(IInitContext context, Settings.Enemy settings, Settings.Path path)
        {
            this.settings = settings;
            this.path = path;
            pathPoints = new Vector2[path.Points.Length];
            for (int i = 0; i < path.Points.Length; i++)
            {
                pathPoints[i] = new Vector2(path.Points[i].X, path.Points[i].Y);
            }
            IsActive = false;
            Center = Vector2.Zero;
        }
        public override void InitContent(IInitContentContext context)
        {
            texture = context.Load<Texture2D>(@"Enemies\Walker");
            OffsetToCenter = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            base.InitContent(context);
        }
        public void Start()
        {
            LastPoint = 0;
            IsActive = true;
            SegmentLength = GetSegmentLength(LastPoint, pathPoints);
            Angle = GetAngleBetweenVectors(pathPoints[0], pathPoints[1]);
        }
        public static double GetSegmentLength(int lastPoint, Vector2[] points)
        {
            return Math.Abs(Vector2.Distance(points[lastPoint], points[lastPoint+1]));
        }
        public static float GetAngleBetweenVectors(Vector2 first, Vector2 second)
        {
            return (float)Math.Atan2(second.Y - first.Y, second.X - first.X);
        }
        public override void Update(UpdateContext context)
        {
            if (IsActive)
            {
                double distance = settings.Speed * context.GameTime.ElapsedGameTime.TotalSeconds;
                if (Traversed + distance < SegmentLength)
                {
                    Traversed += distance;
                }
                else
                {
                    // continue to next line segment
                    if (LastPoint < pathPoints.Length - 1)
                    {
                        LastPoint++;
                        Traversed = Traversed + distance - SegmentLength;
                        SegmentLength = GetSegmentLength(LastPoint, pathPoints);
                        Angle = GetAngleBetweenVectors(pathPoints[LastPoint], pathPoints[LastPoint+1]);
                    }
                    // finished path
                    else
                    {
                        Center = pathPoints[pathPoints.Length - 1];
                    }
                }
                Center = Vector2.Lerp(pathPoints[LastPoint], pathPoints[LastPoint + 1], (float)(Traversed / SegmentLength));
            }
            base.Update(context);
        }
        public override void Draw(IDrawContext context)
        {
            if (IsActive)
            {
                context.Draw(texture, Center, null, Color.White, Angle, OffsetToCenter, new Vector2(1, 1), SpriteEffects.None, 0);
            }
            base.Draw(context);
        }

        public void CopyContentFrom(Enemy enemy)
        {
            texture = enemy.texture;
            OffsetToCenter = enemy.OffsetToCenter;
        }
    }
}
