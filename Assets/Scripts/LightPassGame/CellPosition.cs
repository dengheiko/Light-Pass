using UnityEngine;

namespace LightPassGame
{
    public class CellPosition : MonoBehaviour
    {
        public Cell CurrentCell { get; protected set; }
        public void InitCurrentCell(Cell cell)
        {
            CurrentCell = cell;
            transform.position = cell.transform.position;
        }
    }
}
