using System;
using System.Collections.Generic;
using PoolingBenchmark.Features.CoreSimulation.Services;
using PoolingBenchmark.Features.Environment;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Features.Targets
{
    public sealed class TargetManager : ITickable
    {
        private readonly EntityRegistry _registry;
        private readonly SpatialGrid _grid;
        private readonly ArenaBoundary _boundary;
        
        private readonly List<TargetEntity> _outOfBoundsBuffer = new(256);

        public TargetManager(EntityRegistry registry, SpatialGrid grid, ArenaBoundary boundary)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _boundary = boundary ?? throw new ArgumentNullException(nameof(boundary));
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            IReadOnlyList<TargetEntity> targets = _registry.Targets;
            int count = targets.Count;
            
            _grid.Clear();
            _outOfBoundsBuffer.Clear();

            for (int i = 0; i < count; i++)
            {
                TargetEntity t = targets[i];
                
                if (t.IsDead) continue;
                
                Vector3 newPosition = t.Position + t.MoveDirection * (t.Speed * deltaTime);
                t.UpdatePosition(newPosition);

                if (!_boundary.IsInside(t.Position))
                {
                    _outOfBoundsBuffer.Add(t);
                }
                else
                {
                    _grid.Insert(t);
                }
            }

            int removeCount = _outOfBoundsBuffer.Count;
            for (int i = 0; i < removeCount; i++)
            {
                _outOfBoundsBuffer[i].Despawn();
            }
        }
    }
}