
namespace GraveDefensor.Engine.Settings
{
    public class Battle
    {
        public int Health { get; set; }
        public int Amount { get; set; }
        public Path[] Paths { get; set; }
        public WeaponPlace[] WeaponPlaces { get; set; }
        public EnemyWave[] Waves { get; set; }
        public Weapon[] Weapons { get; set; }
    }
}
