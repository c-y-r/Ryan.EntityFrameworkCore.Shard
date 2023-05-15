using Microsoft.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Caches;
using System.Linq;

namespace Ryan.EntityFrameworkCore
{
    /// <summary>
    /// 分表 ModelBuilder
    /// </summary>
    public abstract class ShardModelBuilder<TEntity> where TEntity : class
    {
        /// <summary>
        /// 缓存
        /// </summary>
        public ShardCache<TEntity> ShardCache { get; }

        public ShardModelBuilder(ShardCache<TEntity> shardCache)
        {
            ShardCache = shardCache;
        }

        /// <summary>
        /// 应用分表
        /// </summary>
        public virtual void ApplyShard(ModelBuilder modelBuilder)
        {
            foreach (var item in ShardCache.Tables)
            {
                GetType().GetMethod("Apply").MakeGenericMethod(item.TableType).Invoke(this, new object[] { modelBuilder, item });
            }
        }

        /// <summary>
        /// 配置 ModelBuilder
        /// </summary>
        public virtual void Apply<TEntityImpl>(ModelBuilder modelBuilder, ShardTableCache cache) where TEntityImpl : TEntity
        {
        }
    }
}
