using System;
using System.Collections.Generic;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;
using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Infrastructure.Pooling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PoolingBenchmark.Features.CoreSimulation.Services
{
    public sealed class EntityFactory : IEntityFactory
    {
        private readonly PoolService _pools;
        private readonly EntityRegistry _registry;
        private readonly SimulationContainers _containers;
        private readonly SimulationConfig _config;
        
        private readonly Action<ProjectileEntity> _projectileReleaseCache;
        private readonly Action<TargetEntity> _targetReleaseCache;

        private readonly List<ProjectileEntity> _projectileCleanupBuffer = new(4096);
        private readonly List<TargetEntity> _targetCleanupBuffer = new(4096);

        private ExecutionMode _mode;
        private int _projNaiveCounter;
        private int _targetNaiveCounter;

        public int ProjNaiveCounter => _projNaiveCounter;
        public int TargetNaiveCounter => _targetNaiveCounter;

        public EntityFactory(PoolService pools, EntityRegistry registry, SimulationContainers containers, SimulationConfig config)
        {
            _pools = pools ?? throw new ArgumentNullException(nameof(pools));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _containers = containers ?? throw new ArgumentNullException(nameof(containers));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            
            _projectileReleaseCache = ReleaseProjectile;
            _targetReleaseCache = ReleaseTarget;
        }

        public void SetMode(ExecutionMode mode) => _mode = mode;

        public void ResetCounter()
        {
            _projNaiveCounter = 0;
            _targetNaiveCounter = 0;
        }

        public ProjectileEntity CreateProjectile(Vector3 pos, Quaternion rot, Vector3 dir)
        {
            ProjectileView view;

            if (_mode == ExecutionMode.Pool)
            {
                view = _pools.ProjectilePool.Get();
            }
            else
            {
                view = Object.Instantiate(_pools.ProjectilePool.Prefab, _containers.ProjectileContainer);
                _projNaiveCounter++;
                view.Show();
            }

            ProjectileEntity projectileEntity = new ProjectileEntity(
                pos,
                rot,
                dir,
                _config.ProjectileSpeed,
                _config.ProjectileMaxLifetime,
                view,
                _projectileReleaseCache
            );

            _registry.AddProjectile(projectileEntity);
            return projectileEntity;
        }
        
        public TargetEntity CreateTarget(Vector3 pos, Vector3 dir)
        {
            TargetView view;

            if (_mode == ExecutionMode.Pool)
            {
                view = _pools.TargetPool.Get();
            }
            else
            {
                view = Object.Instantiate(_pools.TargetPool.Prefab, _containers.TargetContainer);
                _targetNaiveCounter++;
                view.Show();
            }

            TargetEntity targetEntity = new TargetEntity(
                pos, 
                dir, 
                _config.TargetSpeed, 
                view, 
                _targetReleaseCache
            );
            
            _registry.AddTarget(targetEntity);
            return targetEntity;
        }

        private void ReleaseProjectile(ProjectileEntity p)
        {
            if (p == null) return;
            _registry.RemoveProjectile(p);

            if (_mode == ExecutionMode.Pool) 
            {
                _pools.ProjectilePool.Return(p.View);
            }
            else 
            {
                if (p.View != null)
                {
                    Object.Destroy(p.View.gameObject);
                }
            }
        }

        private void ReleaseTarget(TargetEntity t)
        {
            if (t == null) return;

            _registry.RemoveTarget(t);

            if (_mode == ExecutionMode.Pool) 
            {
                _pools.TargetPool.Return(t.View);
            }
            else 
            {
                if (t.View != null)
                {
                    Object.Destroy(t.View.gameObject);
                }
            }
        }

        public void Cleanup()
        {
            _projectileCleanupBuffer.Clear();
            _targetCleanupBuffer.Clear();

            IReadOnlyList<ProjectileEntity> activeProjs = _registry.Projectiles;
            int projCount = activeProjs.Count;
            for (int i = 0; i < projCount; i++)
            {
                _projectileCleanupBuffer.Add(activeProjs[i]);
            }

            IReadOnlyList<TargetEntity> activeTargets = _registry.Targets;
            int targetCount = activeTargets.Count;
            for (int i = 0; i < targetCount; i++)
            {
                _targetCleanupBuffer.Add(activeTargets[i]);
            }
            
            int cachedProjBufferCount = _projectileCleanupBuffer.Count;
            for (int i = 0; i < cachedProjBufferCount; i++)
            {
                ReleaseProjectile(_projectileCleanupBuffer[i]);
            }

            int cachedTargetBufferCount = _targetCleanupBuffer.Count;
            for (int i = 0; i < cachedTargetBufferCount; i++)
            {
                ReleaseTarget(_targetCleanupBuffer[i]);
            }

            _registry.Clear();
        }
    }
}