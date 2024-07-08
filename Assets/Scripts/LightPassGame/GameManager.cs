using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;


namespace LightPassGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;

        private Cell[,] _cells;

        private int _centerX;
        private int _centerY;

        private Random _random;
        private bool[,] _passedCells;

        private List<Enemy> _enemies;
        private List<Coin> _coins;

        private PlayerController _player;

        private void Start()
        {
            gameSettings.gameOverScreen.SetActive(false);
            GenerateMaze();
            InitPlayer();
            InvokeBorn(nameof(BornEnemy));
            InvokeBorn(nameof(BornCoin));
        }

        private void InitPlayer()
        {
            _player = Instantiate(gameSettings.playerPrefab);
            _player.InitCurrentCell(_cells[_centerX, _centerY]);
        }

        private void InvokeBorn(string methodName,float timeToInvoke = 0)
        {
            Invoke(methodName, timeToInvoke);
        }

        private void BornEnemy()
        {
            _enemies ??= new List<Enemy>();
            
            var enemy = Instantiate(gameSettings.enemyPrefab);
            enemy.enemyCatchPlayerEvent.AddListener(OnGameOver);
            
            if (_enemies.Count == 0)
            {
                
                var coordinate = new CellCoordinate(
                    _random.Next(gameSettings.width),
                    _random.Next(gameSettings.height));

                enemy.InitCurrentCell(_cells[coordinate.X, coordinate.Y]);
            }
            else
            {
                var sourceEnemy = _enemies[_random.Next(_enemies.Count)];
                enemy.InitCurrentCell(sourceEnemy.CurrentCell);
            }

            _enemies.Add(enemy);
            InvokeBorn(nameof(BornEnemy),gameSettings.periodToCreateEnemy);

        }

        private void BornCoin()
        {
            _coins ??= new List<Coin>();

            if (_coins.Count >= gameSettings.numberOfCoins) return;

            var coin = Instantiate(gameSettings.coinPrefab);

            while (true)
            {
                var coordinate = new CellCoordinate(
                    _random.Next(gameSettings.width),
                    _random.Next(gameSettings.height));

                var cell = _cells[coordinate.X, coordinate.Y];
                if (_player != null && 
                    Vector3.Distance(cell.transform.position, _player.transform.position) < gameSettings.minDistanceToCreateCoin) 
                    continue;

                coin.InitCurrentCell(cell);
                break;
                
            }
            
            coin.destroyedEvent.AddListener(OnCoinDestroyed);
            _coins.Add(coin);
            InvokeBorn(nameof(BornCoin), gameSettings.periodToCreateCoin);

        }

        private void OnCoinDestroyed(Coin coin)
        {
            _coins.Remove(coin);
            gameSettings.scoreCounter.AddScore();
            InvokeBorn(nameof(BornCoin), gameSettings.periodToCreateCoin);
        }
        
        private void GenerateMaze()
        {
            CreateCells();
            CreateRoots();
            RemoveLongRoots();
        }

        private void CreateCells()
        {
            _cells = new Cell[gameSettings.width, gameSettings.height];
            var t = transform;
            for (var x = 0; x < gameSettings.width; x++)
            {
                for (var y = 0; y < gameSettings.height; y++)
                {
                    var newCell = Instantiate(gameSettings.cellPrefab, t);
                    newCell.transform.localPosition = Vector3.right * x + Vector3.down * y;
                    newCell.SetCoordinate(new CellCoordinate(x, y));
                    newCell.name = "Cell_" + x + "_" + y;
                    _cells[x, y] = newCell;
                }
            }

            _centerX = gameSettings.width / 2;
            _centerY = gameSettings.height / 2;

            t.position = Vector3.left * _centerX + Vector3.up * _centerY;
        }

        private void CreateRoots()
        {
            _random = new Random();
            _passedCells = new bool[gameSettings.width, gameSettings.height];
            RootStep(new CellCoordinate(_centerX, _centerY));
        }

        private void RootStep(CellCoordinate currentCellCoordinate)
        {
           _passedCells[currentCellCoordinate.X, currentCellCoordinate.Y] = true;

            var accessibleNeighbourCoordinates = new List<CellCoordinate>();

            foreach (var offsetCoordinate in CellOffsetCoordinates.All)
            {
                var neighbourCoordinate = currentCellCoordinate + offsetCoordinate;
                if (!neighbourCoordinate.IsValid(gameSettings)) continue;
                accessibleNeighbourCoordinates.Add(neighbourCoordinate);
            }

            while (true)
            {
                var notPassedNeighbourCoordinates = new List<CellCoordinate>();
                foreach (var cellCoordinate in accessibleNeighbourCoordinates)
                {
                    if (_passedCells[cellCoordinate.X, cellCoordinate.Y]) continue;
                    notPassedNeighbourCoordinates.Add(cellCoordinate);
                }

                if (notPassedNeighbourCoordinates.Count == 0) break;
                
                var rndCoordinate = _random.Next(notPassedNeighbourCoordinates.Count);
                var nextCoordinate = notPassedNeighbourCoordinates[rndCoordinate];

                Cell.Connect(
                    _cells[currentCellCoordinate.X, currentCellCoordinate.Y],
                    _cells[nextCoordinate.X, nextCoordinate.Y]);
                RootStep(nextCoordinate);
            }

            var currentCell = _cells[currentCellCoordinate.X, currentCellCoordinate.Y];
            if (currentCell.NeighboursCount > 1) return;

            while (true)
            {
                var rndCoordinate = _random.Next(accessibleNeighbourCoordinates.Count);
                var nextCoordinate = accessibleNeighbourCoordinates[rndCoordinate];
                var nextCell = _cells[nextCoordinate.X, nextCoordinate.Y];

                if (currentCell.IsNeighbour(nextCell)) continue;
                Cell.Connect(currentCell, nextCell);
                break;
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

        #region Detect Long Roots

        private void RemoveLongRoots()
        {
            for (var x = 0; x < gameSettings.width - 1; x++)
            {
                for (var y = 0; y < gameSettings.height - 1; y++)
                {
                    CheckTwoCellsForLongRoot(_cells[x, y], _cells[x, y + 1]);
                    CheckTwoCellsForLongRoot(_cells[x, y], _cells[x + 1, y]);
                }
            }
        }

        private void CheckTwoCellsForLongRoot(Cell source, Cell target)
        {
            CellPathfinderData.ResetAll();
            var cellsToPass = new List<Cell> {source};
            source.CellPathfinderData.UpdateData(0, 0);
            
            while (true)
            {
                if (cellsToPass.Count == 0) break;
                
                var cell = cellsToPass[0];

                cell.CellPathfinderData.Passed = true;
                cellsToPass.Remove(cell);
                
                
                
                if (cell.CellPathfinderData.DistanceFromSource > gameSettings.longRootLimit)
                    continue;

                if (cell == target) return;

                foreach (var neighbour in cell.Neighbours().Where(neighbour => !neighbour.CellPathfinderData.Passed))
                {
                    var distanceFromSource = source.Coordinate.Distance(neighbour.Coordinate);
                    var distanceToTarget = target.Coordinate.Distance(neighbour.Coordinate);

                    // Debug.Log("========");
                    // Debug.Log(neighbour.CellPathfinderData.DistanceFromSource + ", " + neighbour.CellPathfinderData.DistanceToTarget);
                    // Debug.Log(distanceFromSource + ", " + distanceToTarget);
                    
                    neighbour.CellPathfinderData.UpdateData(distanceFromSource, distanceToTarget);
                    
                    // Debug.Log(neighbour.CellPathfinderData.DistanceFromSource + ", " + neighbour.CellPathfinderData.DistanceToTarget);
                    
                    cellsToPass.Add(neighbour);
                }

                cellsToPass = cellsToPass.OrderBy(c => c.CellPathfinderData.DistanceToTarget).ToList();
                
            }

            Cell.Connect(source, target);
            // Debug.Log("CONNECT " + source.Coordinate.ToString() + " > " + target.Coordinate.ToString());


        }
        
        

        #endregion
    }
}
