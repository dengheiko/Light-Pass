using System;
using UnityEngine;

namespace LightPassGame
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 1;

        public Cell CurrentCell { get; private set; }

        private Cell _targetCell;
        private Cell _previousCell;
        private float _moveDelta;
        

        public void InitCurrentCell(Cell cell)
        {
            CurrentCell = cell;
            transform.position = cell.transform.position;
        }

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
