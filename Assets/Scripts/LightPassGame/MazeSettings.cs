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

        [Header("Enemies")] 
        public float periodToCreateEnemy;

        [Header("Prefabs")] 
        public Cell cellPrefab;
        public Enemy enemyPrefab;
        public PlayerController playerPrefab;
    }
}
