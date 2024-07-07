using System;
using UnityEngine;

namespace LightPassGame
{
    public class Enemy : CellPosition
    {
        [SerializeField] private float speed = 1;
        
        private Cell _targetCell;
        private Cell _previousCell;
        private float _moveDelta;
        
        private void Update()
        {
            PickNewTargetCell();
            MoveToTargetCell();
        }

        private void PickNewTargetCell()
        {
            if (_targetCell != null) return;
            _targetCell = CurrentCell.RandomNeighbour(_previousCell);
            _moveDelta = 0;
        }

        private void MoveToTargetCell()
        {
            if (_targetCell == null) return;
            
            _moveDelta += speed * Time.deltaTime;

            if (_moveDelta >= 1)
            {
                transform.position = _targetCell.transform.position;
                _previousCell = CurrentCell;
                CurrentCell = _targetCell;
                _targetCell = null;
                return;
            }

            transform.position = Vector3.Lerp(
                CurrentCell.transform.position,
                _targetCell.transform.position,
                _moveDelta);
        }
    }
}
