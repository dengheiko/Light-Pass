using UnityEngine;

namespace LightPassGame
{
    public class Target : MonoBehaviour
    {
        private Cell _currentCell;

        public void InitCurrentCell(Cell cell)
        {
            _currentCell = cell;
            transform.position = cell.transform.position;
        }
    }
}
