using System;

namespace PoolingBenchmark.Features.CoreSimulation.Interfaces
{
    public interface ISimulationController
    {
        event Action OnSimulationStarted;
        bool IsSimulationStarted { get; }
        
        void StartSimulation();
        void ToggleMode();
    }
}