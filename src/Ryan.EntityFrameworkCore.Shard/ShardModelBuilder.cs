using Microsoft.EntityFrameworkCore;

namespace Ryan
{
    /// <summary>
    /// 分表 ModelBuilder
    /// </summary>
    public abstract class ShardModelBuilder<TEntity>
    {
        /// <summary>
        /// 配置 ModelBuilder
        /// </summary>
        public abstract void OnModelCreating(ModelBuilder modelBuilder);
    }
}
