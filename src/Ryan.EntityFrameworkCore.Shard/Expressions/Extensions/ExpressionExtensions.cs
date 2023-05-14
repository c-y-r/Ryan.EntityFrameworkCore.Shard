using System.Linq.Expressions;
using System;
using Ryan.Expressions.Extensions;

namespace Ryan.Expressions
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 应用分表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TCommon">实体属性</typeparam>
        /// <param name="descriptor">分表配置</param>
        /// <param name="expression">成员表达式</param>
        /// <returns>分表配置</returns>
        public static ShardDescriptor<T> Apply<T, TCommon>(this ShardDescriptor<T> descriptor, Expression<Func<T, TCommon>> expression) where T : class
        {
            descriptor.ShardExpressionVisitorFuncs.Add(() =>
                new ShardExpressionVisitor<TCommon>(expression));

            return descriptor;
        }

        /// <summary>
        /// 应用分表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="descriptor">实体属性</param>
        /// <param name="expression">成员表达式</param>
        /// <param name="format">日期格式化</param>
        /// <returns>分表配置</returns>
        public static ShardDescriptor<T> Apply<T>(this ShardDescriptor<T> descriptor, Expression<Func<T, DateTime>> expression, string format) where T : class
        {
            descriptor.ShardExpressionVisitorFuncs.Add(() =>
                new DateTimeShardExpressionVisitor(expression, format));

            return descriptor;
        }
    }
}
