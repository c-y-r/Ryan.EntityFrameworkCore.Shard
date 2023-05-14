using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using Ryan.Expressions;

namespace Ryan
{
    /// <summary>
    /// 分表描述
    /// </summary>
    public abstract class ShardDescriptor<TEntity> where TEntity : class
    {
        /// <summary>
        /// 模板
        /// </summary>
        public virtual string Template { get; set; }

        /// <summary>
        /// 分表属性访问委托
        /// </summary>
        public virtual List<Func<ShardExpressionVisitor>> ShardExpressionVisitorFuncs { get; } = new List<Func<ShardExpressionVisitor>>();

        /// <summary>
        /// 从实体中获取表名
        /// </summary>
        public virtual IEnumerable<string> TablesFromEntities(IEnumerable<TEntity> entities)
        {
            // 获取键值对
            var pairs = ShardExpressionVisitorFuncs.Select(func =>
            {
                var v = func();
                v.Visit(entities.Select(e => (object)e));
                return new KeyValuePair<string, IEnumerable<string>>(v.MemberExpression.Member.Name, v.Values);
            });

            // 组成字典
            var dict = new Dictionary<string, IEnumerable<string>>(pairs);

            return TablesFromDictionary(dict);
        }

        /// <summary>
        /// 从表达式中获取表名
        /// </summary>
        public virtual IEnumerable<string> TablesFromExpression(Expression expression)
        {
            // 获取键值对
            var pairs = ShardExpressionVisitorFuncs.Select(func =>
            {
                var v = func();
                v.Visit(expression);
                return new KeyValuePair<string, IEnumerable<string>>(v.MemberExpression.Member.Name, v.Values);
            });

            // 组成字典
            var dict = new Dictionary<string, IEnumerable<string>>(pairs);

            return TablesFromDictionary(dict);
        }

        /// <summary>
        /// 从字典中获取表名
        /// </summary>
        protected virtual IEnumerable<string> TablesFromDictionary(Dictionary<string, IEnumerable<string>> dict)
        {
            // 计算可能的组合数
            var count = dict.Values.Aggregate(1, (acc, value) => acc * value.Count());

            // 生成所有可能的组合
            var combinations = Enumerable.Range(0, count)
                .Select(index =>
                {
                    var combination = new Dictionary<string, string>();
                    var i = 0;
                    foreach (var key in dict.Keys)
                    {
                        var values = dict[key].ToArray();
                        var valueIndex = index / (int)Math.Pow(values.Length, i) % values.Length;
                        combination.Add(key, values[valueIndex]);
                        i++;
                    }

                    return combination;
                });

            // 通过模板组成结果
            return combinations.Select(combination => combination.Aggregate(Template, (t, c) => t.Replace($"{{{c.Key}}}", c.Value)));
        }
    }
}
