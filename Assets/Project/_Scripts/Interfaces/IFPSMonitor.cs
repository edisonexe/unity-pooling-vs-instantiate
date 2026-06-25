using System;

namespace PoolingBenchmark.Interfaces
{
    public interface IFPSMonitor
    {
        event Action<int> OnFPSChanged;
    }
}