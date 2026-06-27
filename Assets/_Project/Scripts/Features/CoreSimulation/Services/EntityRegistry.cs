using System;
using System.Collections.Generic;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;

namespace PoolingBenchmark.Features.CoreSimulation.Services
{
    public sealed class EntityRegistry
    {
        public event Action OnChanged;

        private readonly List<ProjectileEntity> _projectiles = new(4096);
        private readonly List<TargetEntity> _targets = new(4096);

        private readonly Dictionary<ProjectileEntity, int> _projectileIndices = new(4096);
        private readonly Dictionary<TargetEntity, int> _targetIndices = new(4096);

        public IReadOnlyList<ProjectileEntity> Projectiles => _projectiles;
        public IReadOnlyList<TargetEntity> Targets => _targets;

        public void AddProjectile(ProjectileEntity p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (_projectileIndices.ContainsKey(p)) return;

            _projectiles.Add(p);
            _projectileIndices.Add(p, _projectiles.Count - 1);
            OnChanged?.Invoke();
        }

        public void RemoveProjectile(ProjectileEntity p)
        {
            if (p == null || !_projectileIndices.TryGetValue(p, out int index)) return;

            int lastIndex = _projectiles.Count - 1;
            if (index != lastIndex)
            {
                ProjectileEntity lastElement = _projectiles[lastIndex];
                _projectiles[index] = lastElement;
                _projectileIndices[lastElement] = index;
            }

            _projectiles.RemoveAt(lastIndex);
            _projectileIndices.Remove(p);
            OnChanged?.Invoke();
        }

        public void AddTarget(TargetEntity t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            if (_targetIndices.ContainsKey(t)) return;

            _targets.Add(t);
            _targetIndices.Add(t, _targets.Count - 1);
            OnChanged?.Invoke();
        }

        public void RemoveTarget(TargetEntity t)
        {
            if (t == null || !_targetIndices.TryGetValue(t, out int index)) return;

            int lastIndex = _targets.Count - 1;
            if (index != lastIndex)
            {
                TargetEntity lastElement = _targets[lastIndex];
                _targets[index] = lastElement;
                _targetIndices[lastElement] = index;
            }

            _targets.RemoveAt(lastIndex);
            _targetIndices.Remove(t);
            OnChanged?.Invoke();
        }

        public void Clear(Action<ProjectileEntity> disposeProjectile, Action<TargetEntity> disposeTarget)
        {
            if (disposeProjectile == null) throw new ArgumentNullException(nameof(disposeProjectile));
            if (disposeTarget == null) throw new ArgumentNullException(nameof(disposeTarget));
            
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                disposeProjectile.Invoke(_projectiles[i]);
            }
            
            for (int i = _targets.Count - 1; i >= 0; i--)
            {
                disposeTarget.Invoke(_targets[i]);
            }
            
            _projectiles.Clear();
            _targets.Clear();
            _projectileIndices.Clear();
            _targetIndices.Clear();
            
            OnChanged?.Invoke();
        }
    }
}