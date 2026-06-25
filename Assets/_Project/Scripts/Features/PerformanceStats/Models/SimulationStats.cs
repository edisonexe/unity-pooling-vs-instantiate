using PoolingBenchmark.Features.CoreSimulation;

namespace PoolingBenchmark.Features.PerformanceStats.Models
{
    public readonly struct SimulationStats
    {
        public ExecutionMode Mode { get; }
        public int ActiveProjs { get; }
        public int ActiveTargets { get; }
        public int TotalProjs { get; }
        public int TotalTargets { get; }
        public int ProjPoolSize { get; }
        public int TargetPoolSize { get; }
        public int ProjAvailable { get; }
        public int TargetAvailable { get; }
        public int ProjReused { get; }
        public int TargetReused { get; }

        public SimulationStats(ExecutionMode mode, int activeProjs, int activeTargets, int totalProjs, 
            int totalTargets, int projPoolSize, int targetPoolSize, int projAvailable, int targetAvailable, 
            int projReused, int targetReused)
        {
            Mode = mode;
            ActiveProjs = activeProjs;
            ActiveTargets = activeTargets;
            TotalProjs = totalProjs;
            TotalTargets = totalTargets;
            ProjPoolSize = projPoolSize;
            TargetPoolSize = targetPoolSize;
            ProjAvailable = projAvailable;
            TargetAvailable = targetAvailable;
            ProjReused = projReused;
            TargetReused = targetReused;
        }
    }
}