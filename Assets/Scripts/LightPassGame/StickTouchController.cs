using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LightPassGame
{
    public class StickTouchController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float detectThreshold = 0.2f;
        
        [Header("Visuals")]
        [SerializeField] private Transform bigCircle;
        [SerializeField] private Transform smallCircle;
        
        private MovementDirection _direction;
        
        private bool _isTouching;
        private float _radius;

        private static StickTouchController _instance;

        public static bool IsDirection(MovementDirection direction)
        {
            if (_instance == null) return false;
            return _instance._direction == direction;
        }
        
        private void Awake()
        {
            _instance = this;
            _direction = MovementDirection.Stay;
            SetTouching(false);
        }

        private void Start()
        {
            var corners = new Vector3[4];
            bigCircle.GetComponent<RectTransform>().GetWorldCorners(corners);
            _radius = (corners[3].x - corners[0].x) / 2;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            bigCircle.position = eventData.position;
            smallCircle.position = eventData.position;
            SetTouching(true);
            StartCoroutine(UpdateValue());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetTouching(false);
        }

        private void SetTouching(bool visible)
        {
            _isTouching = visible;
            bigCircle.gameObject.SetActive(visible);
            smallCircle.gameObject.SetActive(visible);
        }

        private IEnumerator UpdateValue()
        {
            while (_isTouching)
            {
                var value = (Input.mousePosition - bigCircle.position) / _radius;

                
                
                var verticalDirection = Mathf.Abs(value.x) < Mathf.Abs(value.y);
                
                value.x = verticalDirection ? 0 : Mathf.Sign(value.x);
                value.y = !verticalDirection ? 0 : Mathf.Sign(value.y);
                
                smallCircle.position = bigCircle.position + value * _radius;

                if (value.x > 0) _direction = MovementDirection.Right;
                else if (value.x < 0) _direction = MovementDirection.Left;
                else if (value.y > 0) _direction = MovementDirection.Up;
                else if (value.y < 0) _direction = MovementDirection.Down;
                
                yield return null;
            }

            _direction = MovementDirection.Stay;

        }

    }
}
