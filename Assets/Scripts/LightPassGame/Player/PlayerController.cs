using LightPassGame.Maze;
using UnityEngine;

namespace LightPassGame.Player
{
    public class PlayerController : CellPosition
    {
        private const float InputThreshold = 0.1f;
        private const float DistanceToCenter = 0.2f;
        
        [SerializeField] private float speed = 1;
        [SerializeField] private float alignToPathSpeed = 5;

        
        private Vector3 _inputVector;
        private Cell _targetCell;

        private enum MovementType
        {
            None,
            X,
            Y
        }

        private MovementType _movementType;

        private void Update()
        {
            InputUpdate();
            UpdateTargetCell();
            NormalMoveX();
            NormalMoveY();
            CornerMoveX();
            CornerMoveY();
            UpdateCurrentCell();
            AlignToPath();
        }

        private void InputUpdate()
        {
            _inputVector = Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical");
            if (_inputVector.magnitude < InputThreshold)
            {
                _inputVector = Vector3.zero;
                _movementType = MovementType.None;
                return;
            }

            if (Mathf.Abs(_inputVector.x) > Mathf.Abs(_inputVector.y))
            {
                _inputVector.y = 0;
                _movementType = MovementType.X;
            }
            else
            {
                _inputVector.x = 0;
                _movementType = MovementType.Y;
            }
        }

        private void UpdateTargetCell()
        {
            _targetCell = _movementType switch
            {
                MovementType.X => CurrentCell.NeighbourOnDirection(_inputVector.x > 0
                    ? MovementDirection.Right
                    : MovementDirection.Left),
                MovementType.Y => CurrentCell.NeighbourOnDirection(_inputVector.y > 0
                    ? MovementDirection.Up
                    : MovementDirection.Down),
                _ => null
            };
        }

        private void NormalMoveX()
        {
            if (_movementType != MovementType.X)
                return;

            if (Mathf.Abs(transform.position.y - CurrentCell.transform.position.y) > DistanceToCenter)
                return;
            
            
            transform.position += Vector3.right * (_inputVector.x * speed * Time.deltaTime);
            
            if (_targetCell == null &&
                Mathf.Sign((CurrentCell.transform.position - transform.position).x * _inputVector.x) < 0)
            {
                transform.position =
                    Vector3.right * CurrentCell.transform.position.x +
                    Vector3.up * transform.position.y;
            }
        }

        private void NormalMoveY()
        {
            if (_movementType != MovementType.Y)
                return;
            
            if (Mathf.Abs(transform.position.x - CurrentCell.transform.position.x) > DistanceToCenter)
                return;

            transform.position += Vector3.up * (_inputVector.y * speed * Time.deltaTime);
            
            if (_targetCell == null &&
                Mathf.Sign((CurrentCell.transform.position - transform.position).y * _inputVector.y) < 0)
            {
                transform.position =
                    Vector3.up * CurrentCell.transform.position.y +
                    Vector3.right * transform.position.x;
            }
        }

        private void CornerMoveX()
        {
            if (_movementType != MovementType.Y)
                return;
            
            if (Mathf.Abs(transform.position.x - CurrentCell.transform.position.x) <= DistanceToCenter)
                return;

            if (_targetCell == null)
                return;
            
            transform.position += 
                Vector3.right * 
                (Mathf.Sign(CurrentCell.transform.position.x - transform.position.x) * _inputVector.magnitude * speed * Time.deltaTime);
        }

        private void CornerMoveY()
        {
            if (_movementType != MovementType.X)
                return;
            
            if (Mathf.Abs(transform.position.y - CurrentCell.transform.position.y) <= DistanceToCenter)
                return;

            if (_targetCell == null)
                return;
            
            transform.position += 
                Vector3.up * 
                (Mathf.Sign(CurrentCell.transform.position.y - transform.position.y) * _inputVector.magnitude * speed * Time.deltaTime);
        }


        private bool CheckWall() => _targetCell == null;


        private void UpdateCurrentCell()
        {
            if (CurrentDelta().magnitude < .5f) return;
            if (_targetCell == null) return;
            CurrentCell = _targetCell;
        }

        private Vector3 CurrentDelta()
        {
            return transform.position - CurrentCell.transform.position;
        }

        private void AlignToPath()
        {
            switch (_movementType)
            {
                case MovementType.X:
                    transform.position = Vector3.Lerp(
                        transform.position,
                        Vector3.right * transform.position.x + Vector3.up * CurrentCell.transform.position.y,
                        alignToPathSpeed * Time.deltaTime);
                    break;
                case MovementType.Y:
                    transform.position = Vector3.Lerp(
                        transform.position,
                        Vector3.up * transform.position.y + Vector3.right * CurrentCell.transform.position.x,
                        alignToPathSpeed * Time.deltaTime);
                    break;
                
            }
        }
    }
}
