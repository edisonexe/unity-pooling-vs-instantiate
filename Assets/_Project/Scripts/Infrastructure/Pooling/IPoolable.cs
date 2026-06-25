namespace PoolingBenchmark.Infrastructure.Pooling
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}