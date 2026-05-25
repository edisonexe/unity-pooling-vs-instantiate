using System;
using PoolingBenchmark.Domain;

namespace PoolingBenchmark.Interfaces
{
    public interface IStatsProvider
    {
        event Action<SimulationStats> OnStatsChanged;
        void UpdateStats();
    }
}