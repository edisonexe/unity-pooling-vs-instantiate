using System;
using System.Collections.Generic;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;

namespace PoolingBenchmark.Features.CoreSimulation.Services
{
    public sealed class EntityRegistry
    {
        public event Action OnChanged;

        private readonly List<Projectile> _projectiles = new(4096);
        private readonly List<Target> _targets = new(4096);

        private readonly Dictionary<Projectile, int> _projectileIndices = new(4096);
        private readonly Dictionary<Target, int> _targetIndices = new(4096);

        public IReadOnlyList<Projectile> Projectiles => _projectiles;
        public IReadOnlyList<Target> Targets => _targets;

        public void AddProjectile(Projectile p)
        {
            if (!p) throw new ArgumentNullException(nameof(p));
            if (_projectileIndices.ContainsKey(p)) return;

            _projectiles.Add(p);
            _projectileIndices.Add(p, _projectiles.Count - 1);
            OnChanged?.Invoke();
        }

        public void RemoveProjectile(Projectile p)
        {
            if (!p || !_projectileIndices.TryGetValue(p, out int index)) return;

            int lastIndex = _projectiles.Count - 1;
            if (index != lastIndex)
            {
                Projectile lastElement = _projectiles[lastIndex];
                _projectiles[index] = lastElement;
                _projectileIndices[lastElement] = index;
            }

            _projectiles.RemoveAt(lastIndex);
            _projectileIndices.Remove(p);
            OnChanged?.Invoke();
        }

        public void AddTarget(Target t)
        {
            if (!t) throw new ArgumentNullException(nameof(t));
            if (_targetIndices.ContainsKey(t)) return;

            _targets.Add(t);
            _targetIndices.Add(t, _targets.Count - 1);
            OnChanged?.Invoke();
        }

        public void RemoveTarget(Target t)
        {
            if (!t || !_targetIndices.TryGetValue(t, out int index)) return;

            int lastIndex = _targets.Count - 1;
            if (index != lastIndex)
            {
                Target lastElement = _targets[lastIndex];
                _targets[index] = lastElement;
                _targetIndices[lastElement] = index;
            }

            _targets.RemoveAt(lastIndex);
            _targetIndices.Remove(t);
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            _projectiles.Clear();
            _targets.Clear();
            _projectileIndices.Clear();
            _targetIndices.Clear();
            OnChanged?.Invoke();
        }
    }
}