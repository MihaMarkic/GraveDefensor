namespace GraveDefensor.Engine.Settings
{
    public abstract class Weapon
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int FiringRange { get; set; }
        public int TrackingRange { get; set; }
        /// <summary>
        /// Rounds per second
        /// </summary>
        public int Speed { get; set; }
        public int Power { get; set; }
        public bool AirCapability { get; set; }
        public bool GroundCapability { get; set; }
    }

    public abstract class RotationalWeapon: Weapon
    {

    }

    public class MiniGun : RotationalWeapon
    {

    }

    public class Vulcan : RotationalWeapon
    {

    }

    public class Cannon : RotationalWeapon
    {

    }
}
