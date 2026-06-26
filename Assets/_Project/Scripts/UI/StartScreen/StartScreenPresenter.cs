using System;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using Zenject;

namespace PoolingBenchmark.UI.StartScreen
{
    public class StartScreenPresenter : IInitializable, IDisposable
    {
        private readonly StartScreenView _view;
        private readonly ISimulationController _controller;
        
        public StartScreenPresenter(StartScreenView startScreenView, ISimulationController simulationController)
        {
            _view = startScreenView ?? throw new ArgumentNullException(nameof(startScreenView));
            _controller = simulationController ?? throw new ArgumentNullException(nameof(simulationController));
        }

        public void Initialize()
        {
            _view.StartBenchmarkBtn.onClick.AddListener(OnStartClicked);
            
            _view.Show();
        }

        public void Dispose()
        {
            _view.StartBenchmarkBtn.onClick.RemoveListener(OnStartClicked);
        }

        private void OnStartClicked()
        {
            _view.Hide();
            _controller.StartSimulation();
        }
        
    }
}