using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;

namespace PoolingBenchmark.Features.CoreSimulation.Interfaces
{
    public interface IEntityFactory
    {
        ProjectileEntity CreateProjectile();
        TargetEntity CreateTarget();
    }
}