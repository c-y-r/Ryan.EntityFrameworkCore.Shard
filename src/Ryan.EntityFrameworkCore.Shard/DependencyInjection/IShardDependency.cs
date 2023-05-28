using Ryan.EntityFrameworkCore.Builder;
using Ryan.EntityFrameworkCore.Dynamic;
using Ryan.EntityFrameworkCore.Proxy;
using Ryan.EntityFrameworkCore.Query;

namespace Ryan.DependencyInjection
{
    /// <summary>
    /// 分表依赖
    /// </summary>
    public interface IShardDependency
    {
        IDynamicSourceCodeGenerator DynamicSourceCodeGenerator { get; }
        IDynamicTypeGenerator DynamicTypeGenerator { get; }
        IEntityModelBuilderGenerator EntityModelBuilderGenerator { get; }
        IEntityImplementationDictionaryGenerator EntityImplementationDictionaryGenerator { get; }
        IEntityModelBuilderAccessorGenerator EntityModelBuilderAccessorGenerator { get; }
        IEntityShardConfiguration EntityShardConfiguration { get; }
        IEntityProxyGenerator EntityProxyGenerator { get; }
        IDbContextEntityProxyLookupGenerator DbContextEntityProxyLookupGenerator { get; }
        IDbContextEntityProxyGenerator DbContextEntityProxyGenerator { get; }
        IQueryableFinder QueryableFinder { get; }
    }
}
