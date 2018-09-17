
using System.Diagnostics;

namespace GraveDefensor.Engine.Settings
{
    [DebuggerDisplay("{Id,nq}")]
    public class EnemyWave
    {
        public  string Id { get; set; }
        public string PathId { get; set; }
        public string EnemyId { get; set; }
        public int StartTimeOffset { get; set; }
        /// <summary>
        /// Expressed in milliseconds
        /// </summary>
        public int Interval { get; set; }
        public int EnemiesCount { get; set; }
    }
}
