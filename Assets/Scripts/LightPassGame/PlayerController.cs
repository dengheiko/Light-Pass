using UnityEngine;

namespace LightPassGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
    
        private Vector3 _prevMousePosition;
        private Vector3 _targetPosition;
    
        private void Update()
        {
            MouseDownUpdate();
            MoveToTargetUpdate();
        }

        private void MouseDownUpdate()
        {
            if (!Input.GetMouseButton(0)) return;
            if (Input.GetMouseButtonDown(0))
                PrevMousePositionUpdate();
        
        
        
            PrevMousePositionUpdate();
        }

        private void PrevMousePositionUpdate()
        {
            _prevMousePosition = Input.mousePosition;
        }

        private void MoveToTargetUpdate()
        {
            transform.position = Vector3.Lerp(
                transform.position,
                _targetPosition,
                Time.deltaTime * speed);
        }
    }
}
