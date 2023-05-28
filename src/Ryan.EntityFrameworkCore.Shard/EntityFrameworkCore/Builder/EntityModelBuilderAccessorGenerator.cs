using Microsoft.Extensions.Caching.Memory;
using Ryan.EntityFrameworkCore.Infrastructure;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <inheritdoc cref="IEntityModelBuilderAccessorGenerator"/>
    public class EntityModelBuilderAccessorGenerator : IEntityModelBuilderAccessorGenerator
    {
        /// <summary>
        /// 实体模型构建生成器
        /// </summary>
        public IEntityModelBuilderGenerator EntityModelBuilderGenerator { get; }

        /// <summary>
        /// 实体实现字典生成器
        /// </summary>
        public IEntityImplementationDictionaryGenerator ImplementationDictionaryGenerator { get; }

        /// <summary>
        /// 缓存
        /// </summary>
        public IMemoryCache MemoryCache { get; }

        /// <summary>
        /// 创建实体模型构建访问器
        /// </summary>
        public EntityModelBuilderAccessorGenerator(
            IEntityModelBuilderGenerator entityModelBuilderGenerator
            , IEntityImplementationDictionaryGenerator implementationDictionaryGenerator)
        {
            EntityModelBuilderGenerator = entityModelBuilderGenerator;
            ImplementationDictionaryGenerator = implementationDictionaryGenerator;
            MemoryCache = new InternalMemoryCache();
        }

        /// <inheritdoc/>
        public EntityModelBuilderAccessor Create(Type entityType)
        {
            return (MemoryCache.GetOrCreate(entityType, (entry) =>
            {
                var entityModelBulder = EntityModelBuilderGenerator.Create(entityType)!;
                var entityImplementationDictionary = ImplementationDictionaryGenerator.Create(entityType)!;
                return entry.SetSize(1).SetValue(
                    new EntityModelBuilderAccessor(entityType, entityImplementationDictionary, entityModelBulder)
                ).Value;
            }) as EntityModelBuilderAccessor)!;
        }
    }
}
