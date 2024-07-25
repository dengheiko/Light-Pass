using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

namespace LightPassGame
{
    [DisallowMultipleComponent]
    public class EnemyManager: MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private int maxEnemies = -1;
        [SerializeField] private float periodToCreateEnemy;

        private List<Enemy> _enemies;
        
        private Random _random;

        public void Init()
        {
            _enemies ??= new List<Enemy>();
            // StartCoroutine(BornEnemy());
        }
        
        private IEnumerator BornEnemy()
        {
            do
            {
                yield return new WaitForSeconds(periodToCreateEnemy);
                
                var enemy = Instantiate(enemyPrefab);
                // enemy.enemyCatchPlayerEvent.AddListener(OnGameOver);

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
            } while (maxEnemies < 0 || _enemies.Count < maxEnemies);
        }
    }
}