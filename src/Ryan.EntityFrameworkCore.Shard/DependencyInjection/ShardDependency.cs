using Ryan.EntityFrameworkCore.Builder;
using Ryan.EntityFrameworkCore.Dynamic;
using Ryan.EntityFrameworkCore.Proxy;
using Ryan.EntityFrameworkCore.Query;

namespace Ryan.DependencyInjection
{
    /// <inheritdoc cref="IShardDependency"/>
    public class ShardDependency : IShardDependency
    {
        public ShardDependency(
            IDynamicSourceCodeGenerator dynamicSourceCodeGenerator
            , IDynamicTypeGenerator dynamicTypeGenerator
            , IEntityModelBuilderGenerator entityModelBuilderGenerator
            , IEntityImplementationDictionaryGenerator entityImplementationDictionaryGenerator
            , IEntityModelBuilderAccessorGenerator entityModelBuilderAccessorGenerator
            , IEntityShardConfiguration entityShardConfiguration
            , IEntityProxyGenerator entityProxyGenerator
            , IDbContextEntityProxyLookupGenerator dbContextEntityProxyLookupGenerator
            , IDbContextEntityProxyGenerator dbContextEntityProxyGenerator
            , IQueryableFinder queryableFinder)
        {

            DynamicSourceCodeGenerator = dynamicSourceCodeGenerator;
            DynamicTypeGenerator = dynamicTypeGenerator;
            EntityModelBuilderGenerator = entityModelBuilderGenerator;
            EntityImplementationDictionaryGenerator = entityImplementationDictionaryGenerator;
            EntityModelBuilderAccessorGenerator = entityModelBuilderAccessorGenerator;
            EntityShardConfiguration = entityShardConfiguration;
            EntityProxyGenerator = entityProxyGenerator;
            DbContextEntityProxyLookupGenerator = dbContextEntityProxyLookupGenerator;
            DbContextEntityProxyGenerator = dbContextEntityProxyGenerator;
            QueryableFinder = queryableFinder;
        }

        public IDynamicSourceCodeGenerator DynamicSourceCodeGenerator { get; }
        public IDynamicTypeGenerator DynamicTypeGenerator { get; }
        public IEntityModelBuilderGenerator EntityModelBuilderGenerator { get; }
        public IEntityImplementationDictionaryGenerator EntityImplementationDictionaryGenerator { get; }
        public IEntityModelBuilderAccessorGenerator EntityModelBuilderAccessorGenerator { get; }
        public IEntityShardConfiguration EntityShardConfiguration { get; }
        public IEntityProxyGenerator EntityProxyGenerator { get; }
        public IDbContextEntityProxyLookupGenerator DbContextEntityProxyLookupGenerator { get; }
        public IDbContextEntityProxyGenerator DbContextEntityProxyGenerator { get; }
        public IQueryableFinder QueryableFinder { get; }
    }
}
