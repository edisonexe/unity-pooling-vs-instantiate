using System;
using PoolingBenchmark.Features.CoreSimulation;
using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Infrastructure.Pooling
{
    [AddComponentMenu("PoolingBenchmark/Systems/Pool System")]
    public sealed class PoolService : MonoBehaviour, IInitializable
    {
        [Header("Prefabs")]
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Target _targetPrefab;
        
        private SimulationConfig _config;
        private SimulationContainers _containers;
        private bool _isPrewarmed;

        public ObjectPool<Projectile> ProjectilePool { get; private set; }
        public ObjectPool<Target> TargetPool { get; private set; }

        [Inject]
        public void Init(SimulationConfig config, SimulationContainers containers)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _containers = containers ?? throw new ArgumentNullException(nameof(containers));
        }
        
        public void Initialize()
        {
            ValidateContexts();
            
            ProjectilePool = new ObjectPool<Projectile>(_projectilePrefab, _containers.ProjectileContainer);
            TargetPool = new ObjectPool<Target>(_targetPrefab, _containers.TargetContainer);
        }

        public void Prewarm()
        {
            if (_isPrewarmed) return;

            ProjectilePool.Prewarm(_config.PrewarmCount);
            TargetPool.Prewarm(_config.PrewarmCount);
            _isPrewarmed = true;
        }
        
        private void ValidateContexts()
        {
            if (!_projectilePrefab) Debug.LogError("[PoolSystem] Projectile Prefab is missing in Inspector!", this);
            if (!_targetPrefab) Debug.LogError("[PoolSystem] Target Prefab is missing in Inspector!", this);
        }
    }
}