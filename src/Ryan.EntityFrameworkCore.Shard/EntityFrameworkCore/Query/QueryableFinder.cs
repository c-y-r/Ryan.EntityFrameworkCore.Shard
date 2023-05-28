using Microsoft.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Builder;
using System;
using System.Linq;
using System.Reflection;

namespace Ryan.EntityFrameworkCore.Query
{
    public class QueryableFinder : IQueryableFinder
    {
        public IEntityModelBuilderAccessorGenerator EntityModelBuilderAccessorGenerator { get; }
        public MethodInfo MethodInfoSet { get; }
        public MethodInfo MethodInfoOfType { get; }

        public QueryableFinder(IEntityModelBuilderAccessorGenerator entityModelBuilderAccessorGenerator)
        {
            EntityModelBuilderAccessorGenerator = entityModelBuilderAccessorGenerator;
            MethodInfoSet = typeof(DbContext).GetMethods()[4]; // Set()
            MethodInfoOfType = typeof(Queryable).GetMethod("OfType")!;
        }

        public virtual object CreateDbSet(DbContext context, Type implementationType)
        {
            var methodInfo = MethodInfoSet.MakeGenericMethod(implementationType)!;

            return methodInfo.Invoke(context, null)!;
        }

        private IQueryable<TEntity> DbSetConvert<TEntity>(object set) where TEntity : class
        {
            return (IQueryable<TEntity>)MethodInfoOfType.MakeGenericMethod(typeof(TEntity)).Invoke(null, new object[] { set })!;
        }

        public IQueryable<TEntity> Find<TEntity>(DbContext context, Type implementationType) where TEntity : class
        {
            var set = CreateDbSet(context, implementationType);
            return DbSetConvert<TEntity>(set);
        }
    }
}
