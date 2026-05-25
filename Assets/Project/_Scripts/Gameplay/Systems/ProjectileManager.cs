using System;
using System.Collections.Generic;
using PoolingBenchmark.Domain;
using PoolingBenchmark.Gameplay.Entities;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Gameplay.Systems
{
    public sealed class ProjectileManager : ITickable
    {
        private readonly EntityRegistry _registry;
        private readonly List<Projectile> _toRemoveBuffer = new(512);

        public ProjectileManager(EntityRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            IReadOnlyList<Projectile> projectiles = _registry.Projectiles;
            int count = projectiles.Count;

            _toRemoveBuffer.Clear();
            
            for (int i = 0; i < count; i++)
            {
                Projectile p = projectiles[i];

                p.transform.Translate(p.Direction * (p.Speed * deltaTime), Space.World);
                p.CurrentLifetime += deltaTime;

                if (p.CurrentLifetime >= p.MaxLifetime)
                {
                    _toRemoveBuffer.Add(p);
                }
            }

            int removeCount = _toRemoveBuffer.Count;
            for (int i = 0; i < removeCount; i++)
            {
                _toRemoveBuffer[i].Despawn();
            }
        }
    }
}