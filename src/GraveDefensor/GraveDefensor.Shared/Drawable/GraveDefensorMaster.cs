﻿using Settings = GraveDefensor.Engine.Settings;
using GraveDefensor.Shared.Service.Abstract;

namespace GraveDefensor.Shared.Drawable
{
    public sealed class GraveDefensorMaster: Master
    {
        Settings.Size windowSize;
        public void Init(IInitContext context, Settings.Master settings, Settings.Size windowSize)
        {
            this.windowSize = windowSize;
            var scene = context.ObjectPool.GetObject<BattleScene>();
            scene.Init(context, CreateTestSettings(), windowSize);
            CurrentScene = scene;
        }
        public override void InitContent(IInitContentContext context)
        {
            CurrentScene.InitContent(context);
            base.InitContent(context);
        }

        Settings.Battle CreateTestSettings()
        {
            return new Settings.Battle
            {
                Health = 500,
                Cash = 200,
                WeaponPlaces = new[] {
                    new Settings.WeaponPlace
                    {
                        Center = new Settings.Point { X = 100, Y = 120},
                        Size = new Settings.Size { Width = 32, Height = 32 }
                    },
                    new Settings.WeaponPlace
                    {
                        Center = new Settings.Point { X = 200, Y = 170},
                        Size = new Settings.Size { Width = 32, Height = 32 }
                    }
                },
                Paths = new Settings.Path[]
                {
                    new Settings.Path
                    {
                        Id = "Core",
                        Points = new Settings.Point[]
                        {
                            new Settings.Point { X = -10, Y = 70 },
                            new Settings.Point { X = 700, Y = 70 },
                            new Settings.Point { X = 720, Y = 90 },
                            new Settings.Point { X = 720, Y = 420 },
                            new Settings.Point { X = 700, Y = 440 },
                            new Settings.Point { X = 32, Y = 440 },
                        }
                    }
                },
                Enemies = new Settings.Enemy[]
                {
                    new Settings.Enemy { Id = "Creepy Worm", Speed = 40, Health = 120, Award = 25 },
                    new Settings.Enemy { Id = "Skeleton", Speed = 15, Health = 180, Award = 30 }
                },
                Waves = new Settings.EnemyWave[]
                {
                    new Settings.EnemyWave
                    {
                        Id = "First",
                        PathId = "Core",
                        EnemyId = "Creepy Worm",
                        StartTimeOffset = 500,
                        Interval = 50,
                        EnemiesCount = 10
                    }
                },
                Weapons = new Settings.Weapon[]
                {
                    new Settings.Weapon
                    {
                        Name = "Mini gun",
                        FiringRange = 70,
                        TrackingRange = 75,
                        Power = 3,
                        Speed = 260,
                        AirCapability = true,
                        GroundCapability = true
                    },
                    new Settings.Weapon
                    {
                        Name = "Vulcan",
                        FiringRange = 65,
                        TrackingRange = 70,
                        Power = 4,
                        Speed = 214,
                        AirCapability = true,
                        GroundCapability = true
                    }
                    ,
                    new Settings.Weapon
                    {
                        Name = "Cannon",
                        FiringRange = 110,
                        TrackingRange = 120,
                        Power = 125,
                        Speed = 12,
                        AirCapability = false,
                        GroundCapability = true
                    }
                }
            };
        }
    }
}
