using System;
using System.Collections;
using UnityEngine;

namespace LightPassGame
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float speedDamping = 1f;
        [SerializeField] private Transform objectToFollow;

        private void Awake()
        {
            StartCoroutine(UpdateMovement());
        }

        private IEnumerator UpdateMovement()
        {
            var basePosition = transform.position;

            while (true)
            {
                yield return null;
                
                var position = transform.position;
                var playerPosition = GameManager.PlayerPosition;
                position = Vector3.Lerp(
                    position,
                    basePosition + Vector3.right * playerPosition.x + Vector3.up * playerPosition.y,
                    speedDamping * Time.deltaTime);
                transform.position = position;

                
            }

        }
    }
}
