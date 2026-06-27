using System;
using PoolingBenchmark.Infrastructure.Pooling;
using UnityEngine;

namespace PoolingBenchmark.Features.Projectiles
{
    public sealed class ProjectileEntity : IPocoPoolable
    {
        private readonly Action<ProjectileEntity> _onDespawn;
        
        private Vector3 _direction;
        private float _speed;
        private float _maxLifetime;
        private ProjectileView _view;

        private Vector3 _position;
        private float _currentLifetime;

        public bool IsDestroyed { get; private set; }
        
        public Vector3 Direction => _direction;
        public float Speed => _speed;
        public float CurrentLifetime => _currentLifetime;
        public float MaxLifetime => _maxLifetime;
        public Vector3 Position => _position;
        public ProjectileView View => _view;

        public bool IsExpired => _currentLifetime >= _maxLifetime;

        public ProjectileEntity(Action<ProjectileEntity> onDespawn)
        {
            _onDespawn = onDespawn ?? throw new ArgumentNullException(nameof(onDespawn));
        }

        public void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, float speed, float maxLifetime, ProjectileView view)
        {
            if (view == null)
            {
                Debug.LogError("[ProjectileEntity] ProjectileView passed into Initialize is NULL!");
                return;
            }

            _position = position;
            _direction = direction;
            _speed = speed;
            _maxLifetime = maxLifetime;
            _view = view;
            _currentLifetime = 0f;

            IsDestroyed = false;
            
            _view.Setup(_position, rotation);
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
                Debug.LogError("[ProjectileEntity] Missing ProjectileView reference during UpdatePosition!");
            }
        }

        public void AdvanceLifetime(float deltaTime)
        {
            _currentLifetime += deltaTime;
        }

        public void Despawn()
        {
            if (IsDestroyed) return;
            IsDestroyed = true;
            _onDespawn.Invoke(this);
        }

        public void Reset()
        {
            _view = null; 
            IsDestroyed = false;
        }
    }
}