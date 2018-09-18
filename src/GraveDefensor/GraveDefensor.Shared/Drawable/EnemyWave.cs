using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using NLog;
using System;
using System.Collections.Generic;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Drawable
{
    public class EnemyWave: Drawable
    {
        static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        Settings.EnemyWave settings;
        Settings.Path path;
        Settings.Enemy enemySettings;
        List<Enemy> enemies;
        Enemy enemyTemplate;
        TimeSpan? relative;
        TimeSpan relativeSpawnTime;
        public bool IsActive { get; set; }
        public void Init(IInitContext context, Settings.EnemyWave settings, Settings.Enemy enemySettings, Settings.Path path)
        {
            this.settings = settings;
            this.path = path;
            this.enemySettings = enemySettings;
            enemies = context.ObjectPool.GetObject<List<Enemy>>();
            enemyTemplate = context.ObjectPool.GetObject<Enemy>();
            logger.Info($"Wave {settings.Id} init");
        }
        public override void InitContent(IInitContentContext context)
        {
            enemyTemplate.InitContent(context);
            base.InitContent(context);
        }
        public override void Update(UpdateContext context)
        {
            if (!IsActive)
            {
                if (!relative.HasValue)
                {
                    relative = TimeSpan.Zero;
                }
                else
                {
                    relative += context.GameTime.ElapsedGameTime;
                    if (relative.Value.TotalMilliseconds > settings.StartTimeOffset)
                    {
                        relativeSpawnTime = TimeSpan.Zero;
                        SpawnEnemy(context);
                        IsActive = true;
                        logger.Info($"Wave {settings.Id} become active");
                        return;
                    }
                } 
            }
            else
            {

            }
            if (IsActive)
            {
                //relativeSpawnTime
                //if (relativeSpawnTime.TotalMilliseconds > settings.Interval)
                //{
                //    relativeSpawnTime = relativeSpawnTime.TotalMilliseconds + settings.Interval
                //}
            }
            foreach (var enemy in enemies)
            {
                enemy.Update(context);
            }
            base.Update(context);
        }
        public override void Draw(IDrawContext context)
        {
            if (IsActive)
            {
                foreach (var enemy in enemies)
                {
                    enemy.Draw(context);
                }
            }
            base.Draw(context);
        }
        public void SpawnEnemy(UpdateContext context)
        {
            var enemy = context.ObjectPool.GetObject<Enemy>();
            enemy.Init(null, enemySettings, path);
            enemy.CopyContentFrom(enemyTemplate);
            enemy.Start();
            enemies.Add(enemy);
            logger.Info($"Enemy spawned for wave {settings.Id}");
        }
        public override void ReleaseResources(IObjectPool objectPool)
        {
            objectPool.ReleaseObject(enemies);
            objectPool.ReleaseObject(enemyTemplate);
            base.ReleaseResources(objectPool);
        }
    }
}
