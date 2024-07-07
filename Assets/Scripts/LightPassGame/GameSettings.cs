using System;
using UnityEngine;

namespace LightPassGame
{
    [Serializable]
    public struct GameSettings
    {
        [Header("Maze size")]
        public int width;
        public int height;

        [Header("Coins")] 
        public int numberOfCoins;
        public float periodToCreateCoin;
        public float minDistanceToCreateCoin;

        [Header("Enemies")] 
        public float periodToCreateEnemy;

        [Header("Score")] 
        public ScoreCounter scoreCounter;

        [Header("Game Over Screen")] 
        public GameObject gameOverScreen;
        
        [Header("Prefabs")] 
        public Cell cellPrefab;
        public Enemy enemyPrefab;
        public PlayerController playerPrefab;
        public Coin coinPrefab;
    }
}
