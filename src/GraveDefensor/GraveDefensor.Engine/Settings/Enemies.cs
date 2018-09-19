﻿using System;

namespace GraveDefensor.Engine.Settings
{
    public class Enemies
    {
        public CreepyWorm CreepyWorm { get; set; }
        public Skeleton Skeleton { get; set; }
        public Enemy GetEnemyById(string id)
        {
            switch (id)
            {
                case "CreepyWorm": return CreepyWorm;
                case "Skeleton": return Skeleton;
                default: throw new ArgumentException($"Could not find enemy with id {id}", nameof(id));
            }
        }
    }
}
