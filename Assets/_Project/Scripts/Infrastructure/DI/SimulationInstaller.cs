using PoolingBenchmark.Features.CoreSimulation;
using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Features.Environment;
using PoolingBenchmark.Features.PerformanceStats;
using PoolingBenchmark.Features.PerformanceStats.Services;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;
using PoolingBenchmark.Features.Weapons;
using PoolingBenchmark.Infrastructure.Pooling;
using PoolingBenchmark.UI;
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
        [SerializeField] private TurretView _turretView;
        [SerializeField] private PlaneSpawnPointsCache _spawnPointsCache;
        [SerializeField] private StressTestUIView _uiView;
        [SerializeField] private SimulationContainers _containers;

        public override void InstallBindings()
        {
            ValidateContexts();
            
            Container.BindInstance(_config).AsSingle();
            
            Container.Bind<EntityRegistry>().AsSingle();
            
            Container.Bind<IEntityFactory>().To<EntityFactory>().AsSingle();
            Container.Bind<ISimulationController>().To<SimulationController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<SimulationContainers>().FromInstance(_containers).AsSingle();
            Container.BindInterfacesAndSelfTo<PoolService>().FromInstance(_poolService).AsSingle();
            
            Container.BindInterfacesAndSelfTo<TurretView>().FromInstance(_turretView).AsSingle();
            
            Container.BindInterfacesTo<FPSMonitor>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<StressTestUIView>().FromInstance(_uiView).AsSingle();
            Container.BindInterfacesTo<PlaneSpawnPointsCache>().FromInstance(_spawnPointsCache).AsSingle();
            
            Container.BindInterfacesTo<StressTestUIPresenter>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<StatsCollector>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TurretSystem>().AsSingle();
            
            Container.BindInterfacesTo<SimulationStartup>().AsSingle().NonLazy();
        }

        private void ValidateContexts()
        {
            if (!_config) Debug.LogError("[SimulationInstaller] SimulationConfig Asset is missing!", this);
            if (!_containers) Debug.LogError("[SimulationInstaller] SimulationContainers reference is missing!", this);
            if (!_poolService) Debug.LogError("[SimulationInstaller] PoolSystem reference is missing!", this);
            if (!_turretView) Debug.LogError("[SimulationInstaller] TurretView reference is missing!", this);
            if (!_spawnPointsCache) Debug.LogError("[SimulationInstaller] PlaneSpawnPointsCache reference is missing!", this);
            if (!_uiView) Debug.LogError("[SimulationInstaller] StressTestUI reference is missing!", this);
        }
    }
}