using LightPassGame.Game;
using UnityEngine;

namespace LightPassGame.Player
{
    public class PlayerDamage : MonoBehaviour
    {
        [SerializeField] private ParticleSystem playerDeathEffect;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy.Enemy>(out var enemy))
            {
                var deathEffect = Instantiate(playerDeathEffect);
                deathEffect.transform.position = transform.position;

                GetComponent<PlayerController>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                GameManager.Events.OnPlayerDamage?.Invoke();
            }
        }
    }
}
