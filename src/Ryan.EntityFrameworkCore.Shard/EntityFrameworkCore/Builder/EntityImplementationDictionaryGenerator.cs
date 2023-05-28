using Microsoft.Extensions.Caching.Memory;
using Ryan.EntityFrameworkCore.Infrastructure;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <inheritdoc cref="IEntityImplementationDictionaryGenerator" />
    public class EntityImplementationDictionaryGenerator : IEntityImplementationDictionaryGenerator
    {
        /// <summary>
        /// 缓存
        /// </summary>
        public IMemoryCache MemoryCache { get; }

        /// <inheritdoc/>
        public EntityImplementationDictionaryGenerator()
        {
            MemoryCache = new InternalMemoryCache();
        }

        /// <inheritdoc/>
        public virtual EntityImplementationDictionary Create(Type entityType)
        {
            return (MemoryCache.GetOrCreate(entityType, (entry) =>
            {
                return entry.SetSize(1).SetValue(
                    new EntityImplementationDictionary(entityType)
                ).Value;
            }) as EntityImplementationDictionary)!;
        }
    }
}
