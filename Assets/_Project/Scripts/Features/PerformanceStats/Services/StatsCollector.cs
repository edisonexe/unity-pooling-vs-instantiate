using System;
using PoolingBenchmark.Features.CoreSimulation;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Features.PerformanceStats.Interfaces;
using PoolingBenchmark.Features.PerformanceStats.Models;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;
using PoolingBenchmark.Infrastructure.Pooling;
using Zenject;

namespace PoolingBenchmark.Features.PerformanceStats.Services
{
    public sealed class StatsCollector : IStatsProvider, ITickable, IDisposable
    {
        private const float UPDATE_INTERVAL = 0.25f;

        private readonly PoolService _pools;
        private readonly EntityRegistry _registry;
        private readonly TargetSimulationFacade _targetSimulation;
        private readonly ProjectileSimulationFacade _projectileSimulation;
        
        private ExecutionMode _currentMode;
        private float _timer;
        private bool _isDirty;

        public event Action<SimulationStats> OnStatsChanged;

        public StatsCollector(
            PoolService pools, 
            EntityRegistry registry, 
            TargetSimulationFacade targetSimulation, 
            ProjectileSimulationFacade projectileSimulation)
        {
            _pools = pools ?? throw new ArgumentNullException(nameof(pools));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _targetSimulation = targetSimulation ?? throw new ArgumentNullException(nameof(targetSimulation));
            _projectileSimulation = projectileSimulation ?? throw new ArgumentNullException(nameof(projectileSimulation));

            _registry.OnChanged += SetDirty;
        }

        public void SetMode(ExecutionMode mode)
        {
            _currentMode = mode;
            SetDirty();
        }

        public void Tick()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            _timer += deltaTime;

            if (_isDirty && _timer >= UPDATE_INTERVAL)
            {
                UpdateStats();
                _timer = 0f;
                _isDirty = false;
            }
        }

        public void UpdateStats()
        {
            bool isPool = _currentMode == ExecutionMode.Pool;
            
            int totalProjs = isPool ? _projectileSimulation.TotalPoolSize : _projectileSimulation.NaiveCounter;
            int totalTargets = isPool ? _targetSimulation.TotalPoolSize : _targetSimulation.NaiveCounter;

            int availableProjs = isPool ? _projectileSimulation.AvailableCount : 0;
            int availableTargets = isPool ? _targetSimulation.AvailableCount : 0;
            
            int reusedProjs = isPool && _pools.ProjectilePool != null ? _pools.ProjectilePool.ReusedCount : 0;
            int reusedTargets = isPool && _pools.TargetPool != null ? _pools.TargetPool.ReusedCount : 0;

            SimulationStats stats = new SimulationStats(
                _currentMode,
                _registry.Projectiles.Count,
                _registry.Targets.Count,
                totalProjs,                               
                totalTargets,                             
                totalProjs,       
                totalTargets,     
                availableProjs,    
                availableTargets,  
                reusedProjs,       
                reusedTargets      
            );

            OnStatsChanged?.Invoke(stats);
        }

        private void SetDirty() => _isDirty = true;

        public void Dispose()
        {
            if (_registry != null) _registry.OnChanged -= SetDirty;
        }
    }
}