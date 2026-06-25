using System;
using System.Collections.Generic;
using PoolingBenchmark.Features.CoreSimulation.Configs;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Infrastructure.Collections;
using PoolingBenchmark.Features.Targets;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Features.Projectiles
{
    public sealed class ProjectileManager : ITickable
    {
        private readonly EntityRegistry _registry;
        private readonly SimulationConfig _config;
        private readonly SpatialGrid _grid;
        
        private readonly List<ProjectileEntity> _toRemoveBuffer = new(512);

        public ProjectileManager(EntityRegistry registry, SimulationConfig config, SpatialGrid grid)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            IReadOnlyList<ProjectileEntity> projectiles = _registry.Projectiles;
            int projsCount = projectiles.Count;
            
            _toRemoveBuffer.Clear();

            float hitRadius = _config.HitRadius;
            float sqrHitRadius = hitRadius * hitRadius;

            // 1. Движение снарядов
            for (int i = 0; i < projsCount; i++)
            {
                ProjectileEntity p = projectiles[i];
                
                Vector3 newPosition = p.Position + p.Direction * (p.Speed * deltaTime);
                p.UpdatePosition(newPosition);
                p.AdvanceLifetime(deltaTime);

                if (p.IsExpired)
                {
                    _toRemoveBuffer.Add(p);
                }
            }

            // 2. Локальный просчет коллизий через сетку
            for (int i = 0; i < projsCount; i++)
            {
                ProjectileEntity proj = projectiles[i];
                if (_toRemoveBuffer.Contains(proj)) continue; 

                Vector3 projPos = proj.Position;

                List<TargetEntity> cellTargets = _grid.GetCell(projPos);
                if (cellTargets == null || cellTargets.Count == 0) continue;

                // Проверка коллизий только внутри локальной ячейки
                for (int j = cellTargets.Count - 1; j >= 0; j--)
                {
                    TargetEntity target = cellTargets[j];
                    Vector3 targetPos = target.Position;

                    float deltaX = projPos.x - targetPos.x;
                    if (deltaX > hitRadius || deltaX < -hitRadius) continue;
                    
                    float deltaZ = projPos.z - targetPos.z;
                    if (deltaZ > hitRadius || deltaZ < -hitRadius) continue;

                    float sqrDistance2D = (deltaX * deltaX) + (deltaZ * deltaZ);

                    if (sqrDistance2D <= sqrHitRadius)
                    {
                        _toRemoveBuffer.Add(proj);
                        target.Despawn();
                        cellTargets.RemoveAt(j);
                        break; 
                    }
                }
            }

            // 3. Деспавн снарядов
            int removeCount = _toRemoveBuffer.Count;
            for (int i = 0; i < removeCount; i++)
            {
                _toRemoveBuffer[i].Despawn();
            }
        }
    }
}