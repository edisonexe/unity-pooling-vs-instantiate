using System;
using System.Collections.Generic;
using PoolingBenchmark.Domain;
using PoolingBenchmark.Gameplay.Entities;
using PoolingBenchmark.Interfaces;
using PoolingBenchmark.Enums;
using PoolingBenchmark.Infrastructure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PoolingBenchmark.Gameplay.Services
{
    public sealed class EntityFactory : IEntityFactory
    {
        private readonly PoolService _pools;
        private readonly EntityRegistry _registry;
        private readonly SimulationContainers _containers;
        
        private readonly Action<Projectile> _projectileReleaseCache;
        private readonly Action<Target> _targetReleaseCache;

        private readonly List<Projectile> _projectileCleanupBuffer = new(4096);
        private readonly List<Target> _targetCleanupBuffer = new(4096);

        private ExecutionMode _mode;
        private int _projNaiveCounter;
        private int _targetNaiveCounter;

        public int ProjNaiveCounter => _projNaiveCounter;
        public int TargetNaiveCounter => _targetNaiveCounter;

        public EntityFactory(PoolService pools, EntityRegistry registry, SimulationContainers containers)
        {
            _pools = pools ?? throw new ArgumentNullException(nameof(pools));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _containers = containers ?? throw new ArgumentNullException(nameof(containers));
            
            _projectileReleaseCache = ReleaseProjectile;
            _targetReleaseCache = ReleaseTarget;
        }

        public void SetMode(ExecutionMode mode) => _mode = mode;

        public void ResetCounter()
        {
            _projNaiveCounter = 0;
            _targetNaiveCounter = 0;
        }

        public Projectile CreateProjectile(Vector3 pos, Quaternion rot, Vector3 dir)
        {
            Projectile proj = _mode == ExecutionMode.Pool
                ? _pools.ProjectilePool.Get()
                : Object.Instantiate(_pools.ProjectilePool.Prefab, _containers.ProjectileContainer);

            if (_mode == ExecutionMode.Naive) 
                _projNaiveCounter++;

            proj.Init(pos, rot, dir, _projectileReleaseCache);
            proj.gameObject.SetActive(true);

            _registry.AddProjectile(proj);
            return proj;
        }

        public Target CreateTarget(Vector3 pos, Vector3 dir)
        {
            Target target = _mode == ExecutionMode.Pool
                ? _pools.TargetPool.Get()
                : Object.Instantiate(_pools.TargetPool.Prefab, _containers.TargetContainer);

            if (_mode == ExecutionMode.Naive) 
                _targetNaiveCounter++;

            target.Init(pos, dir, _targetReleaseCache);
            target.gameObject.SetActive(true);
            
            _registry.AddTarget(target);
            return target;
        }

        private void ReleaseProjectile(Projectile p)
        {
            _registry.RemoveProjectile(p);

            if (_mode == ExecutionMode.Pool) 
                _pools.ProjectilePool.Return(p);
            else 
                Object.Destroy(p.gameObject);
        }

        private void ReleaseTarget(Target t)
        {
            _registry.RemoveTarget(t);

            if (_mode == ExecutionMode.Pool) 
                _pools.TargetPool.Return(t);
            else 
                Object.Destroy(t.gameObject);
        }

        public void Cleanup()
        {
            _projectileCleanupBuffer.Clear();
            _targetCleanupBuffer.Clear();

            IReadOnlyList<Projectile> activeProjs = _registry.Projectiles;
            int projCount = activeProjs.Count;
            for (int i = 0; i < projCount; i++)
            {
                _projectileCleanupBuffer.Add(activeProjs[i]);
            }

            IReadOnlyList<Target> activeTargets = _registry.Targets;
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