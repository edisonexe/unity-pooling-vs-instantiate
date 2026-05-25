using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Infrastructure
{
    [AddComponentMenu("PoolingBenchmark/Infrastructure/Simulation Containers")]
    public sealed class SimulationContainers : MonoBehaviour, IInitializable
    {
        [Header("Hierarchy Folders")]
        [SerializeField] private Transform _projectileContainer;
        [SerializeField] private Transform _targetContainer;

        public Transform ProjectileContainer => _projectileContainer;
        public Transform TargetContainer => _targetContainer;
        
        public void Initialize()
        {
            ValidateContexts();
        }

        private void ValidateContexts()
        {
            if (!_projectileContainer) Debug.LogError("[SimulationContainers] Projectile Container Transform is unassigned in Inspector!", this);
            if (!_targetContainer) Debug.LogError("[SimulationContainers] Target Container Transform is unassigned in Inspector!", this);
        }
    }
}