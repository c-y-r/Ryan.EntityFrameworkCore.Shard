using Microsoft.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Builder;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <inheritdoc cref="IEntityProxyGenerator"/>
    public class EntityProxyGenerator : IEntityProxyGenerator
    {
        /// <inheritdoc cref="IEntityModelBuilderGenerator"/>
        public IEntityModelBuilderGenerator EntityModelBuilderGenerator { get; }

        /// <inheritdoc cref="IEntityImplementationDictionaryGenerator"/>
        public IEntityImplementationDictionaryGenerator EntityImplementationDictionaryGenerator { get; }

        /// <summary>
        /// 创建实体代理
        /// </summary>
        public EntityProxyGenerator(
            IEntityModelBuilderGenerator entityModelBuilderGenerator
            , IEntityImplementationDictionaryGenerator entityImplementationDictionaryGenerator)
        {
            EntityModelBuilderGenerator = entityModelBuilderGenerator;
            EntityImplementationDictionaryGenerator = entityImplementationDictionaryGenerator;
        }

        /// <inheritdoc/>
        public EntityProxy Create(object entity, EntityProxyType type, DbContext dbContext)
        {
            if (type == EntityProxyType.NonQuery)
            {
                var builder = (EntityModelBuilderGenerator.Create(entity.GetType()) as IEntityModelBuilder)!;
                var visitors = builder.GetExpressionVisitors().ToList();
                foreach (var visitor in visitors)
                {
                    visitor.Visit(entity);
                }

                var pairs = visitors.Select(x => new KeyValuePair<string, string?>(x.MemberExpression.Member.Name, x.Values.FirstOrDefault()));
                var dictionary = new Dictionary<string, string>(pairs!);
                var tableName = builder.GetTableName(dictionary);

                var ei = EntityImplementationDictionaryGenerator.Create(entity.GetType())[tableName];
                var entityImplementation = Activator.CreateInstance(ei.ImplementationType)!;
                return new EntityProxy(entity, entityImplementation, type, dbContext);
            }

            return new EntityProxy(entity, null, type, dbContext);
        }
    }
}
