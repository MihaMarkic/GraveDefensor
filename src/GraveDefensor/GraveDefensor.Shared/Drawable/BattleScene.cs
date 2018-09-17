using GraveDefensor.Engine.Services.Abstract;
using Settings = GraveDefensor.Engine.Settings;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraveDefensor.Shared.Drawable
{
    public class BattleScene: ScrollableScene
    {
        WeaponPlace[] weaponPlaces;
        EnemyWave[] waves;
        Settings.Battle settings;
        public void Init(IInitContext context, Settings.Battle settings)
        {
            this.settings = settings;
            
            weaponPlaces = new WeaponPlace[settings.WeaponPlaces.Length];
            for (int i = 0; i < settings.WeaponPlaces.Length; i++)
            {
                var setting = settings.WeaponPlaces[i];
                var wp = context.ObjectPool.GetObject<WeaponPlace>();
                wp.Init(setting);
                weaponPlaces[i] = wp;
            }
            waves = new EnemyWave[settings.Waves.Length];
            for (int i=0; i<settings.Waves.Length; i++)
            {
                var setting = settings.Waves[i];
                var wave = context.ObjectPool.GetObject<EnemyWave>();
                var enemySettings = settings.Enemies.Single(e => string.Equals(e.Id, setting.EnemyId, StringComparison.Ordinal));
                var pathSettings = settings.Paths.Single(p => string.Equals(p.Id, setting.PathId, StringComparison.Ordinal));
                wave.Init(context, setting, enemySettings, pathSettings);
                waves[i] = wave;
            }
        }
        public override void InitContent(IInitContentContext context)
        {
            foreach (var wp in weaponPlaces)
            {
                wp.InitContent(context);
            }
            foreach (var wave in waves)
            {
                wave.InitContent(context);
            }
            base.InitContent(context);
        }

        public override void Update(UpdateContext context)
        {
            // converts mouse coordinates to absolute to scene
            var childContext = OffsetUpdateContext(context);
            foreach (var wp in weaponPlaces)
            {
                wp.Update(context);
            }
            foreach (var wave in waves)
            {
                wave.Update(context);
            }
            base.Update(context);
        }

        public override void Draw(IDrawContext context)
        {
            foreach (var wp in weaponPlaces)
            {
                wp.Draw(context);
            }
            foreach (var wave in waves)
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
            base.Draw(context);
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
            objectPool.ReleaseObject(weaponPlaces);
            base.ReleaseResources(objectPool);
        }
    }
}
