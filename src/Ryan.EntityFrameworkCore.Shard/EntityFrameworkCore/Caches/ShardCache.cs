using System.Collections.Generic;

namespace Ryan.EntityFrameworkCore.Caches
{
    /// <summary>
    /// 分表缓存
    /// </summary>
    public abstract class ShardCache
    {
        /// <summary>
        /// 表名
        /// </summary>
        public List<ShardTableCache> Tables { get; } = new List<ShardTableCache>();
    }

    /// <summary>
    /// 分表缓存
    /// </summary>
    public class ShardCache<TEntity> : ShardCache where TEntity : class
    {
    }
}
