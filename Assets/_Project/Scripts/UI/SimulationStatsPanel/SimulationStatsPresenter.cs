using System;
using PoolingBenchmark.Features.CoreSimulation;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.PerformanceStats.Interfaces;
using PoolingBenchmark.Features.PerformanceStats.Models;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.UI.SimulationStatsPanel
{
    public sealed class SimulationStatsPresenter : IInitializable, IDisposable
    {
        private readonly SimulationStatsView _view;
        private readonly IStatsProvider _statsProvider;
        private readonly IFPSMonitor _fpsMonitor;
        private readonly ISimulationController _controller;
        
        private ExecutionMode _cachedMode;
        private bool _isFirstUpdate = true;
        private bool _isActive;
        
        private static readonly string[] _modeDisplayStrings = { "Mode: Naive", "Mode: Pool" };
        
        private const string FPS_TEMPLATE = "FPS: {0}";
        
        private const string ACT_P_TEMPLATE = "Active Projectiles: {0}";
        private const string ACT_T_TEMPLATE = "Active Targets: {0}";
        private const string TOT_P_TEMPLATE = "Total Created Proj: {0}";
        private const string TOT_T_TEMPLATE = "Total Created Targets: {0}";
        
        private const string P_SIZE_TEMPLATE = "Proj Pool Size: {0}";
        private const string T_SIZE_TEMPLATE = "Target Pool Size: {0}";
        private const string P_AVAIL_TEMPLATE = "Available Proj: {0}";
        private const string T_AVAIL_TEMPLATE = "Available Targets: {0}";
        private const string P_REUSED_TEMPLATE = "Reused Proj: {0}";
        private const string T_REUSED_TEMPLATE = "Reused Targets: {0}";

        public SimulationStatsPresenter(
            SimulationStatsView view, 
            IStatsProvider statsProvider,
            IFPSMonitor fpsMonitor,
            ISimulationController controller)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _statsProvider = statsProvider ?? throw new ArgumentNullException(nameof(statsProvider));
            _fpsMonitor = fpsMonitor ?? throw new ArgumentNullException(nameof(fpsMonitor));
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public void Initialize()
        {
            _statsProvider.OnStatsChanged += OnStatsUpdated;
            _fpsMonitor.OnFPSChanged += OnFPSChanged;
            _controller.OnSimulationStarted += ActivateStatsPanel;
            
            _view.Hide();
        }

        public void Dispose()
        {
            if (_statsProvider != null) _statsProvider.OnStatsChanged -= OnStatsUpdated;
            if (_fpsMonitor != null) _fpsMonitor.OnFPSChanged -= OnFPSChanged;
            if (_controller != null) _controller.OnSimulationStarted -= ActivateStatsPanel;
        }

        private void ActivateStatsPanel()
        {
            _isActive = true;
            _view.Show();
            _statsProvider.UpdateStats();
        }
        
        private void OnStatsUpdated(SimulationStats stats)
        {
            if (!_isActive || !_view.ExecModeText) return;

            bool isModeChanged = _isFirstUpdate || _cachedMode != stats.Mode;

            if (isModeChanged)
            {
                _cachedMode = stats.Mode;
                _isFirstUpdate = false;

                _view.ExecModeText.text = _modeDisplayStrings[(int)stats.Mode];

                bool isPool = stats.Mode == ExecutionMode.Pool;
                
                ToggleGroup(_view.PoolOnlyRows, isPool);
                ToggleGroup(_view.NaiveOnlyRows, !isPool);
            }

            _view.ActiveProjsText.SetText(ACT_P_TEMPLATE, stats.ActiveProjs);
            _view.ActiveTargetsText.SetText(ACT_T_TEMPLATE, stats.ActiveTargets);

            if (_cachedMode == ExecutionMode.Pool)
            {
                _view.ProjsPoolSizeText.SetText(P_SIZE_TEMPLATE, stats.ProjPoolSize);
                _view.TargetsPoolSizeText.SetText(T_SIZE_TEMPLATE, stats.TargetPoolSize);
                _view.AvailableProjsText.SetText(P_AVAIL_TEMPLATE, stats.ProjAvailable);
                _view.AvailableTargetsText.SetText(T_AVAIL_TEMPLATE, stats.TargetAvailable);
                _view.ReusedProjsText.SetText(P_REUSED_TEMPLATE, stats.ProjReused);
                _view.ReusedTargetsText.SetText(T_REUSED_TEMPLATE, stats.TargetReused);
            }
            else
            {
                _view.TotalCreatedProjsText.SetText(TOT_P_TEMPLATE, stats.TotalProjs);
                _view.TotalCreatedTargetsText.SetText(TOT_T_TEMPLATE, stats.TotalTargets);
            }
        }

        private void OnFPSChanged(int fps)
        {
            if (!_view.FPSText) 
                return;

            _view.FPSText.SetText(FPS_TEMPLATE, fps);
        }
        
        private void ToggleGroup(GameObject[] objects, bool isActive)
        {
            if (objects == null) return;
            
            int count = objects.Length;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = objects[i];
                if (obj && obj.activeSelf != isActive)
                {
                    obj.SetActive(isActive);
                }
            }
        }
    }
}