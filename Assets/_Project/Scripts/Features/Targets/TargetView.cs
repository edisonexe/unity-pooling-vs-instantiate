using UnityEngine;
using IPoolable = PoolingBenchmark.Infrastructure.Pooling.IPoolable;

namespace PoolingBenchmark.Features.Targets
{
    [AddComponentMenu("PoolingBenchmark/Views/Target View")]
    public sealed class TargetView : MonoBehaviour, IPoolable
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
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