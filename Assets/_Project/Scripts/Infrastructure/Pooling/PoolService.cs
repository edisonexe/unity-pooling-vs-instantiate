using System;
using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;

namespace PoolingBenchmark.Infrastructure.Pooling
{
    public sealed class PoolService
    {
        private readonly SimulationConfig _config;
        private readonly SimulationContainers _containers;
        private readonly ProjectileView _projectilePrefab;
        private readonly TargetView _targetPrefab;
        
        private bool _isPrewarmed;

        public MonoBehaviourObjectPool<ProjectileView> ProjectilePool { get; private set; }
        public MonoBehaviourObjectPool<TargetView> TargetPool { get; private set; }

        public PoolService(
            SimulationConfig config, 
            SimulationContainers containers, 
            ProjectileView projectilePrefab, 
            TargetView targetPrefab)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _containers = containers ?? throw new ArgumentNullException(nameof(containers));
            _projectilePrefab = projectilePrefab ?? throw new ArgumentNullException(nameof(projectilePrefab));
            _targetPrefab = targetPrefab ?? throw new ArgumentNullException(nameof(targetPrefab));
            
            ProjectilePool = new MonoBehaviourObjectPool<ProjectileView>(_projectilePrefab, _containers.ProjectileContainer);
            TargetPool = new MonoBehaviourObjectPool<TargetView>(_targetPrefab, _containers.TargetContainer);
        }

        public void Prewarm()
        {
            if (_isPrewarmed) return;

            ProjectilePool.Prewarm(_config.PrewarmCount);
            TargetPool.Prewarm(_config.PrewarmCount);
            _isPrewarmed = true;
        }

        public void ClearPools()
        {
            if (ProjectilePool != null) ProjectilePool.Clear();
            if (TargetPool != null) TargetPool.Clear();
            
            _isPrewarmed = false;
        }
    }
}