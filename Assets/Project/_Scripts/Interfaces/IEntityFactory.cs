using PoolingBenchmark.Enums;
using PoolingBenchmark.Gameplay.Entities;
using UnityEngine;

namespace PoolingBenchmark.Interfaces
{
    public interface IEntityFactory
    {
        int ProjNaiveCounter { get; }
        int TargetNaiveCounter { get; }
        void SetMode(ExecutionMode mode);
        void ResetCounter();
        Projectile CreateProjectile(Vector3 pos, Quaternion rot, Vector3 dir);
        Target CreateTarget(Vector3 pos, Vector3 dir);
        void Cleanup();
    }
}