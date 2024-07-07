using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace LightPassGame
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject[] wallGameObjects;

        private Cell[] _allNeighbours;
        private List<Cell> _realNeighbours;
        private CellCoordinate _coordinate;

        private Random _random;
        
        private void Awake()
        {
            _allNeighbours = new Cell[4];
            _realNeighbours = new List<Cell>();
            _random = new Random();
        }

        public void SetCoordinate(CellCoordinate coordinate)
        {
            _coordinate = coordinate;
        }

        public static void Connect(Cell cell1, Cell cell2)
        {
            var vertical=false;
            if (cell1._coordinate.X == cell2._coordinate.X)
            {
                if (cell1._coordinate.Y > cell2._coordinate.Y)
                {
                    var c = cell2;
                    cell2 = cell1;
                    cell1 = c;
                }
                vertical = true;
            }
            else if (cell1._coordinate.Y == cell2._coordinate.Y)
            {
                if (cell1._coordinate.X > cell2._coordinate.X)
                {
                    var c = cell2;
                    cell2 = cell1;
                    cell1 = c;
                }
            }
            else
            {
                return;
            }

            var neighbourCell1 = vertical ? 2 : 1;
            cell1._allNeighbours[neighbourCell1] = cell2;
            cell1.wallGameObjects[neighbourCell1].SetActive(false);
            cell1._realNeighbours.Add(cell2);
            
            var neighbourCell2 = vertical ? 0 : 3;
            cell2._allNeighbours[neighbourCell2] = cell1;
            cell2.wallGameObjects[neighbourCell2].SetActive(false);
            cell2._realNeighbours.Add(cell1);
        }

        public int NeighboursCount =>
            _realNeighbours.Count;

        public bool IsNeighbour(Cell cell) =>
            _realNeighbours.Contains(cell);

        public Cell RandomNeighbour(Cell previousCell)
        {
            while (true)
            {
                var neighbour = _realNeighbours[_random.Next(_realNeighbours.Count)];
                if (neighbour == previousCell) continue;
                return neighbour;
            }
        }

        public Cell NeighbourOnDirection(MovementDirection direction) =>
            direction switch
            {
                MovementDirection.Up => _allNeighbours[0],
                MovementDirection.Down => _allNeighbours[2],
                MovementDirection.Right => _allNeighbours[1],
                MovementDirection.Left => _allNeighbours[3],
                _ => null
            };

    }
}
