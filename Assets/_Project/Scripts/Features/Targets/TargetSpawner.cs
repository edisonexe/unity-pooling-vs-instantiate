using System;
using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.Environment;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace PoolingBenchmark.Features.Targets
{
    public sealed class TargetSpawner : ITickable
    {
        private readonly IEntityFactory _factory;
        private readonly SimulationConfig _config;
        private readonly ISpawnPointsProvider _pointsProvider;
        
        private float _timer;
        private bool _isSpawning;

        public TargetSpawner(IEntityFactory factory, SimulationConfig config, ISpawnPointsProvider pointsProvider)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _pointsProvider = pointsProvider ?? throw new ArgumentNullException(nameof(pointsProvider));
        }

        public void StartSpawning()
        {
            _isSpawning = true;
            _timer = _config.SpawnInterval;
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        public void Tick()
        {
            if (!_isSpawning) return;

            _timer += Time.deltaTime;
            
            while (_timer >= _config.SpawnInterval)
            {
                _timer -= _config.SpawnInterval;
                SpawnRandomTarget();
            }
        }
        
        private void SpawnRandomTarget()
        {
            Vector3 spawnPosition = _pointsProvider.GetRandomSpawnPoint();
            
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            Vector3 randomDirection = new Vector3(randomCircle.x, 0f, randomCircle.y);

            _factory.CreateTarget(spawnPosition, randomDirection);
        }
    }
}