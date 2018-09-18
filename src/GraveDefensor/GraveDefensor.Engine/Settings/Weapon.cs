namespace GraveDefensor.Engine.Settings
{
    public class Weapon
    {
        public string Name { get; set; }
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
}
