using UnityEngine;

namespace PoolingBenchmark.Features.Weapons
{
    [AddComponentMenu("PoolingBenchmark/Views/Turret View")]
    public sealed class TurretView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _yawRoot;
        [SerializeField] private Transform _shootingPoint;

        public Transform YawRoot => _yawRoot;
        public Transform ShootingPoint => _shootingPoint;

        private void OnValidate()
        {
            if (!_yawRoot) Debug.LogError("[TurretView] YawRoot Transform is unassigned!", this);
            if (!_shootingPoint) Debug.LogError("[TurretView] ShootingPoint Transform is unassigned!", this);
        }
    }
}