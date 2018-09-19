using GraveDefensor.Shared.Messages;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Righthand.MessageBus;
using System;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Drawable.Enemies
{
    public abstract class Enemy: Drawable
    {
        public static TimeSpan VisibleBeforeDone { get; } = TimeSpan.FromSeconds(1);
        Texture2D texture;
        public Vector2 Center { get; private set; }
        public Vector2 OffsetToCenter { get; private set; }
        public float Angle { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int LastPoint { get; set; }
        public double Traversed { get; private set; }
        public double SegmentLength { get; set; }
        public EnemyStatus Status { get; private set; }
        public int Health { get; private set; }
        public TimeSpan FinishedStatusSpan { get; private set; }
        IDispatcher dispatcher;
        Settings.Enemy settings;
        Settings.Path path;
        Vector2[] pathPoints;
        public void Init(IInitContext context, Settings.Enemy settings, Settings.Path path)
        {
            this.settings = settings;
            this.path = path;
            dispatcher = context.Dispatcher;
            pathPoints = new Vector2[path.Points.Length];
            for (int i = 0; i < path.Points.Length; i++)
            {
                pathPoints[i] = new Vector2(path.Points[i].X, path.Points[i].Y);
            }
            Center = Vector2.Zero;
            Status = EnemyStatus.Ready;
            Health = settings.Health;
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
            SegmentLength = GetSegmentLength(LastPoint, pathPoints);
            Angle = GetAngleBetweenVectors(pathPoints[0], pathPoints[1]);
            Status = EnemyStatus.Walking;
            FinishedStatusSpan = TimeSpan.FromSeconds(1);
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
            switch (Status)
            {
                case EnemyStatus.Walking:
                    double distance = settings.Speed * context.GameTime.ElapsedGameTime.TotalSeconds;
                    if (Traversed + distance < SegmentLength)
                    {
                        Traversed += distance;
                    }
                    else
                    {
                        // continue to next line segment
                        if (LastPoint < pathPoints.Length - 2)
                        {
                            LastPoint++;
                            Traversed = Traversed + distance - SegmentLength;
                            SegmentLength = GetSegmentLength(LastPoint, pathPoints);
                            Angle = GetAngleBetweenVectors(pathPoints[LastPoint], pathPoints[LastPoint + 1]);
                        }
                        // finished path
                        else
                        {
                            TransitionToFinished();
                        }
                    }
                    Center = Vector2.Lerp(pathPoints[LastPoint], pathPoints[LastPoint + 1], (float)(Traversed / SegmentLength));
                    break;
                case EnemyStatus.Finished:
                    FinishedStatusSpan -= context.GameTime.ElapsedGameTime;
                    if (FinishedStatusSpan < TimeSpan.Zero)
                    {
                        TransitionToDone();
                    }
                    break;
            }
            base.Update(context);
        }
        internal void TransitionToFinished()
        {
            Center = pathPoints[pathPoints.Length - 1];
            Status = EnemyStatus.Finished;
            dispatcher.Dispatch(new ChangeStatusMessage(0, -Health));
        }
        internal void TransitionToDone()
        {
            Status = EnemyStatus.Done;
        }
        public override void Draw(IDrawContext context)
        {
            if (IsVisible)
            {
                context.Draw(texture, Center, null, Color.White, Angle, OffsetToCenter, new Vector2(1, 1), SpriteEffects.None, 0);
                DrawHealth(context);
            }
            base.Draw(context);
        }
        void DrawHealth(IDrawContext context)
        {
            const float width = 40;
            const float height = 10;
            const float verticalOffset = 5;
            var topLeft = Center - OffsetToCenter + new Vector2(0, -height - verticalOffset);
            if (Health == settings.Health)
            {
                context.FillRectangle(topLeft, new Vector2(width, height), Color.Red);
            }
            else
            {
                float percent = (float)Health / settings.Health;
                float healthWidth = width * percent;
                context.FillRectangle(topLeft, new Vector2(healthWidth, height), Color.Red);
                var blackTopLeft = topLeft + new Vector2(healthWidth, 0);
                context.FillRectangle(blackTopLeft, new Vector2(width - healthWidth, height), Color.Black);
            }
        }

        public void CopyContentFrom(Enemy enemy)
        {
            texture = enemy.texture;
            OffsetToCenter = enemy.OffsetToCenter;
        }
        public bool IsDone => Status == EnemyStatus.Done;
        public bool IsVisible => Status != EnemyStatus.Ready && Status != EnemyStatus.Done;
    }

    public enum EnemyStatus
    {
        Ready,
        Walking,
        Killed,
        Frozen,
        Finished,
        Done
    }
}
