namespace GraveDefensor.Engine.Settings
{
    public abstract class Enemy
    {
        public string Name { get; set; }
        /// <summary>
        /// Expressed in pixels per second
        /// </summary>
        public int Speed { get; set; }
        public int Health { get; set; }
        public int Award { get; set; }
    }
}
