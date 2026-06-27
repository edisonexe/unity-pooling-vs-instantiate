using System;
using PoolingBenchmark.Features.CoreSimulation;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Infrastructure.Pooling;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Features.Projectiles
{
    public sealed class ProjectileSimulationFacade
    {
        private readonly PocoObjectPool<ProjectileEntity> _projectilePocoPool;
        private readonly PoolService _viewPools;
        private readonly EntityRegistry _registry;
        private readonly SimulationContainers _containers;
        private readonly IEntityFactory _factory;
        private readonly ProjectileView _prefab;
        private readonly int _prewarmCount;

        private ExecutionMode _mode;
        private int _naiveCounter;

        public int NaiveCounter => _naiveCounter;
        public int TotalPoolSize => _projectilePocoPool.TotalCreated;
        public int AvailableCount => _projectilePocoPool.AvailableCount;
        public PocoObjectPool<ProjectileEntity> PocoPool => _projectilePocoPool;

        public ProjectileSimulationFacade(
            IEntityFactory factory,
            PoolService viewPools,
            EntityRegistry registry,
            SimulationContainers containers,
            ProjectileView prefab,
            int prewarmCount)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _viewPools = viewPools ?? throw new ArgumentNullException(nameof(viewPools));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _containers = containers ?? throw new ArgumentNullException(nameof(containers));
            _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            _prewarmCount = prewarmCount;

            _projectilePocoPool = new PocoObjectPool<ProjectileEntity>(_factory.CreateProjectile, _prewarmCount);

            if (_factory is EntityFactory concreteFactory)
            {
                concreteFactory.RegisterProjectileRecycle(RecycleProjectile);
            }
        }

        public void Prewarm() => _projectilePocoPool.Prewarm(_prewarmCount);

        public void SetMode(ExecutionMode mode) => _mode = mode;
        public void ResetCounter() => _naiveCounter = 0;

        public ProjectileEntity Spawn(Vector3 pos, Quaternion rot, Vector3 dir, float speed, float maxLifetime)
        {
            ProjectileView view;

            if (_mode == ExecutionMode.Pool)
            {
                view = _viewPools.ProjectilePool.Get();
            }
            else
            {
                view = UnityEngine.Object.Instantiate(_prefab, _containers.ProjectileContainer);
                _naiveCounter++;
                view.Show();
            }

            ProjectileEntity entity = _projectilePocoPool.Get();
            entity.Initialize(pos, rot, dir, speed, maxLifetime, view);
            
            _registry.AddProjectile(entity);
            return entity;
        }

        public void RecycleProjectile(ProjectileEntity p)
        {
            if (p == null) return;
            _registry.RemoveProjectile(p);

            if (_mode == ExecutionMode.Pool)
            {
                _viewPools.ProjectilePool.Return(p.View);
            }
            else
            {
                if (p.View != null) UnityEngine.Object.Destroy(p.View.gameObject);
            }

            _projectilePocoPool.Return(p);
        }
    }
}