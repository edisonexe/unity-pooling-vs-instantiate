using System;
using UnityEngine;

namespace PoolingBenchmark.Features.Targets
{
    public sealed class TargetEntity
    {
        private readonly Action<TargetEntity> _onDespawn;
        
        private Vector3 _position;
        private readonly Vector3 _moveDirection;
        private readonly float _speed;
        
        private readonly TargetView _view; 

        public Vector3 Position => _position;
        public Vector3 MoveDirection => _moveDirection;
        public float Speed => _speed;
        public TargetView View => _view;

        public TargetEntity(Vector3 position, Vector3 moveDirection, float speed, TargetView view, Action<TargetEntity> onDespawn)
        {
            _position = position;
            _moveDirection = moveDirection;
            _speed = speed;
            _view = view;
            _onDespawn = onDespawn ?? throw new ArgumentNullException(nameof(onDespawn));
            
            _view.SetPosition(_position);
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            _position = newPosition;
            _view.SetPosition(_position);
        }

        public void Despawn()
        {
            _onDespawn.Invoke(this);
        }
    }
}