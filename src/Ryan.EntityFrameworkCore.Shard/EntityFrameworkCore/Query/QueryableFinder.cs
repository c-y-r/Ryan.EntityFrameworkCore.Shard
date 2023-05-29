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
            MethodInfoSet = typeof(DbContext).GetMethods().FirstOrDefault(x => x.Name == "Set"); // Set()
            MethodInfoOfType = typeof(Queryable).GetMethods().FirstOrDefault(x => x.Name == "OfType"); // OfType
        }

        public virtual object DbSet(DbContext context, Type implementationType)
        {
            var methodInfo = MethodInfoSet.MakeGenericMethod(implementationType)!;

            return methodInfo.Invoke(context, null)!;
        }

        private IQueryable<TEntity> OfType<TEntity>(object set) where TEntity : class
        {
            return (IQueryable<TEntity>)MethodInfoOfType.MakeGenericMethod(typeof(TEntity)).Invoke(null, new object[] { set })!;
        }

        public IQueryable<TEntity> Find<TEntity>(DbContext context, Type implementationType) where TEntity : class
        {
            var set = DbSet(context, implementationType);
            return OfType<TEntity>(set);
        }
    }
}
