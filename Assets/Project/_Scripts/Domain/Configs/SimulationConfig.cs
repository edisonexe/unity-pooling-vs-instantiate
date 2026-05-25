using UnityEngine;

namespace PoolingBenchmark.Domain.Configs
{
    [CreateAssetMenu(fileName = "SimulationConfig", menuName = "PoolingBenchmark/Simulation Config")]
    public sealed class SimulationConfig : ScriptableObject
    {
        [Header("Pool Options")]
        [SerializeField] private int _prewarmCount = 500;

        [Header("Spawner Options")]
        [SerializeField] private float _spawnInterval = 0.5f;

        [Header("Turret Options")]
        [SerializeField] private float _fireRate = 0.01f;
        [SerializeField] private float _fovAngle = 120f;

        [Header("Spawn Points Options")]
        [SerializeField] private int _spawnPointsCacheSize = 100;
        
        public int PrewarmCount => _prewarmCount;
        public float SpawnInterval => _spawnInterval;
        public float FireRate => _fireRate;
        public float FovAngle => _fovAngle;
        public int SpawnPointsCacheSize => _spawnPointsCacheSize;
    }
}