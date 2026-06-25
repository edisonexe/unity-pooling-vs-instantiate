using UnityEngine;

namespace PoolingBenchmark.Features.Environment
{
    public interface ISpawnPointsProvider
    {
        Vector3 GetRandomSpawnPoint();
    }
}