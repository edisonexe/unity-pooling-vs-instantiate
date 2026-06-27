namespace PoolingBenchmark.Infrastructure.Pooling
{
    public interface IMonoBehaviourPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}