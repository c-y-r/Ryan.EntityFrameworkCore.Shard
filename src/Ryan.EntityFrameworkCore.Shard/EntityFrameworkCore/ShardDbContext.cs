using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Ryan.DependencyInjection;
using Ryan.EntityFrameworkCore.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ryan.EntityFrameworkCore
{
    /// <summary>
    /// 分表上下文
    /// </summary>
    public class ShardDbContext<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 锁
        /// </summary>
        private object _lock = new object();

        /// <summary>
        /// 实际上下文
        /// </summary>
        public TDbContext DbContext { get; protected set; }

        /// <summary>
        /// 创建分表上下文
        /// </summary>
        public ShardDbContext()
        {
            ReloadDbContext();
        }

        /// <summary>
        /// 获取缓存表
        /// </summary>
        public IEnumerable<(ShardTableCache, IEnumerable<TEntity>)> GetShardTableCache<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            var shardCache = RyanService.ServiceProvider.GetService<ShardCache<TEntity>>();

            foreach (var relation in RyanService.ServiceProvider.GetService<ShardDescriptor<TEntity>>().TablesFromEntities(entities))
            {
                // 根据表名获取缓存
                var shardTableCache = shardCache.Tables.FirstOrDefault(x => x.TableName == relation.Item1);
                if (shardTableCache == null)
                {
                    lock (_lock)
                    {
                        shardTableCache = shardCache.Tables.FirstOrDefault(x => x.TableName == relation.Item1);
                        if (shardTableCache == null)
                        {
                            shardTableCache = ShardTableCache.FromTableName(relation.Item1, typeof(TEntity));
                            shardCache.Tables.Add(shardTableCache);
                            ReloadDbContext();
                        }
                    }
                }

                yield return (shardTableCache, relation.Item2);
            }
        }   

        /// <inheritdoc cref="DbContext.Add(object)"/>/>
        public EntityEntry Add<TEntity>(TEntity entity) where TEntity : class
        {
            // 映射
            var mapper = RyanService.ServiceProvider.GetService<IShardMapper<TEntity>>();

            // 获取缓存
            foreach (var cache in GetShardTableCache(new[] { entity }))
            {
                foreach (var item in cache.Item2)
                {
                    var shardEntity = mapper.Map(item, cache.Item1.TableType);
                    return DbContext.Add(shardEntity);
                }
            }

            throw new InvalidOperationException();
        }

        /// <inheritdoc cref="DbContext.AddRange(IEnumerable{object})"/>
        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            // 映射
            var mapper = RyanService.ServiceProvider.GetService<IShardMapper<TEntity>>();

            // 获取缓存
            foreach (var cache in GetShardTableCache(entities))
            {
                foreach (var item in cache.Item2)
                {
                    var shardEntity = mapper.Map(item, cache.Item1.TableType);
                    DbContext.Add(shardEntity);
                }
            }
        }

        /// <summary>
        /// 重载上下文
        /// </summary>
        public void ReloadDbContext()
        {
            DbContext = RyanService.ServiceProvider.GetService<TDbContext>();
        }
    }
}
