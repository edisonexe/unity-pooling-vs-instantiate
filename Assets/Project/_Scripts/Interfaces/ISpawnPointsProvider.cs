using UnityEngine;

namespace PoolingBenchmark.Interfaces
{
    public interface ISpawnPointsProvider
    {
        Vector3 GetRandomSpawnPoint();
    }
}