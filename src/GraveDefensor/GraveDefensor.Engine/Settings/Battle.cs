
namespace GraveDefensor.Engine.Settings
{
    public class Battle
    {
        public int Health { get; set; }
        public int Cash { get; set; }
        public Path[] Paths { get; set; }
        public WeaponPlace[] WeaponPlaces { get; set; }
        public Enemy[] Enemies { get; set; }
        public EnemyWave[] Waves { get; set; }
        public Weapon[] Weapons { get; set; }
    }
}
