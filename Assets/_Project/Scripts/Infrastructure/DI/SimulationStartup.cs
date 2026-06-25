using System;
using PoolingBenchmark.Features.Targets;
using Zenject;

namespace PoolingBenchmark.Infrastructure.DI
{
    public sealed class SimulationStartup : IInitializable
    {
        private readonly TargetSpawner _targetSpawner;

        public SimulationStartup(TargetSpawner targetSpawner)
        {
            _targetSpawner = targetSpawner ?? throw new ArgumentNullException(nameof(targetSpawner));
        }

        public void Initialize()
        {
            _targetSpawner.StartSpawning();
        }
    }
}