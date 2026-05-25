using System;
using System.Collections.Generic;
using PoolingBenchmark.Domain;
using PoolingBenchmark.Domain.Configs;
using PoolingBenchmark.Gameplay.Entities;
using PoolingBenchmark.Gameplay.Views;
using PoolingBenchmark.Interfaces;
using UnityEngine;
using Zenject;

namespace PoolingBenchmark.Gameplay.Systems
{
    public sealed class TurretSystem : ITickable
    {
        private readonly TurretView _view;
        private readonly IEntityFactory _factory;
        private readonly EntityRegistry _registry;
        private readonly SimulationConfig _config;

        private Target _currentTarget;
        private float _fireTimer;
        private Quaternion _lookRotation;

        public TurretSystem(TurretView view, IEntityFactory factory, EntityRegistry registry, SimulationConfig config)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            if (_view.YawRoot != null)
            {
                _lookRotation = _view.YawRoot.rotation;
            }
        }

        public void Tick()
        {
            Transform yawTransform = _view.YawRoot;
            if (!yawTransform) return;

            Vector3 gunPos = yawTransform.position;
            Vector3 gunForward = yawTransform.forward;
            
            if (!IsTargetValid(_currentTarget, gunPos, gunForward))
            {
                _currentTarget = FindBestTarget(gunPos, gunForward);
            }

            if (_currentTarget)
            {
                UpdateTargetRotation(_currentTarget.transform.position, gunPos);
            }

            yawTransform.rotation = Quaternion.RotateTowards(
                yawTransform.rotation,
                _lookRotation,
                270f * Time.deltaTime
            );

            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _config.FireRate)
            {
                _fireTimer = 0f;
                
                _factory.CreateProjectile(_view.ShootingPoint.position, yawTransform.rotation, yawTransform.forward);
            }
        }

        private bool IsTargetValid(Target target, Vector3 myPos, Vector3 myForward)
        {
            if (!target || !target.gameObject.activeSelf) return false;

            Vector3 diff = target.transform.position - myPos;
            float sqrDist = diff.sqrMagnitude;
            if (sqrDist < 0.001f) return false;

            Vector3 dirToTarget = diff / Mathf.Sqrt(sqrDist);
            float dotProduct = Vector3.Dot(myForward, dirToTarget);
            float minCosThreshold = Mathf.Cos(_config.FovAngle * 0.5f * Mathf.Deg2Rad);

            return dotProduct >= minCosThreshold;
        }

        private void UpdateTargetRotation(Vector3 targetPos, Vector3 myPos)
        {
            Vector3 direction = (targetPos - myPos).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                _lookRotation = Quaternion.LookRotation(direction);
            }
        }

        private Target FindBestTarget(Vector3 myPos, Vector3 myForward)
        {
            Target best = null;
            float minScore = float.MaxValue;
            float minCosThreshold = Mathf.Cos(_config.FovAngle * 0.5f * Mathf.Deg2Rad);
            
            IReadOnlyList<Target> targets = _registry.Targets;
            int count = targets.Count;

            for (int i = 0; i < count; i++)
            {
                Target t = targets[i];
                if (!t || !t.gameObject.activeSelf) continue;

                Vector3 diff = t.transform.position - myPos;
                float sqrDist = diff.sqrMagnitude;

                float distance = Mathf.Sqrt(sqrDist);
                if (distance < 0.001f) continue;
                Vector3 dirToTarget = diff / distance;

                float dotProduct = Vector3.Dot(myForward, dirToTarget);
                if (dotProduct < minCosThreshold) continue;

                float angleScore = (1.0f - dotProduct) * 10f;
                float score = sqrDist + angleScore;

                if (score < minScore)
                {
                    minScore = score;
                    best = t;
                }
            }
            return best;
        }
    }
}