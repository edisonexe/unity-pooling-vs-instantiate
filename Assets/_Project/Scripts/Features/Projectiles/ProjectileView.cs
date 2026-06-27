using UnityEngine;
using PoolingBenchmark.Infrastructure.Pooling;

namespace PoolingBenchmark.Features.Projectiles
{
    [AddComponentMenu("PoolingBenchmark/Views/Projectile View")]
    public sealed class ProjectileView : MonoBehaviour, IMonoBehaviourPoolable
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void Setup(Vector3 position, Quaternion rotation)
        {
            _transform.position = position;
            _transform.rotation = rotation;
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public void Show()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        public void OnSpawn() => Show();
        public void OnDespawn() => Hide();
    }
}