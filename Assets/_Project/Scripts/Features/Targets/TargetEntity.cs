using System;
using PoolingBenchmark.Infrastructure.Pooling;
using UnityEngine;

namespace PoolingBenchmark.Features.Targets
{
    public sealed class TargetEntity : IPocoPoolable
    {
        private readonly Action<TargetEntity> _onDespawn;
        
        private Vector3 _position;
        private Vector3 _moveDirection;
        private float _speed;
        private TargetView _view; 

        
        public bool IsDead { get; private set; }
        
        public Vector3 Position => _position;
        public Vector3 MoveDirection => _moveDirection;
        public float Speed => _speed;
        public TargetView View => _view;

        public TargetEntity(Action<TargetEntity> onDespawn)
        {
            _onDespawn = onDespawn ?? throw new ArgumentNullException(nameof(onDespawn));
        }

        public void Initialize(Vector3 position, Vector3 moveDirection, float speed, TargetView view)
        {
            if (view == null)
            {
                Debug.LogError("[TargetEntity] TargetView passed into Initialize is NULL!");
                return;
            }

            _position = position;
            _moveDirection = moveDirection;
            _speed = speed;
            _view = view;
            
            IsDead = false;
            
            _view.SetPosition(_position);
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            _position = newPosition;

            if (_view != null)
            {
                _view.SetPosition(_position);
            }
            else
            {
                Debug.LogError("[TargetEntity] Missing TargetView reference during UpdatePosition!");
            }
        }

        public void Despawn()
        {
            if (IsDead) return;
            IsDead = true;
            _onDespawn.Invoke(this);
        }

        public void Reset()
        {
            _view = null;
            IsDead = false;
        }
    }
}