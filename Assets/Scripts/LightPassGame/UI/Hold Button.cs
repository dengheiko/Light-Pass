using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LightPassGame.UI
{
    public class HoldButton : MonoBehaviour
    {
        public UnityAction OnButtonPressed;
        
        [SerializeField] private float timeToInvoke = 2;
        [SerializeField] private RectTransform bgFillRectTransform;
        
        
        
        private void Awake()
        {
            SetBgFill(0);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Accept") == false) return;
            StartCoroutine(HoldCoroutine());
        }

        private IEnumerator HoldCoroutine()
        {
            var startTime = Time.time;

            for (var delta = 0f; delta < 1; delta = (Time.time - startTime) / timeToInvoke)
            {
                if (Input.GetButton("Accept") == false)
                {
                    SetBgFill(0);
                    yield break;
                }

                SetBgFill(delta);
                yield return null;
            }

            SetBgFill(1);
            OnButtonPressed?.Invoke();
        }

        private void SetBgFill(float delta)
        {
            bgFillRectTransform.anchorMax = Vector2.right * delta + Vector2.up;
        }
    }
}
