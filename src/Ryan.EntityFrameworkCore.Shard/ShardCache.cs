using System.Collections.Generic;

namespace Ryan
{
    /// <summary>
    /// 分表缓存
    /// </summary>
    public abstract class ShardCache
    {
        /// <summary>
        /// 表名
        /// </summary>
        public List<string> TableNames { get; } = new List<string>();
    }

    /// <summary>
    /// 分表缓存
    /// </summary>
    public class ShardCache<TEntity> : ShardCache where TEntity : class
    {
    }
}
