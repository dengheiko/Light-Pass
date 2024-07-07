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

        [Header("Coins")] 
        public int numberOfCoins;
        public float periodToCreateCoin;

        [Header("Enemies")] 
        public float periodToCreateEnemy;

        [Header("Score")] 
        public ScoreCounter scoreCounter;
        
        [Header("Prefabs")] 
        public Cell cellPrefab;
        public Enemy enemyPrefab;
        public PlayerController playerPrefab;
        public Coin coinPrefab;
    }
}
