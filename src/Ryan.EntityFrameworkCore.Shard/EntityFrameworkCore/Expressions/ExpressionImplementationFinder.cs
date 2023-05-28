using Ryan.EntityFrameworkCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ryan.EntityFrameworkCore.Expressions
{
    /// <inheritdoc cref="IExpressionImplementationFinder"/>
    public class ExpressionImplementationFinder : IExpressionImplementationFinder
    {
        /// <summary>
        /// 访问器生成器
        /// </summary>
        public IEntityModelBuilderAccessorGenerator EntityModelBuilderAccessorGenerator { get; }

        /// <summary>
        /// 创建表达式实现查询器
        /// </summary>
        public ExpressionImplementationFinder(IEntityModelBuilderAccessorGenerator entityModelBuilderAccessorGenerator)
        {
            EntityModelBuilderAccessorGenerator = entityModelBuilderAccessorGenerator;
        }

        /// <inheritdoc/>
        public IEnumerable<Type> Find<TEntity>(Expression expression)
        {
            // 访问器
            var accessor = EntityModelBuilderAccessorGenerator.Create(typeof(TEntity));
            var builder = (IEntityModelBuilder)accessor.EntityModelBuilder;
            var visitors = builder.GetExpressionVisitors().ToList();
            visitors.ForEach(x => x.Visit(node: expression));

            // 获取结果
            var pairs = visitors.Select(x => new KeyValuePair<string, HashSet<string>>(x.MemberExpression.Member.Name, x.Values));
            var result = GetCombinations(new Dictionary<string, HashSet<string>>(pairs));

            // 获取实现
            var tableNames = result.Select(x => builder.GetTableName(new Dictionary<string, string>(x)));
            return tableNames.Select(x => accessor.Dictionary[x].ImplementationType);
        }

        List<List<KeyValuePair<string, string>>> GetCombinations(Dictionary<string, HashSet<string>> dictionary)
        {
            List<List<KeyValuePair<string, string>>> combinations = new List<List<KeyValuePair<string, string>>>();
            GetCombinationsHelper(dictionary, new List<KeyValuePair<string, string>>(), combinations);
            return combinations;
        }

        void GetCombinationsHelper(Dictionary<string, HashSet<string>> dictionary, List<KeyValuePair<string, string>> currentCombination, List<List<KeyValuePair<string, string>>> combinations)
        {
            if (dictionary.Count == 0)
            {
                combinations.Add(currentCombination);
                return;
            }

            string key = dictionary.Keys.First();
            HashSet<string> values = dictionary[key];
            dictionary.Remove(key);

            foreach (string value in values)
            {
                List<KeyValuePair<string, string>> newCombination = new List<KeyValuePair<string, string>>(currentCombination);
                newCombination.Add(new KeyValuePair<string, string>(key, value));
                GetCombinationsHelper(dictionary, newCombination, combinations);
            }

            dictionary.Add(key, values);
        }
    }
}
