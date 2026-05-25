using System;
using System.Collections.Generic;
using PoolingBenchmark.Domain;
using PoolingBenchmark.Gameplay.Entities;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Gameplay.Systems
{
    public sealed class TargetManager : ITickable
    {
        private readonly EntityRegistry _registry;

        public TargetManager(EntityRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            IReadOnlyList<Target> targets = _registry.Targets;
            int count = targets.Count;

            for (int i = 0; i < count; i++)
            {
                Target t = targets[i];

                t.transform.Translate(t.MoveDirection * (t.Speed * deltaTime), Space.World);
            }
        }
    }
}