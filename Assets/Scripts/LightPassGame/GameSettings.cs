using System;
using UnityEngine;

namespace LightPassGame
{
    [Serializable]
    public struct GameSettings
    {
        
        [Header("Score")] 
        public ScoreCounter scoreCounter;

        [Header("Game Over Screen")] 
        public GameObject gameOverScreen;
        
        [Header("Prefabs")] 
        public Cell cellPrefab;
        
        public PlayerController playerPrefab;
        
    }
}
