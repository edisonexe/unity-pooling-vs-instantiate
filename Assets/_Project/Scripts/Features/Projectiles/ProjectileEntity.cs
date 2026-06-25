using System;
using UnityEngine;

namespace PoolingBenchmark.Features.Projectiles
{
    public sealed class ProjectileEntity
    {
        private readonly Action<ProjectileEntity> _onDespawn;
        private readonly Vector3 _direction;
        private readonly float _speed;
        private readonly float _maxLifetime;
        private readonly ProjectileView _view;

        private Vector3 _position;
        private float _currentLifetime;

        public Vector3 Direction => _direction;
        public float Speed => _speed;
        public float CurrentLifetime => _currentLifetime;
        public float MaxLifetime => _maxLifetime;
        public Vector3 Position => _position;
        public ProjectileView View => _view;

        public bool IsExpired => _currentLifetime >= _maxLifetime;

        public ProjectileEntity(
            Vector3 position, 
            Quaternion rotation, 
            Vector3 direction, 
            float speed, 
            float maxLifetime, 
            ProjectileView view, 
            Action<ProjectileEntity> onDespawn)
        {
            _position = position;
            _direction = direction;
            _speed = speed;
            _maxLifetime = maxLifetime;
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _onDespawn = onDespawn ?? throw new ArgumentNullException(nameof(onDespawn));

            _view.Setup(_position, rotation);
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            _position = newPosition;
            _view.SetPosition(_position);
        }

        public void AdvanceLifetime(float deltaTime)
        {
            _currentLifetime += deltaTime;
        }

        public void Despawn()
        {
            _onDespawn.Invoke(this);
        }
    }
}