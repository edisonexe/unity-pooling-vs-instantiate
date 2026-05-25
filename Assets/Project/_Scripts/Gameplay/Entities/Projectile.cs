using System;
using PoolingBenchmark.Interfaces;
using UnityEngine;

namespace PoolingBenchmark.Gameplay.Entities
{
    [AddComponentMenu("PoolingBenchmark/Entities/Projectile")]
    [RequireComponent(typeof(Collider))]
    public sealed class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _maxLifetime = 3f;
        
        private float _currentLifetime;
        private Vector3 _direction;
        private Action<Projectile> _onDespawn;
        
        private const string TARGET_TAG = "Target";
        
        public float Speed => _speed;
        public Vector3 Direction => _direction;
        public float CurrentLifetime { get => _currentLifetime; set => _currentLifetime = value; }
        public float MaxLifetime => _maxLifetime;
        
        public void Init(Vector3 position, Quaternion rotation, Vector3 dir, Action<Projectile> onDespawn)
        {
            transform.position = position;
            transform.rotation = rotation;
            _direction = dir;
            _onDespawn = onDespawn;
            
            ValidateContexts();
        }
        
        public void OnSpawn() => _currentLifetime = 0f;

        public void OnDespawn() => _onDespawn = null;

        public void Despawn() => _onDespawn?.Invoke(this);

        private void OnTriggerEnter(Collider other)
        {
            if (_onDespawn is null) return;
            
            if (other.CompareTag(TARGET_TAG)) 
            {
                Despawn();
            }
        }
        
        private void ValidateContexts()
        {
            if (_onDespawn == null) Debug.LogError("[Projectile] onDespawn Action is missing!", this);
        }
    }
}