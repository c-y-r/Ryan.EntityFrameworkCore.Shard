using Ryan.EntityFrameworkCore.Dynamic;
using System;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <inheritdoc cref="IEntityShardConfiguration"/>
    public class EntityShardConfiguration : IEntityShardConfiguration
    {
        /// <inheritdoc cref="IEntityImplementationDictionaryGenerator"/>
        public IEntityImplementationDictionaryGenerator EntityImplementationDictionaryGenerator { get; }

        /// <summary>
        /// 动态类型创建
        /// </summary>
        public IDynamicTypeGenerator DynamicTypeGenerator { get; }

        /// <summary>
        /// 创建实体分表配置
        /// </summary>
        public EntityShardConfiguration(
            IEntityImplementationDictionaryGenerator entityImplementationDictionaryGenerator
            , IDynamicTypeGenerator dynamicTypeGenerator)
        {
            EntityImplementationDictionaryGenerator = entityImplementationDictionaryGenerator;
            DynamicTypeGenerator = dynamicTypeGenerator;
        }

        /// <inheritdoc/>
        public Type AddShard<TEntity>(string tableName) where TEntity : class
        {
            var implementationType = DynamicTypeGenerator.Create(typeof(TEntity));
            EntityImplementationDictionaryGenerator
                .Create(typeof(TEntity))
                .Add(tableName, new EntityImplementation(typeof(TEntity), implementationType, tableName));
            return implementationType;
        }
    }
}
