using System.Diagnostics;

namespace GraveDefensor.Engine.Settings
{
    [DebuggerDisplay("{Id,nq}")]
    public class EnemyWave
    {
        public string Id { get; set; }
        /// <summary>
        /// Time to start after previous wave finishes in milliseconds.
        /// </summary>
        public int TimeOffsetToPrevious { get; set; }
        public EnemySet[] Sets { get; set; }
    }
}
