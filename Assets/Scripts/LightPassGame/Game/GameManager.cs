using LightPassGame.Coin;
using LightPassGame.Enemy;
using LightPassGame.Maze;
using LightPassGame.Player;
using LightPassGame.UI;
using UnityEngine;

namespace LightPassGame.Game
{
    [RequireComponent(typeof(MazeManager))]
    [RequireComponent(typeof(EnemyManager))]
    [RequireComponent(typeof(CoinManager))]
    [RequireComponent(typeof(ScoreManager))]
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static MazeManager Maze => _instance._mazeManager;
        public static EnemyManager Enemies => _instance._enemyManager;
        public static CoinManager Coins => _instance._coinManager;
        public static EventManager Events => _instance._eventManager;

        public static ScoreManager Score => _instance._scoreManager;
        public static Vector3 PlayerPosition => _instance._player.transform.position;

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private GameOverScreen gameOverScreen;
        
        private MazeManager _mazeManager;
        private EnemyManager _enemyManager;
        private CoinManager _coinManager;
        private ScoreManager _scoreManager;
        private PlayerController _player;
        private EventManager _eventManager;
        

        private void Awake()
        {
            _instance = this;
            
            _mazeManager = GetComponent<MazeManager>();
            _enemyManager = GetComponent<EnemyManager>();
            _coinManager = GetComponent<CoinManager>();
            _scoreManager = GetComponent<ScoreManager>();
            _eventManager = new EventManager();
            Events.OnPlayerDamage += StartGameOverScreen;
        }
        private void Start()
        {
            Maze.Init();
            InitPlayer();
            Enemies.Init();
            Coins.Init();
        }
        private void InitPlayer()
        {
            _player = Instantiate(playerPrefab);
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
        private void StartGameOverScreen()
        {
            Instantiate(gameOverScreen);
        }

        
    }
}
