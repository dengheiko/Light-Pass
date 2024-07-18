using System;
using UnityEngine;
using UnityEngine.Events;

namespace LightPassGame
{
    public class Coin : CellPosition
    {
        public UnityEvent<Coin> destroyedEvent;

        [SerializeField] private ParticleSystem coinExplodePrefab;
        
        private void OnTriggerEnter(Collider other)
        {   
            var player =  other.GetComponent<PlayerController>();
            if (player == null) return;

            destroyedEvent.Invoke(this);
            
            var explode = Instantiate(coinExplodePrefab);
            explode.transform.position = transform.position;
            
            Destroy(gameObject);
        }
    }
}
