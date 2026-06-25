using UnityEngine;

namespace PoolingBenchmark.Features.Environment
{
    [AddComponentMenu("PoolingBenchmark/Environment/Arena Boundary")]
    public sealed class ArenaBoundary : MonoBehaviour, ISpawnPointsProvider
    {
        [Header("Boundary Dimensions")]
        [SerializeField] private float _widthX = 60f;
        [SerializeField] private float _longtudeZ = 60f;

        [Header("Gizmos Settings")]
        [SerializeField] private Color _gizmoColor = Color.red;
        [SerializeField] private bool _showGizmos = true;
        
        public bool IsInside(Vector3 position)
        {
            float halfX = _widthX * 0.5f;
            float halfY = _longtudeZ * 0.5f;

            return position.x >= -halfX && position.x <= halfX &&
                   position.z >= -halfY && position.z <= halfY;
        }

        public Vector3 GetRandomSpawnPoint()
        {
            float halfX = _widthX * 0.5f;
            float halfZ = _longtudeZ * 0.5f;

            float randomX = Random.Range(-halfX, halfX);
            float randomZ = Random.Range(-halfZ, halfZ);

            return new Vector3(
                transform.position.x + randomX, 
                transform.position.y, 
                transform.position.z + randomZ
            );
        }
        
        private void OnDrawGizmos()
        {
            if (!_showGizmos) return;

            Gizmos.color = _gizmoColor;
            Vector3 size = new Vector3(_widthX, 1f, _longtudeZ);
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}