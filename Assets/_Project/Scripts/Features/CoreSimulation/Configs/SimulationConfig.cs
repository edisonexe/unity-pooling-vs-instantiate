using UnityEngine;

namespace PoolingBenchmark.Features.CoreSimulation.Configs
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

        [Header("Target Options")]
        [SerializeField, Min(0.1f)] private float _targetSpeed = 5f;
        
        [Header("Projectile Options")]
        [SerializeField, Min(0.1f)] private float _projectileSpeed = 20f;
        [SerializeField, Min(0.1f)] private float _projectileMaxLifetime = 3f;

        [Header("Collision Settings")]
        [SerializeField, Min(0.01f)] private float _hitRadius = 0.75f;

        [Header("Spatial Grid Options")]
        [SerializeField, Min(0.5f)] private float _gridCellSize = 2.0f;
        
        public int PrewarmCount => _prewarmCount;
        public float SpawnInterval => _spawnInterval;
        public float FireRate => _fireRate;
        public float FovAngle => _fovAngle;
        public float HitRadius => _hitRadius;
        public float GridCellSize => _gridCellSize;
        public float TargetSpeed => _targetSpeed;
        public float ProjectileSpeed => _projectileSpeed;
        public float ProjectileMaxLifetime => _projectileMaxLifetime;
    }
}