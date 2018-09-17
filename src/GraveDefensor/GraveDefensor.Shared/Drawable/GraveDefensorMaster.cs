using Settings = GraveDefensor.Engine.Settings;
using GraveDefensor.Shared.Service.Abstract;

namespace GraveDefensor.Shared.Drawable
{
    public sealed class GraveDefensorMaster: Master
    {
        public void Init(IInitContext context, Settings.Master settings)
        {
            var scene = context.ObjectPool.GetObject<BattleScene>();
            scene.Init(context, CreateTestSettings());
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
                WeaponPlaces = new[] {
                    new Settings.WeaponPlace
                    {
                        Center = new Settings.Point { X = 100, Y = 100},
                        Size = new Settings.Size { Width = 32, Height = 32 }
                    },
                    new Settings.WeaponPlace
                    {
                        Center = new Settings.Point { X = 200, Y = 150},
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
                            new Settings.Point { X = -10, Y = 50 },
                            new Settings.Point { X = 700, Y = 50 },
                            new Settings.Point { X = 720, Y = 70 },
                            new Settings.Point { X = 720, Y = 400 },
                            new Settings.Point { X = 700, Y = 420 },
                            new Settings.Point { X = 32, Y = 420 },
                        }
                    }
                },
                Enemies = new Settings.Enemy[]
                {
                    new Settings.GroundEnemy { Id = "First", Speed = 40 },
                    new Settings.GroundEnemy { Id = "Second", Speed = 15 }
                },
                Waves = new Settings.EnemyWave[]
                {
                    new Settings.EnemyWave
                    {
                        Id = "First",
                        PathId = "Core",
                        EnemyId = "First",
                        StartTimeOffset = 500,
                        Interval = 50,
                        EnemiesCount = 10
                    }
                }
            };
        }
    }
}
