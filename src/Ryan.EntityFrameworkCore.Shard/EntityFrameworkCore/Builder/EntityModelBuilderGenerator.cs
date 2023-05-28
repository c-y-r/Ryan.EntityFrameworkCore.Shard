using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Ryan.EntityFrameworkCore.Infrastructure;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体模型构建对象生成器
    /// </summary>
    public class EntityModelBuilderGenerator : IEntityModelBuilderGenerator
    {
        /// <summary>
        /// 容器
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 内存缓存
        /// </summary>
        public IMemoryCache MemoryCache { get; }

        /// <summary>
        /// 创建实体模型构建对象生成器
        /// </summary>
        public EntityModelBuilderGenerator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            MemoryCache = new InternalMemoryCache();
        }

        /// <summary>
        /// 创建实体模块构建器
        /// </summary>
        public virtual object CreateEntityModelBuilder(Type entityType)
        {
            var entityModelBuilderType = typeof(EntityModelBuilder<>).MakeGenericType(entityType);
            return ServiceProvider.GetRequiredService(entityModelBuilderType);
        }

        /// <inheritdoc/>
        public virtual object Create(Type entityType)
        {
            return MemoryCache.GetOrCreate(entityType, (entry) => 
            {
                return entry.SetSize(1).SetValue(
                    CreateEntityModelBuilder(entityType)
                ).Value;
            });
        }
    }
}
