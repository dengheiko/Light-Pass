using System;
using UnityEngine;
using UnityEngine.Events;

namespace LightPassGame
{
    public class Coin : MonoBehaviour
    {
        public Cell CurrentCell { get; private set; }

        public UnityEvent<Coin> destroyedEvent;
        
        
        public void InitCurrentCell(Cell cell)
        {
            CurrentCell = cell;
            transform.position = cell.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {   
            var player =  other.GetComponent<PlayerController>();
            if (player == null) return;

            destroyedEvent.Invoke(this);
            Destroy(gameObject);
        }
    }
}
