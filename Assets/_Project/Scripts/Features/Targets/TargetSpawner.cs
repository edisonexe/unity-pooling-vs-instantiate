using System;
using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Features.Environment;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace PoolingBenchmark.Features.Targets
{
    public sealed class TargetSpawner : ITickable
    {
        private readonly TargetSimulationFacade _targetSimulation;
        private readonly SimulationConfig _config;
        private readonly ISpawnPointsProvider _pointsProvider;
        
        private float _timer;
        private bool _isSpawning;

        public TargetSpawner(TargetSimulationFacade targetSimulation, SimulationConfig config, ISpawnPointsProvider pointsProvider)
        {
            _targetSimulation = targetSimulation ?? throw new ArgumentNullException(nameof(targetSimulation));
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
            
            _targetSimulation.Spawn(spawnPosition, randomDirection, _config.TargetSpeed);
        }
    }
}