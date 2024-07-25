using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace LightPassGame
{
    [DisallowMultipleComponent]
    public class CoinManager: MonoBehaviour
    {
        [SerializeField] private int numberOfCoins;
        [SerializeField] private float periodToCreateCoin;
        [SerializeField] private float minDistanceToCreateCoin;
        
        [SerializeField] private Coin coinPrefab;
        
        private List<Coin> _coins;

        public void Init()
        {
            _coins ??= new List<Coin>();
            GameManager.Events.OnCoinDestroy += DeleteCoinFromList;
            StartCoroutine(BornCoin());
        }

        private IEnumerator BornCoin()
        {
            while (true)
            {
                yield return new WaitForSeconds(periodToCreateCoin);

                if (_coins.Count >= numberOfCoins) continue;

                var coin = Instantiate(coinPrefab);

                while (true)
                {
                    var cell = GameManager.Maze.RandomCell();

                    if(GameManager.DistanceToPlayer(cell.transform.position) < minDistanceToCreateCoin)
                        continue;

                    // if (CheckDistanceToCoins(cell.transform.position) == false)
                    //     continue;


                    coin.InitCurrentCell(cell);
                    break;
                }
                _coins.Add(coin);
            }
        }
        
        private bool CheckDistanceToCoins(Vector3 position) =>
            _coins.All(coin => !(Vector3.Distance(position, coin.transform.position) < 4));
        
        
        private void DeleteCoinFromList(Coin coin)
        {
            _coins.Remove(coin);
        }
    }
}