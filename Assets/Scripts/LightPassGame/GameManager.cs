using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;


namespace LightPassGame
{
    [RequireComponent(typeof(MazeManager))]
    [RequireComponent(typeof(EnemyManager))]
    [RequireComponent(typeof(CoinManager))]
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        public static MazeManager Maze => _instance._mazeManager;
        public static EnemyManager Enemies => _instance._enemyManager;
        public static CoinManager Coins => _instance._coinManager;
        public static EventManager Events => _instance._eventManager;
        
        private static GameManager _instance;
        public static Vector3 PlayerPosition => _instance._player.transform.position;

        public static GameSettings Settings => _instance.gameSettings;

        [SerializeField] private GameSettings gameSettings;

        private MazeManager _mazeManager;
        private EnemyManager _enemyManager;
        private CoinManager _coinManager;
        private PlayerController _player;
        private EventManager _eventManager;
        
        private Random _random;

        private void Awake()
        {
            _instance = this;
            _mazeManager = GetComponent<MazeManager>();
            _enemyManager = GetComponent<EnemyManager>();
            _coinManager = GetComponent<CoinManager>();
            _eventManager = new EventManager();
        }

        private void Start()
        {
            gameSettings.gameOverScreen.SetActive(false);

            Maze.Init();
            InitPlayer();
            Enemies.Init();
            Coins.Init();
        }

        private void InitPlayer()
        {
            _player = Instantiate(gameSettings.playerPrefab);
            _player.InitCurrentCell(_mazeManager.CentralCell);
        }

        public static float DistanceToPlayer(Vector3 position)
        {
            try
            {
                return Vector3.Distance(_instance._player.transform.position, position);
            }
            catch
            {
                return float.MaxValue;
            }
        }

        private void OnGameOver(Enemy enemy)
        {
            gameSettings.gameOverScreen.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Scenes/Main Scene");
        }
    }
}
