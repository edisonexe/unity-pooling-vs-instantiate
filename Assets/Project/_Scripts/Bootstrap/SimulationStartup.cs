using System;
using PoolingBenchmark.Gameplay.Services;
using Zenject;

namespace PoolingBenchmark.Bootstrap
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