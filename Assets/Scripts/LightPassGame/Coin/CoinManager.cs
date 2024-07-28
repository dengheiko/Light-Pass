using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LightPassGame.Game;
using UnityEngine;

namespace LightPassGame.Coin
{
    [DisallowMultipleComponent]
    public class CoinManager: MonoBehaviour
    {
        [SerializeField] private int numberOfCoins;
        [SerializeField] private float periodToCreateCoin;
        [SerializeField] private float minDistanceToCreateCoin;
        
        [SerializeField] private LightPassGame.Coin.Coin coinPrefab;
        
        private List<LightPassGame.Coin.Coin> _coins;

        public void Init()
        {
            _coins ??= new List<LightPassGame.Coin.Coin>();
            GameManager.Events.OnCoinDestroy += DeleteCoinFromList;
            StartCoroutine(BornCoin());
        }

        private IEnumerator BornCoin()
        {
            InstantiateCoin();
            while (true)
            {
                yield return new WaitForSeconds(periodToCreateCoin);
                if (_coins.Count >= numberOfCoins) continue;
                InstantiateCoin();
            }
        }

        private void InstantiateCoin()
        {
            var coin = Instantiate(coinPrefab);
            for (var i = 0; i < 10; i++)
            {
                var cell = GameManager.Maze.RandomCell();
                if(GameManager.DistanceToPlayer(cell.transform.position) < minDistanceToCreateCoin)
                    continue;
                coin.InitCurrentCell(cell);
                break;
            }
            _coins.Add(coin);
        }
        
        private bool CheckDistanceToCoins(Vector3 position) =>
            _coins.All(coin => !(Vector3.Distance(position, coin.transform.position) < 4));
        
        
        private void DeleteCoinFromList(LightPassGame.Coin.Coin coin)
        {
            _coins.Remove(coin);
        }
    }
}