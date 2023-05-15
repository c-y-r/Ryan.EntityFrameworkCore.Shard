using System;

namespace Ryan.EntityFrameworkCore.Mapper
{
    /// <summary>
    /// 默认映射
    /// </summary>
    public class DefaultShardMapper<TEntity> : IShardMapper<TEntity> where TEntity : class
    {
        public TEntity Map(TEntity entity, Type shardType)
        {
            var shardEntity = (TEntity)Activator.CreateInstance(shardType);

            foreach (var property in typeof(TEntity).GetProperties())
            {
                property.SetValue(shardEntity, property.GetValue(entity));
            }

            return shardEntity;
        }
    }
}
