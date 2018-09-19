﻿using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Drawable.Enemies;
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
        IInitContext initContext;
        Settings.EnemyWave settings;
        Settings.Path path;
        Settings.Enemy enemySettings;
        List<Enemy> enemies;
        List<Enemy> completedEnemies;
        Enemy enemyTemplate;
        internal TimeSpan Relative { get; private set; }
        internal TimeSpan RelativeSpawnTime { get; private set; }
        public IReadOnlyList<Enemy> Enemies => enemies;
        public IReadOnlyList<Enemy> CompletedEnemies => completedEnemies;
        public EnemyWaveStatus Status { get; private set; }
        public void Init(IInitContext context, Settings.EnemyWave settings, Settings.Enemy enemySettings, Settings.Path path)
        {
            initContext = context;
            this.settings = settings;
            this.path = path;
            this.enemySettings = enemySettings;
            enemies = context.ObjectPool.GetObject<List<Enemy>>();
            completedEnemies = context.ObjectPool.GetObject<List<Enemy>>();
            enemyTemplate = EnemyFactory.GetEnemyFromPool(context.ObjectPool, enemySettings);
            logger.Info($"Wave {settings.Id} init");
            Relative = TimeSpan.FromMilliseconds(settings.StartTimeOffset);
            Status = EnemyWaveStatus.Ready;
        }
        public override void InitContent(IInitContentContext context)
        {
            enemyTemplate.InitContent(context);
            base.InitContent(context);
        }
        public override void Update(UpdateContext context)
        {
            switch (Status)
            {
                case EnemyWaveStatus.Ready:
                    Relative -= context.GameTime.ElapsedGameTime;
                    if (Relative < TimeSpan.Zero)
                    {
                        TransitionToSpawning(context);
                    }
                    break;
                case EnemyWaveStatus.Spawning:
                    if (enemies.Count + completedEnemies.Count == settings.EnemiesCount)
                    {
                        TransitionToWaitingForFinish();
                    }
                    else
                    {
                        RelativeSpawnTime -= context.GameTime.ElapsedGameTime;
                        double milliseconds = RelativeSpawnTime.TotalMilliseconds;
                        if (milliseconds < 0)
                        {
                            RelativeSpawnTime = TimeSpan.FromMilliseconds(settings.Interval + milliseconds);
                            SpawnEnemy(context);
                        }
                    }
                    break;
                case EnemyWaveStatus.WaitingForFinish:
                    if (enemies.Count == 0)
                    {
                        TransitionToDone();
                    }
                    break;
            }
            UpdateEnemies(context);
            base.Update(context);
        }

        internal void UpdateEnemies(UpdateContext context)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                enemy.Update(context);
                if (enemy.IsDone)
                {
                    enemies.RemoveAt(i);
                    completedEnemies.Add(enemy);
                }
            }
        }

        internal void TransitionToSpawning(UpdateContext context)
        {
            RelativeSpawnTime = TimeSpan.FromMilliseconds(settings.Interval) + Relative;
            SpawnEnemy(context);
            Status = EnemyWaveStatus.Spawning;
            logger.Info($"Wave {settings.Id} become active");
        }
        internal void TransitionToWaitingForFinish()
        {
            Status = EnemyWaveStatus.WaitingForFinish;
        }
        internal void TransitionToDone()
        {
            Status = EnemyWaveStatus.Done;
        }
        public override void Draw(IDrawContext context)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(context);
            }
            base.Draw(context);
        }
        public void SpawnEnemy(UpdateContext context)
        {
            var enemy = EnemyFactory.GetEnemyFromPool(context.ObjectPool, enemySettings);
            enemy.Init(initContext, enemySettings, path);
            enemy.CopyContentFrom(enemyTemplate);
            enemy.Start();
            enemies.Add(enemy);
            logger.Info($"Enemy spawned for wave {settings.Id}");
        }
        public override void ReleaseResources(IObjectPool objectPool)
        {
            objectPool.ReleaseObject(enemies);
            objectPool.ReleaseObject(completedEnemies);
            objectPool.ReleaseObject(enemyTemplate);
            base.ReleaseResources(objectPool);
        }
    }

    public enum EnemyWaveStatus
    {
        Ready,
        Spawning,
        WaitingForFinish,
        Done
    }
}
