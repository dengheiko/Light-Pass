using System;
using System.Collections.Generic;
using UnityEngine;

namespace LightPassGame
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject[] wallGameObjects;

        private Cell[] _neighbours;
        private CellCoordinate _coordinate;
        
        private void Awake()
        {
            _neighbours = new Cell[4];

            // InitWallGameObjects();
        }

        public void SetCoordinate(CellCoordinate coordinate)
        {
            _coordinate = coordinate;
        }
        
        private void InitWallGameObjects()
        {
            wallGameObjects = transform.GetChild(0).transform.GetComponentsInChildren<GameObject>();
        }

        public void UpdateWallsVisibility(IReadOnlyList<bool> walls)
        {
            for (var i = 0; i < walls.Count; i++)
            {
                wallGameObjects[i].SetActive(walls[i]);
            }
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
            cell1._neighbours[neighbourCell1] = cell2;
            cell1.wallGameObjects[neighbourCell1].SetActive(false);
            
            var neighbourCell2 = vertical ? 0 : 3;
            cell2._neighbours[neighbourCell2] = cell1;
            cell2.wallGameObjects[neighbourCell2].SetActive(false);
        }
        
        
    }
}
