using System;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.PerformanceStats.Services;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;
using PoolingBenchmark.Infrastructure.Pooling;

namespace PoolingBenchmark.Features.CoreSimulation.Services
{
    public sealed class SimulationController : ISimulationController
    {
        private readonly TargetSimulationFacade _targetSimulation;
        private readonly ProjectileSimulationFacade _projectileSimulation;
        private readonly StatsCollector _collector;
        private readonly TargetSpawner _spawner;
        private readonly PoolService _pools;

        private ExecutionMode _currentMode = ExecutionMode.Naive;
        private bool _isSimulationStarted;

        public event Action OnSimulationStarted;
        public bool IsSimulationStarted => _isSimulationStarted;

        public SimulationController(
            TargetSimulationFacade targetSimulation,
            ProjectileSimulationFacade projectileSimulation,
            StatsCollector collector, 
            TargetSpawner spawner, 
            PoolService pools)
        {
            _targetSimulation = targetSimulation ?? throw new ArgumentNullException(nameof(targetSimulation));
            _projectileSimulation = projectileSimulation ?? throw new ArgumentNullException(nameof(projectileSimulation));
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
            _pools = pools ?? throw new ArgumentNullException(nameof(pools));
            
            ApplyMode(_currentMode);
        }

        public void StartSimulation()
        {
            if (_isSimulationStarted) return;
            
            _isSimulationStarted = true;
            _spawner.StartSpawning();
            
            OnSimulationStarted?.Invoke();
        }
        
        public void ToggleMode()
        {
            _spawner.StopSpawning();

            _projectileSimulation.PocoPool.Clear();
            _targetSimulation.PocoPool.Clear();
            _pools.ClearPools();

            _currentMode = _currentMode == ExecutionMode.Naive ? ExecutionMode.Pool : ExecutionMode.Naive;
            ApplyMode(_currentMode);
            
            if (_isSimulationStarted) _spawner.StartSpawning();
        }

        private void ApplyMode(ExecutionMode mode)
        {
            if (mode == ExecutionMode.Pool)
            {
                _pools.Prewarm();
            }

            _targetSimulation.SetMode(mode);
            _targetSimulation.ResetCounter();
            _projectileSimulation.SetMode(mode);
            _projectileSimulation.ResetCounter();
            
            _collector.SetMode(mode);
        }
    }
}