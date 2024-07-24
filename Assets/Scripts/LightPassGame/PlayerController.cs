using System;
using UnityEngine;

namespace LightPassGame
{
    public class PlayerController : CellPosition
    {
        private const float InputThreshold = 0.1f;
        
        [SerializeField] private float speed = 1;

        [SerializeField] private float distanceToDetectWall = .6f;
        
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
            MoveX();
            MoveY();
            UpdateCurrentCell();
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

        private void MoveX()
        {
            if (_movementType != MovementType.X)
                return;

            transform.position += Vector3.right * (_inputVector.x * speed * Time.deltaTime);
            
            if (CheckWall() &&
                Mathf.Sign((CurrentCell.transform.position - transform.position).x * _inputVector.x) < 0)
            {
                transform.position =
                    Vector3.right * CurrentCell.transform.position.x +
                    Vector3.up * transform.position.y;
            }
        }

        private void MoveY()
        {
            if (_movementType != MovementType.Y)
                return;

            transform.position += Vector3.up * (_inputVector.y * speed * Time.deltaTime);
            
            if (CheckWall() &&
                Mathf.Sign((CurrentCell.transform.position - transform.position).y * _inputVector.y) < 0)
            {
                transform.position =
                    Vector3.up * CurrentCell.transform.position.y +
                    Vector3.right * transform.position.x;
            }
        }


        private bool CheckWall() => _targetCell == null;
        

        private void UpdateCurrentCell()
        {
            if (CurrentDelta().magnitude < .5f) return;
            CurrentCell = _targetCell;
        }

        private Vector3 CurrentDelta()
        {
            return transform.position - CurrentCell.transform.position;
        }
    }
}
