using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


namespace LightPassGame
{
    public class Maze : MonoBehaviour
    {
        [SerializeField] private MazeSettings mazeSettings;

        private Cell[,] _cells;

        private int _centerX;
        private int _centerY;

        private Random _random;
        private bool[,] _passedCells;

        private List<Enemy> _enemies;

        private void Start()
        {
            GenerateMaze();
            InvokeBornEnemy();
        }

        private void InvokeBornEnemy(float timeToInvoke = 0)
        {
            Invoke(nameof(BornEnemy), timeToInvoke);
        }

        private void BornEnemy()
        {
            _enemies ??= new List<Enemy>();
            
            var enemy = Instantiate(mazeSettings.enemyPrefab);
            
            if (_enemies.Count == 0)
            {
                
                var coordinate = new CellCoordinate(
                    _random.Next(mazeSettings.width),
                    _random.Next(mazeSettings.height));

                enemy.InitCurrentCell(_cells[coordinate.X, coordinate.Y]);
            }
            else
            {
                var sourceEnemy = _enemies[_random.Next(_enemies.Count)];
                enemy.InitCurrentCell(sourceEnemy.CurrentCell);
            }

            _enemies.Add(enemy);
            InvokeBornEnemy(mazeSettings.periodToCreateEnemy);

        }
        
        private void CreateEnemies()
        {
            
        }


        private void GenerateMaze()
        {
            CreateCells();
            CreateRoots();
        }

        private void CreateCells()
        {
            _cells = new Cell[mazeSettings.width, mazeSettings.height];
            var t = transform;
            for (var x = 0; x < mazeSettings.width; x++)
            {
                for (var y = 0; y < mazeSettings.height; y++)
                {
                    var newCell = Instantiate(mazeSettings.cellPrefab, t);
                    newCell.transform.localPosition = Vector3.right * x + Vector3.down * y;
                    newCell.SetCoordinate(new CellCoordinate(x, y));
                    newCell.name = "Cell_" + x + "_" + y;
                    _cells[x, y] = newCell;
                }
            }

            _centerX = mazeSettings.width / 2;
            _centerY = mazeSettings.height / 2;

            t.position = Vector3.left * _centerX + Vector3.up * _centerY;
        }

        private void CreateRoots()
        {
            _random = new Random();
            _passedCells = new bool[mazeSettings.width, mazeSettings.height];
            RootStep(new CellCoordinate(_centerX, _centerY));
        }

        private void RootStep(CellCoordinate currentCellCoordinate)
        {
           _passedCells[currentCellCoordinate.X, currentCellCoordinate.Y] = true;

            var accessibleNeighbourCoordinates = new List<CellCoordinate>();

            foreach (var offsetCoordinate in CellOffsetCoordinates.All)
            {
                var neighbourCoordinate = currentCellCoordinate + offsetCoordinate;
                if (!neighbourCoordinate.IsValid(mazeSettings)) continue;
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
    }
}
