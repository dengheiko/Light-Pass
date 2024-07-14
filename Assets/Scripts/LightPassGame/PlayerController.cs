using UnityEngine;
using UnityEngine.InputSystem;

namespace LightPassGame
{
    public class PlayerController : CellPosition
    {
        [SerializeField] private float speed = 1;

        
        private Cell _targetCell;
        private float _moveDelta;


        private Vector2 _movementVector;
        private MovementDirection _movementDirection;
        private MovementDirection _previousMovementDirection;

        private void OnMovement(InputAction.CallbackContext context)
        {
            _movementVector = context.ReadValue<Vector2>();
        }
        
        private void Update()
        {
            InputUpdate();
            PickTargetCell();
            MoveUpdate();
        }
        
        

        private void InputUpdate()
        {
            _previousMovementDirection = _movementDirection;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || StickTouchController.IsDirection(MovementDirection.Up)) 
                _movementDirection = MovementDirection.Up;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || StickTouchController.IsDirection(MovementDirection.Down)) 
                _movementDirection = MovementDirection.Down;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || StickTouchController.IsDirection(MovementDirection.Right)) 
                _movementDirection = MovementDirection.Right;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || StickTouchController.IsDirection(MovementDirection.Left)) 
                _movementDirection = MovementDirection.Left;
        }

        private void PickTargetCell()
        {
            if (_previousMovementDirection != _movementDirection && _moveDelta < .5f)
            {
                var tryCell = CurrentCell.NeighbourOnDirection(_movementDirection);
                if (tryCell != null)
                {
                    _targetCell = tryCell;
                    _moveDelta = 0;
                }
            }
            
            if (_targetCell != null) return;
            _targetCell = CurrentCell.NeighbourOnDirection(_movementDirection);
            _moveDelta = 0;
            if (_targetCell == null) _movementDirection = MovementDirection.Stay;
        }

        private void MoveUpdate()
        {
            if (_targetCell == null) return;
            
            _moveDelta += speed * Time.deltaTime;
            
            if (_moveDelta >= 1)
            {
                transform.position = _targetCell.transform.position;
                CurrentCell = _targetCell;
                _targetCell = null;
                _movementDirection = MovementDirection.Stay;
                return;
            }

            transform.position = Vector3.Lerp(
                CurrentCell.transform.position,
                _targetCell.transform.position,
                _moveDelta);
        }
    }
}
