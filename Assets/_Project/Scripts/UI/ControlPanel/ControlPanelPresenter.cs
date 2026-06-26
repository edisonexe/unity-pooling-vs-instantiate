using System;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using Zenject;

namespace PoolingBenchmark.UI.ControlPanel
{
    public class ControlPanelPresenter : IInitializable, IDisposable
    {
        private readonly ControlPanelView _view;
        private readonly ISimulationController _controller;
        
        public ControlPanelPresenter(ControlPanelView view, ISimulationController controller)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public void Initialize()
        {
            _view.ToggleModeBtn.onClick.AddListener(_controller.ToggleMode);
            _controller.OnSimulationStarted += HandleSimulationStarted;
            
            _view.Hide();
        }

        public void Dispose()
        {
            if (_view && _view.ToggleModeBtn)
            {
                _view.ToggleModeBtn.onClick.RemoveListener(_controller.ToggleMode);
            }
            
            if (_controller != null)
            {
                _controller.OnSimulationStarted -= HandleSimulationStarted;
            }
        }

        private void HandleSimulationStarted()
        {
            _view.Show();
        }
    }
}