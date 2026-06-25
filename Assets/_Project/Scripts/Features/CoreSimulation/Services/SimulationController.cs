using System;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.PerformanceStats.Services;
using PoolingBenchmark.Features.Targets;
using PoolingBenchmark.Infrastructure.Pooling;

namespace PoolingBenchmark.Features.CoreSimulation.Services
{
    public sealed class SimulationController : ISimulationController
    {
        private readonly IEntityFactory _factory;
        private readonly StatsCollector _collector;
        private readonly TargetSpawner _spawner;
        private readonly PoolService _pools;

        private ExecutionMode _currentMode = ExecutionMode.Naive;

        public SimulationController(
            IEntityFactory factory, 
            StatsCollector collector, 
            TargetSpawner spawner, 
            PoolService pools)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
            _pools = pools ?? throw new ArgumentNullException(nameof(pools));
            
            ApplyMode(_currentMode);
        }

        public void ToggleMode()
        {
            _spawner.StopSpawning();
            _factory.Cleanup();

            _currentMode = _currentMode == ExecutionMode.Naive ? ExecutionMode.Pool : ExecutionMode.Naive;
            
            ApplyMode(_currentMode);
            _spawner.StartSpawning();
        }

        private void ApplyMode(ExecutionMode mode)
        {
            if (mode == ExecutionMode.Pool)
            {
                _pools.Prewarm();
            }

            _factory.SetMode(mode);
            _factory.ResetCounter();
            _collector.SetMode(mode);
        }
    }
}