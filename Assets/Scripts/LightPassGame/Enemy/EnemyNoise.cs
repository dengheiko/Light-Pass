using UnityEngine;

namespace LightPassGame.Enemy
{
    public class EnemyNoise : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float amplitude;
        [SerializeField] private float yOffset;

        private Transform _parentTransform;

        private void Awake()
        {
            _parentTransform = transform.parent;
        }

        private void Update()
        {
            transform.position =
                _parentTransform.position +
                Vector3.right * (Mathf.PerlinNoise(Time.time * speed, 0) * amplitude) +
                Vector3.up * (Mathf.PerlinNoise(0, (Time.time + yOffset) * speed) * amplitude);
        }
    }
}
