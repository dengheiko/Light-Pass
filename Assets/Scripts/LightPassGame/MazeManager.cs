using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace LightPassGame
{
    [DisallowMultipleComponent]
    public class MazeManager : MonoBehaviour
    {
        public Cell CentralCell
        {
            get
            {
                try
                {
                    return _cells[_centerX, _centerY];
                }
                catch
                {
                    return null;
                }
            }
        }
        public Cell GetCell(CellCoordinate coordinate)
        {
            try
            {
                return _cells[coordinate.X, coordinate.Y];
            }
            catch
            {
                return null;
            }
            
        }

        public Cell RandomCell()
        {
            try
            {
                var coordinate = new CellCoordinate(
                    _random.Next(width),
                    _random.Next(height));
                return GetCell(coordinate);
            }
            catch
            {
                return null;
            }
            
        }

        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private int longRootLimit;
        
        private Cell[,] _cells;
        private int _centerX;
        private int _centerY;
        
        private bool[,] _passedCells;
        private Random _random;

        public void Init()
        {
            GenerateMaze();
        }
        
        private void GenerateMaze()
        {
            CreateCells();
            CreateRoots();
            RemoveLongRoots();
        }
        
        private void CreateCells()
        {
            _cells = new Cell[width, height];
            var t = transform;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var newCell = Instantiate(GameManager.Settings.cellPrefab, t);
                    newCell.transform.localPosition = Vector3.right * x + Vector3.down * y;
                    newCell.SetCoordinate(new CellCoordinate(x, y));
                    newCell.name = "Cell_" + x + "_" + y;
                    _cells[x, y] = newCell;
                }
            }

            _centerX = width / 2;
            _centerY = height / 2;

            t.position = Vector3.left * _centerX + Vector3.up * _centerY;
        }
        
        private void CreateRoots()
        {
            _random = new Random();
            _passedCells = new bool[width, height];
            RootStep(new CellCoordinate(_centerX, _centerY));
        }

        private void RootStep(CellCoordinate currentCellCoordinate)
        {
           _passedCells[currentCellCoordinate.X, currentCellCoordinate.Y] = true;

            var accessibleNeighbourCoordinates = new List<CellCoordinate>();

            foreach (var offsetCoordinate in CellOffsetCoordinates.All)
            {
                var neighbourCoordinate = currentCellCoordinate + offsetCoordinate;
                if (!neighbourCoordinate.IsValid(width, height)) continue;
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
        
        private void RemoveLongRoots()
        {
            for (var x = 0; x < width - 1; x++)
            {
                for (var y = 0; y < height - 1; y++)
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
                
                
                
                if (cell.CellPathfinderData.DistanceFromSource > longRootLimit)
                    continue;

                if (cell == target) return;

                foreach (var neighbour in cell.Neighbours().Where(neighbour => !neighbour.CellPathfinderData.Passed))
                {
                    var distanceFromSource = source.Coordinate.Distance(neighbour.Coordinate);
                    var distanceToTarget = target.Coordinate.Distance(neighbour.Coordinate);
                    
                    neighbour.CellPathfinderData.UpdateData(distanceFromSource, distanceToTarget);
                    
                    
                    cellsToPass.Add(neighbour);
                }

                cellsToPass = cellsToPass.OrderBy(c => c.CellPathfinderData.DistanceToTarget).ToList();
                
            }

            Cell.Connect(source, target);


        }
    }
}
