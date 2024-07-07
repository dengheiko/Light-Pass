using UnityEngine;

namespace LightPassGame
{
    public class Coin : MonoBehaviour
    {
        public Cell CurrentCell { get; private set; }
        
        public void InitCurrentCell(Cell cell)
        {
            CurrentCell = cell;
            transform.position = cell.transform.position;
        }
    }
}
