using UnityEngine;

namespace PoolingBenchmark.Domain.Configs
{
    [CreateAssetMenu(fileName = "SimulationConfig", menuName = "PoolingBenchmark/Simulation Config")]
    public sealed class SimulationConfig : ScriptableObject
    {
        [Header("Pool Options")]
        [SerializeField, Min(1)] private int _prewarmCount = 500;

        [Header("Spawner Options")]
        [SerializeField, Min(0.001f)] private float _spawnInterval = 0.5f;

        [Header("Turret Options")]
        [SerializeField, Min(0.0001f)] private float _fireRate = 0.01f;
        [SerializeField, Range(0f, 360f)] private float _fovAngle = 120f;

        [Header("Spawn Points Options")]
        [SerializeField, Min(1)] private int _spawnPointsCacheSize = 100;
        
        public int PrewarmCount => _prewarmCount;
        public float SpawnInterval => _spawnInterval;
        public float FireRate => _fireRate;
        public float FovAngle => _fovAngle;
        public int SpawnPointsCacheSize => _spawnPointsCacheSize;
    }
}