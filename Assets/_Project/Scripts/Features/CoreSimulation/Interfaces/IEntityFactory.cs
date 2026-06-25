using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;
using UnityEngine;

namespace PoolingBenchmark.Features.CoreSimulation.Interfaces
{
    public interface IEntityFactory
    {
        int ProjNaiveCounter { get; }
        int TargetNaiveCounter { get; }
        void SetMode(ExecutionMode mode);
        void ResetCounter();
        ProjectileEntity CreateProjectile(Vector3 pos, Quaternion rot, Vector3 dir);
        TargetEntity CreateTarget(Vector3 pos, Vector3 dir);
        void Cleanup();
    }
}