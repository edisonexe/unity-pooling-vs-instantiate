using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Features.Environment;
using PoolingBenchmark.Features.PerformanceStats.Services;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;
using PoolingBenchmark.Features.Weapons;
using PoolingBenchmark.Infrastructure.Collections;
using PoolingBenchmark.Infrastructure.Pooling;
using PoolingBenchmark.Scripts.UI.SimulationStatsPanel;
using PoolingBenchmark.UI.ControlPanel;
using PoolingBenchmark.UI.StartScreen;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Infrastructure.DI
{
    public sealed class SimulationInstaller : MonoInstaller
    {
        [Header("Configurations")]
        [SerializeField] private SimulationConfig _config;

        [Header("Scene Component Contexts")]
        [SerializeField] private PoolService _poolService;
        [SerializeField] private SimulationContainers _containers;
        [SerializeField] private TurretView _turretView;
        
        [Header("UI Views")]
        [SerializeField] private StartScreenView _startScreenView;
        [SerializeField] private ControlPanelView _controlPanelView;
        [SerializeField] private SimulationStatsView _simulationStatsView;
        
        [Header("Boundary")]
        [SerializeField] private ArenaBoundary _arenaBoundary;

        public override void InstallBindings()
        {
            ValidateContexts();

            Container.BindInstance(_config).AsSingle();
            Container.Bind<EntityRegistry>().AsSingle();

            Container.BindInterfacesAndSelfTo<ArenaBoundary>().FromInstance(_arenaBoundary).AsSingle();
            Container.Bind<SpatialGrid>().AsSingle().WithArguments(_config.GridCellSize);
            Container.BindInterfacesAndSelfTo<SimulationContainers>().FromInstance(_containers).AsSingle();
            Container.BindInterfacesAndSelfTo<PoolService>().FromInstance(_poolService).AsSingle();

            Container.Bind<IEntityFactory>().To<EntityFactory>().AsSingle();
            Container.Bind<ISimulationController>().To<SimulationController>().AsSingle();
            Container.Bind<TurretView>().FromInstance(_turretView).AsSingle();
            
            Container.Bind<StartScreenView>().FromInstance(_startScreenView).AsSingle();
            Container.Bind<ControlPanelView>().FromInstance(_controlPanelView).AsSingle();
            Container.Bind<SimulationStatsView>().FromInstance(_simulationStatsView).AsSingle();
            Container.BindInterfacesTo<StartScreenPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ControlPanelPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SimulationStatsPresenter>().AsSingle().NonLazy();
            
            Container.BindInterfacesTo<FPSMonitor>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TurretSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<StatsCollector>().AsSingle();
        }

        private void ValidateContexts()
        {
            if (!_config) Debug.LogError("[SimulationInstaller] SimulationConfig Asset is missing!", this);
            if (!_containers) Debug.LogError("[SimulationInstaller] SimulationContainers reference is missing!", this);
            if (!_poolService) Debug.LogError("[SimulationInstaller] PoolService reference is missing!", this);
            if (!_turretView) Debug.LogError("[SimulationInstaller] TurretView reference is missing!", this);
            if (!_startScreenView) Debug.LogError("[SimulationInstaller] StartScreenView reference is missing!", this);
            if (!_controlPanelView) Debug.LogError("[SimulationInstaller] ControlPanelView reference is missing!", this);
            if (!_simulationStatsView) Debug.LogError("[SimulationInstaller] SimulationStatsView reference is missing!", this);
            if (!_arenaBoundary) Debug.LogError("[SimulationInstaller] ArenaBoundary reference is missing!", this);
        }
    }
}