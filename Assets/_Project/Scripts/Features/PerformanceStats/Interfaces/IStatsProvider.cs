using System;
using PoolingBenchmark.Features.PerformanceStats.Models;

namespace PoolingBenchmark.Features.PerformanceStats.Interfaces
{
    public interface IStatsProvider
    {
        event Action<SimulationStats> OnStatsChanged;
        void UpdateStats();
    }
}