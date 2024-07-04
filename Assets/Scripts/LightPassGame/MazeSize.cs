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

        [Header("Prefabs")] public Cell cellPrefab;
    }
}
