using System;

namespace PoolingBenchmark.Features.PerformanceStats.Interfaces
{
    public interface IFPSMonitor
    {
        event Action<int> OnFPSChanged;
    }
}