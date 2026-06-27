using PoolingBenchmark.Infrastructure.Pooling;
using UnityEngine;

namespace PoolingBenchmark.Features.Targets
{
    [AddComponentMenu("PoolingBenchmark/Views/Target View")]
    public sealed class TargetView : MonoBehaviour, IMonoBehaviourPoolable
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