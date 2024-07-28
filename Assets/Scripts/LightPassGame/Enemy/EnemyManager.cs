using System.Collections;
using System.Collections.Generic;
using LightPassGame.Game;
using UnityEngine;
using Random = System.Random;

namespace LightPassGame.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyManager: MonoBehaviour
    {
        [SerializeField] private LightPassGame.Enemy.Enemy enemyPrefab;
        [SerializeField] private int maxEnemies = -1;
        [SerializeField] private float periodToCreateEnemy;

        private List<LightPassGame.Enemy.Enemy> _enemies;
        
        private Random _random;

        public void Init()
        {
            _enemies ??= new List<LightPassGame.Enemy.Enemy>();
            _random = new Random();
            StartCoroutine(BornEnemy());
        }
        
        private IEnumerator BornEnemy()
        {
            do
            {
                var enemy = Instantiate(enemyPrefab);

                if (_enemies.Count == 0)
                {
                    enemy.InitCurrentCell(GameManager.Maze.RandomCell());
                }
                else
                {
                    var sourceEnemy = _enemies[_random.Next(_enemies.Count)];
                    enemy.InitCurrentCell(sourceEnemy.CurrentCell);
                }

                _enemies.Add(enemy);
                
                yield return new WaitForSeconds(periodToCreateEnemy);
            } 
            while (maxEnemies < 0 || _enemies.Count < maxEnemies);
        }
    }
}