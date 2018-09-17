namespace GraveDefensor.Engine.Settings
{
    public abstract class Weapon
    {
        public int Range { get; set; }
        /// <summary>
        /// Expressed in milliseconds.
        /// </summary>
        public int ReloadTime { get; set; }
    }

    public class MachineGun: Weapon
    {

    }
}
