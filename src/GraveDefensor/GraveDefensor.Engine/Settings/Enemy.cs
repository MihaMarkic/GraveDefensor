namespace GraveDefensor.Engine.Settings
{
    public abstract class Enemy
    {
        public string Id { get; set; }
        /// <summary>
        /// Expressed in pixels per second
        /// </summary>
        public int Speed { get; set; }
    }
    public class GroundEnemy: Enemy
    {

    }
}
