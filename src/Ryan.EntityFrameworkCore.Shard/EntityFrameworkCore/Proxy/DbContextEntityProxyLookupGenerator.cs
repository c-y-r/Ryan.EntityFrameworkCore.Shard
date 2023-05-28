using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Ryan.EntityFrameworkCore.Infrastructure;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <inheritdoc cref="IDbContextEntityProxyLookupGenerator"/>
    public class DbContextEntityProxyLookupGenerator : IDbContextEntityProxyLookupGenerator
    {
        /// <summary>
        /// 上下文实体代理生成器
        /// </summary>
        public IDbContextEntityProxyGenerator DbContextEntityProxyGenerator { get; }

        /// <summary>
        /// 缓存
        /// </summary>
        public IMemoryCache MemoryCache { get; set; }

        /// <summary>
        /// 创建上下文实体代理字典
        /// </summary>
        public DbContextEntityProxyLookupGenerator(IDbContextEntityProxyGenerator dbContextEntityProxyGenerator)
        {
            DbContextEntityProxyGenerator = dbContextEntityProxyGenerator;
            MemoryCache = new InternalMemoryCache();
        }

        /// <inheritdoc/>
        public DbContextEntityProxyLookup Create(DbContext dbContext)
        {
            return (MemoryCache.GetOrCreate(dbContext.ContextId, (entry) =>
            {
                return entry.SetSize(1).SetValue(
                    new DbContextEntityProxyLookup(DbContextEntityProxyGenerator)
                );
            }).Value as DbContextEntityProxyLookup)!;
        }

        /// <inheritdoc/>
        public void Delete(DbContext dbContext)
        {
            MemoryCache.Remove(dbContext.ContextId);
        }
    }
}
