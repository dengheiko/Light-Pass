using System;
using UnityEngine;

namespace LightPassGame
{
    [Serializable]
    public struct MazeSettings
    {
        [Header("Maze size")]
        public int width;
        public int height;

        [Header("Targets")] 
        public int targetsCount;
        public float periodToCreate;

        [Header("Prefabs")] 
        public Cell cellPrefab;
        public Target targetPrefab;
    }
}
