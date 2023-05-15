using System;

namespace Ryan.EntityFrameworkCore
{
    /// <summary>
    /// 分表对象转换
    /// </summary>
    public interface IShardMapper<TEntity> where TEntity : class
    {
        /// <summary>
        /// 映射
        /// </summary>
        public TEntity Map(TEntity entity, Type shardType);
    }
}
