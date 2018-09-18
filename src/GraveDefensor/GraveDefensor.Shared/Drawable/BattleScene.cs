using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Drawable
{
    public class BattleScene: ScrollableScene
    {
        public WeaponPlace[] WeaponPlaces { get; private set; }
        public EnemyWave[] Waves { get; private set; }
        Settings.Battle settings;
        public int Health { get; private set; }
        public int Cash { get; private set; }
        Settings.Size windowSize;
        public void Init(IInitContext context, Settings.Battle settings, Settings.Size windowSize)
        {
            this.settings = settings;
            this.windowSize = windowSize;
            Health = settings.Health;
            Cash = settings.Cash;
            WeaponPlaces = new WeaponPlace[settings.WeaponPlaces.Length];
            for (int i = 0; i < settings.WeaponPlaces.Length; i++)
            {
                var setting = settings.WeaponPlaces[i];
                var wp = context.ObjectPool.GetObject<WeaponPlace>();
                wp.Init(setting);
                WeaponPlaces[i] = wp;
            }
            Waves = new EnemyWave[settings.Waves.Length];
            for (int i=0; i<settings.Waves.Length; i++)
            {
                var setting = settings.Waves[i];
                var wave = context.ObjectPool.GetObject<EnemyWave>();
                var enemySettings = settings.Enemies.Single(e => string.Equals(e.Id, setting.EnemyId, StringComparison.Ordinal));
                var pathSettings = settings.Paths.Single(p => string.Equals(p.Id, setting.PathId, StringComparison.Ordinal));
                wave.Init(context, setting, enemySettings, pathSettings);
                Waves[i] = wave;
            }
        }
        public override void InitContent(IInitContentContext context)
        {
            foreach (var wp in WeaponPlaces)
            {
                wp.InitContent(context);
            }
            foreach (var wave in Waves)
            {
                wave.InitContent(context);
            }
            base.InitContent(context);
        }

        public override void Update(UpdateContext context)
        {
            // converts mouse coordinates to absolute to scene
            var childContext = OffsetUpdateContext(context);
            foreach (var wp in WeaponPlaces)
            {
                wp.Update(context);
            }
            foreach (var wave in Waves)
            {
                wave.Update(context);
            }
            base.Update(context);
        }

        public override void Draw(IDrawContext context)
        {
            foreach (var wp in WeaponPlaces)
            {
                wp.Draw(context);
            }
            foreach (var wave in Waves)
            {
                wave.Draw(context);
            }
            if (Globals.ShowPaths)
            {
                foreach (var path in settings.Paths)
                {
                    DrawPath(context, path);
                }
            }
            DrawHeader(context);
            base.Draw(context);
        }
        void DrawHeader(IDrawContext context)
        {
            const float BetweenColumns = 80;
            const float BetweenRows = 20;
            const float Top = 10;
            const float Left = 10;
            context.FillRectangle(new Rectangle(0, 0, windowSize.Width, 60), new Color(Color.Black, 0.2f));
            context.DrawString(GlobalContent.Default.HudFont, "Health", new Vector2(Left, Top), Color.Yellow);
            context.DrawString(GlobalContent.Default.HudFont, Health.ToString("#,##0"), new Vector2(Left, Top+BetweenRows), Color.Yellow);
            context.DrawString(GlobalContent.Default.HudFont, "Amount", new Vector2(Left+BetweenColumns, Top), Color.Yellow);
            context.DrawString(GlobalContent.Default.HudFont, Cash.ToString("#,##0"), new Vector2(Left + BetweenColumns, Top+BetweenRows), Color.Yellow);

        }
        void DrawPath(IDrawContext context, Settings.Path path)
        {
            for (int i = 1; i < path.Points.Length; i++)
            {
                var prev = path.Points[i - 1];
                var next = path.Points[i];
                context.DrawLine(prev.X, prev.Y, next.X, next.Y, Color.Yellow, 3f);
            }
        }

        public override void ReleaseResources(IObjectPool objectPool)
        {
            objectPool.ReleaseObject(WeaponPlaces);
            base.ReleaseResources(objectPool);
        }
    }
}
