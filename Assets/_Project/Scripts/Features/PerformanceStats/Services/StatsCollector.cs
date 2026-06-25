using System;
using PoolingBenchmark.Features.CoreSimulation;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Features.PerformanceStats.Interfaces;
using PoolingBenchmark.Features.PerformanceStats.Models;
using PoolingBenchmark.Infrastructure;
using PoolingBenchmark.Infrastructure.Pooling;
using Zenject;

namespace PoolingBenchmark.Features.PerformanceStats.Services
{
    public sealed class StatsCollector : IStatsProvider, ITickable, IDisposable
    {
        private const float UPDATE_INTERVAL = 0.1f;

        private readonly PoolService _pools;
        private readonly EntityRegistry _registry;
        private readonly IEntityFactory _entityFactory;
        
        private ExecutionMode _currentMode;
        private float _timer;
        private bool _isDirty;

        public event Action<SimulationStats> OnStatsChanged;

        public StatsCollector(PoolService pools, EntityRegistry registry, IEntityFactory entityFactory)
        {
            _pools = pools ?? throw new ArgumentNullException(nameof(pools));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));

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
            
            int totalProjs = isPool ? _pools.ProjectilePool.TotalCreated : _entityFactory.ProjNaiveCounter;
            int totalTargets = isPool ? _pools.TargetPool.TotalCreated : _entityFactory.TargetNaiveCounter;

            SimulationStats stats = new SimulationStats(
                _currentMode,
                _registry.Projectiles.Count,
                _registry.Targets.Count,
                totalProjs,                               
                totalTargets,                             
                _pools.ProjectilePool.TotalCreated,       
                _pools.TargetPool.TotalCreated,         
                _pools.ProjectilePool.AvailableCount,
                _pools.TargetPool.AvailableCount,
                _pools.ProjectilePool.ReusedCount,
                _pools.TargetPool.ReusedCount
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