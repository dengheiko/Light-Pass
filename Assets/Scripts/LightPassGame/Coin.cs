using System;
using UnityEngine;
using UnityEngine.Events;

namespace LightPassGame
{
    public class Coin : CellPosition
    {
        public UnityEvent<Coin> destroyedEvent;
        
        private void OnTriggerEnter(Collider other)
        {   
            var player =  other.GetComponent<PlayerController>();
            if (player == null) return;

            destroyedEvent.Invoke(this);
            Destroy(gameObject);
        }
    }
}
