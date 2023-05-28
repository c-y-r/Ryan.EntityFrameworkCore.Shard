using System;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体分配配置
    /// </summary>
    public interface IEntityShardConfiguration
    {
        /// <summary>
        /// 添加分表
        /// </summary>
        Type AddShard<TEntity>(string tableName) where TEntity : class;
    }
}
