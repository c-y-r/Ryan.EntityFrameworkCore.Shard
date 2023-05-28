using Microsoft.Extensions.DependencyInjection.Extensions;
using Ryan.DependencyInjection;
using Ryan.EntityFrameworkCore.Builder;
using Ryan.EntityFrameworkCore.Dynamic;
using Ryan.EntityFrameworkCore.Proxy;
using Ryan.EntityFrameworkCore.Query;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 分表扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 增加分表
        /// </summary>
        public static void AddShard(this ServiceCollection services)
        {
            services.TryAddSingleton<IDynamicSourceCodeGenerator, DynamicSourceCodeGenerator>();
            services.TryAddSingleton<IDynamicTypeGenerator, DynamicTypeGenerator>();
            services.TryAddSingleton<IEntityModelBuilderGenerator, EntityModelBuilderGenerator>();
            services.TryAddSingleton<IEntityImplementationDictionaryGenerator, EntityImplementationDictionaryGenerator>();
            services.TryAddSingleton<IEntityModelBuilderAccessorGenerator, EntityModelBuilderAccessorGenerator>();
            services.TryAddSingleton<IEntityShardConfiguration, EntityShardConfiguration>();
            services.TryAddSingleton<IEntityProxyGenerator, EntityProxyGenerator>();
            services.TryAddSingleton<IDbContextEntityProxyLookupGenerator, DbContextEntityProxyLookupGenerator>();
            services.TryAddSingleton<IDbContextEntityProxyGenerator, DbContextEntityProxyGenerator>();
            services.TryAddSingleton<IQueryableFinder, QueryableFinder>();
            services.TryAddSingleton<IShardDependency, ShardDependency>();
        }
    }
}
