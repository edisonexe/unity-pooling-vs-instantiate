using System;
using PoolingBenchmark.Features.CoreSimulation;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Infrastructure.Pooling;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Features.Targets
{
    public sealed class TargetSimulationFacade : IInitializable
    {
        private readonly PocoObjectPool<TargetEntity> _targetPocoPool;
        private readonly PoolService _viewPools;
        private readonly EntityRegistry _registry;
        private readonly SimulationContainers _containers;
        private readonly IEntityFactory _factory;
        private readonly int _prewarmCount;

        private ExecutionMode _mode;
        private int _naiveCounter;

        public int NaiveCounter => _naiveCounter;
        public int TotalPoolSize => _targetPocoPool.TotalCreated;
        public int AvailableCount => _targetPocoPool.AvailableCount;
        public PocoObjectPool<TargetEntity> PocoPool => _targetPocoPool;

        public TargetSimulationFacade(
            IEntityFactory factory,
            PoolService viewPools,
            EntityRegistry registry,
            SimulationContainers containers,
            int prewarmCount)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _viewPools = viewPools ?? throw new ArgumentNullException(nameof(viewPools));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _containers = containers ?? throw new ArgumentNullException(nameof(containers));
            _prewarmCount = prewarmCount;

            _targetPocoPool = new PocoObjectPool<TargetEntity>(_factory.CreateTarget, _prewarmCount);

            if (_factory is EntityFactory concreteFactory)
            {
                concreteFactory.RegisterTargetRecycle(RecycleTarget);
            }
        }

        public void Initialize()
        {
            _targetPocoPool.Prewarm(_prewarmCount);
        }

        public void SetMode(ExecutionMode mode) => _mode = mode;
        public void ResetCounter() => _naiveCounter = 0;

        public TargetEntity Spawn(Vector3 pos, Vector3 dir, float speed)
        {
            TargetView view;

            if (_mode == ExecutionMode.Pool)
            {
                view = _viewPools.TargetPool.Get();
            }
            else
            {
                view = UnityEngine.Object.Instantiate(_viewPools.TargetPool.Prefab, _containers.TargetContainer);
                _naiveCounter++;
                view.Show();
            }

            TargetEntity entity = _targetPocoPool.Get();
            entity.Initialize(pos, dir, speed, view);
            
            _registry.AddTarget(entity);
            return entity;
        }

        public void RecycleTarget(TargetEntity t)
        {
            if (t == null) return;
            _registry.RemoveTarget(t);

            if (_mode == ExecutionMode.Pool)
            {
                _viewPools.TargetPool.Return(t.View);
            }
            else
            {
                if (t.View != null) UnityEngine.Object.Destroy(t.View.gameObject);
            }

            _targetPocoPool.Return(t);
        }
    }
}