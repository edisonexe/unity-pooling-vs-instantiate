using System;
using PoolingBenchmark.Features.CoreSimulation.Interfaces;
using PoolingBenchmark.Features.Projectiles;
using PoolingBenchmark.Features.Targets;

namespace PoolingBenchmark.Features.CoreSimulation.Services
{
    public sealed class EntityFactory : IEntityFactory
    {
        private readonly Func<Action<ProjectileEntity>, ProjectileEntity> _projectileConcreteFactory;
        private readonly Func<Action<TargetEntity>, TargetEntity> _targetConcreteFactory;
        
        private Action<ProjectileEntity> _onProjectileRecycle;
        private Action<TargetEntity> _onTargetRecycle;

        public EntityFactory(
            Func<Action<ProjectileEntity>, ProjectileEntity> projectileConcreteFactory,
            Func<Action<TargetEntity>, TargetEntity> targetConcreteFactory)
        {
            _projectileConcreteFactory = projectileConcreteFactory ?? throw new ArgumentNullException(nameof(projectileConcreteFactory));
            _targetConcreteFactory = targetConcreteFactory ?? throw new ArgumentNullException(nameof(targetConcreteFactory));
        }

        public void RegisterProjectileRecycle(Action<ProjectileEntity> onProjRecycle)
        {
            _onProjectileRecycle = onProjRecycle ?? throw new ArgumentNullException(nameof(onProjRecycle));
        }

        public void RegisterTargetRecycle(Action<TargetEntity> onTargetRecycle)
        {
            _onTargetRecycle = onTargetRecycle ?? throw new ArgumentNullException(nameof(onTargetRecycle));
        }

        public ProjectileEntity CreateProjectile()
        {
            if (_onProjectileRecycle == null) throw new InvalidOperationException("Projectile recycle callback is not registered!");
            return _projectileConcreteFactory.Invoke(_onProjectileRecycle);
        }

        public TargetEntity CreateTarget()
        {
            if (_onTargetRecycle == null) throw new InvalidOperationException("Target recycle callback is not registered!");
            return _targetConcreteFactory.Invoke(_onTargetRecycle);
        }
    }
}