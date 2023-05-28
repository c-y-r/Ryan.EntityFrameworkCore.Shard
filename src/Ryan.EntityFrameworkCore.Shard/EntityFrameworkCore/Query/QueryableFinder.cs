using Microsoft.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Builder;
using System.Reflection;

namespace Ryan.EntityFrameworkCore.Query
{
    public class QueryableFinder : IQueryableFinder
    {
        public IEntityModelBuilderAccessorGenerator EntityModelBuilderAccessorGenerator { get; }
        public MethodInfo MethodInfoSet { get; }

        public QueryableFinder(IEntityModelBuilderAccessorGenerator entityModelBuilderAccessorGenerator)
        {
            EntityModelBuilderAccessorGenerator = entityModelBuilderAccessorGenerator;
            MethodInfoSet = typeof(DbContext).GetMethods()[4]; // Set()
        }

        public virtual object CreateDbSet(DbContext context, Type implementationType)
        {
            var methodInfo = MethodInfoSet.MakeGenericMethod(implementationType)!;

            return methodInfo.Invoke(context, null)!;
        }

        private IQueryable<TEntity> DbSetConvert<TEntity>(object set) where TEntity : class
        {
            var methodInfo = typeof(Queryable)
                .GetMethod("OfType")!
                .MakeGenericMethod(typeof(TEntity))!;

            return (IQueryable<TEntity>)methodInfo.Invoke(null, new object[] { set })!;
        }

        public IQueryable<TEntity> Find<TEntity>(DbContext context, Type implementationType) where TEntity : class
        {
            var set = CreateDbSet(context, implementationType);
            return DbSetConvert<TEntity>(set);
        }
    }
}
