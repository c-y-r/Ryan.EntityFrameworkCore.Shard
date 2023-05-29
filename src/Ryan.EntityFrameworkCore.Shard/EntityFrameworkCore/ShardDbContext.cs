using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ryan.DependencyInjection;
using Ryan.EntityFrameworkCore.Builder;
using Ryan.EntityFrameworkCore.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ryan.EntityFrameworkCore
{
    /// <summary>
    /// 分表上下文
    /// </summary>
    public class ShardDbContext : DbContext
    {
        /// <summary>
        /// 分表实体
        /// </summary>
        private List<Type> ShardEntityTypes { get; set; } = new List<Type>();

        /// <summary>
        /// 分表依赖
        /// </summary>
        public IShardDependency Dependencies { get; }

        /// <summary>
        /// 创建分表上下文
        /// </summary>
        public ShardDbContext(IShardDependency shardDependency)
        {

            InitShardConfiguration();
            Dependencies = shardDependency;
        }

        /// <summary>
        /// 初始化分表配置
        /// </summary>
        private void InitShardConfiguration()
        {
            // 获取分表类型
            var shardAttribute = GetType().GetCustomAttribute<ShardAttribute>();
            if (shardAttribute == null)
            {
                return;
            }

            ShardEntityTypes.AddRange(shardAttribute.GetShardEntities());
        }

        /// <summary>
        /// 配置
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        /// <summary>
        /// 构建实体
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 构建 ModelBuilder
            foreach (var shardEntity in ShardEntityTypes)
            {
                Dependencies.EntityModelBuilderAccessorGenerator.Create(shardEntity).Accessor(shardEntity, modelBuilder);
            }
        }

        /// <summary>
        /// 创建非查询代理
        /// </summary>
        internal EntityProxy CreateEntityProxy(object entity, EntityProxyType type)
        {
            // 上下文代理
            var dbContextProxy = Dependencies.DbContextEntityProxyLookupGenerator
                .Create(this)
                .GetOrDefault(entity.GetType().BaseType!, this);

            // 创建代理
            var proxy = dbContextProxy.EntityProxies.FirstOrDefault(x => x.Entity == entity);
            if (proxy != null)
            {
                return proxy;
            }

            // 创建代理
            dbContextProxy.EntityProxies.Add(
                proxy = Dependencies.EntityProxyGenerator.Create(entity, EntityProxyType.NonQuery, this));

            return proxy;
        }

        /// <summary>
        /// 分表查询
        /// </summary>
        public virtual IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            if (!ShardEntityTypes.Contains(typeof(TEntity)))
            {
                throw new InvalidOperationException();
            }

            // 获取实现
            var queryables = Dependencies.ExpressionImplementationFinder
                .Find<TEntity>(expression)
                .Select(x => Dependencies.QueryableFinder.Find<TEntity>(this, x));
            
            // 组合查询
            var queryable = queryables.FirstOrDefault();
            foreach (var nextQueryable in queryables.Skip(1))
            {
                queryable = queryable.Union(nextQueryable);
            }

            return queryable;
        }

        /// <summary>
        /// 分表查询
        /// </summary>
        public virtual IEnumerable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (!ShardEntityTypes.Contains(typeof(TEntity)))
            {
                throw new InvalidOperationException();
            }

            // 获取实现
            var queryables = Dependencies.ExpressionImplementationFinder
                .Find<TEntity>(expression)
                .Select(x => Dependencies.QueryableFinder.Find<TEntity>(this, x));

            // 组合查询
            return queryables.SelectMany(x => x.Where(predicate).ToList());
        }

        /// <inheritdoc/>
        public override EntityEntry Add(object entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Add(entity);
            }

            // 将实现添加入状态管理
            return base.Add(CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.AddAsync(entity, cancellationToken);
            }

            // 将实现添加入状态管理
            return base.AddAsync(CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation, cancellationToken);
        }

        /// <inheritdoc/>
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Add(entity);
            }

            // 将实现添加入状态管理
            return base.Add((TEntity)CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.AddAsync(entity, cancellationToken);
            }

            // 将实现添加入状态管理
            return base.AddAsync((TEntity)CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation, cancellationToken);
        }

        /// <inheritdoc/>
        public override EntityEntry Attach(object entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Attach(entity);
            }

            return base.Attach(CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Attach(entity);
            }

            return base.Attach((TEntity)CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override EntityEntry Entry(object entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Entry(entity);
            }

            return base.Entry(CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Entry(entity);
            }

            return base.Entry((TEntity)CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override EntityEntry Remove(object entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Remove(entity);
            }

            return base.Remove(CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Remove(entity);
            }

            return base.Remove((TEntity)CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override EntityEntry Update(object entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Update(entity);
            }

            return base.Update(CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if (!ShardEntityTypes.Contains(entity.GetType()))
            {
                return base.Update(entity);
            }

            return base.Update((TEntity)CreateEntityProxy(entity, EntityProxyType.NonQuery).Implementation);
        }

        /// <inheritdoc/>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var lookup = Dependencies.DbContextEntityProxyLookupGenerator.Create(this);

            lookup.Changes();

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            lookup.Changed();

            return result;
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var lookup = Dependencies.DbContextEntityProxyLookupGenerator.Create(this);

            lookup.Changes();

            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            lookup.Changed();

            return result;
        }

        public override void Dispose()
        {
            Dependencies.DbContextEntityProxyLookupGenerator.Delete(this);
            base.Dispose();
        }
    }
}
