using System;
using PoolingBenchmark.Domain.Configs;
using PoolingBenchmark.Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace PoolingBenchmark.Gameplay.Entities
{
    [AddComponentMenu("PoolingBenchmark/Entities/Plane Spawn Points Cache")]
    public sealed class PlaneSpawnPointsCache : MonoBehaviour, ISpawnPointsProvider, IInitializable
    {
        [Header("References")]
        [SerializeField] private Transform _planeTransform;
        
        private const float BASE_MESH_EXTENT = 3f;
        
        private SimulationConfig _config;
        private Vector3[] _cachedPoints;
        private int _cacheSize;
        
        [Inject]
        public void Init(SimulationConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        
        public void Initialize()
        {
            ValidateContexts();
            BakeSpawnPoints();
        }
        
        public Vector3 GetRandomSpawnPoint()
        {
            if (_cachedPoints is null || _cachedPoints.Length == 0)
            {
                return transform.position;
            }

            int randomIndex = Random.Range(0, _cacheSize);
            return _cachedPoints[randomIndex];
        }

        private void BakeSpawnPoints()
        {
            _cacheSize = _config.SpawnPointsCacheSize;
            _cachedPoints = new Vector3[_cacheSize];

            Vector3 scale = _planeTransform.localScale;
            
            float halfWidth = scale.x * BASE_MESH_EXTENT;
            float halfLength = scale.z * BASE_MESH_EXTENT;
            Vector3 planeOrigin = _planeTransform.position;
            
            for (int i = 0; i < _cacheSize; i++)
            {
                float randomX = Random.Range(-halfWidth, halfWidth);
                float randomZ = Random.Range(-halfLength, halfLength);

                _cachedPoints[i] = new Vector3(
                    planeOrigin.x + randomX, 
                    1f, 
                    planeOrigin.z + randomZ
                );
            }
        }

        private void ValidateContexts()
        {
            if (!_planeTransform)
            {
                Debug.LogError("[PlaneSpawnPointsCache] Target Plane Transform reference is missing in Inspector!", this);
            }
        }
    }
}