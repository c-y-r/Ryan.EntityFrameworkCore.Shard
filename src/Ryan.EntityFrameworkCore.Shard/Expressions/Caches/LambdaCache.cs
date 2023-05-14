using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Ryan.Expressions.Caches
{
    /// <summary>
    /// Lambda 缓存
    /// </summary>
    internal class LambdaCache<T>
    {
        /// <summary>
        /// 缓存
        /// </summary>
        public static Dictionary<LambdaExpression, Func<object, T>> CACHE = new Dictionary<LambdaExpression, Func<object, T>>();

        /// <summary>
        /// 获取委托
        /// </summary>
        /// <param name="lambdaExpression">成员表达式</param>
        /// <returns>委托</returns>
        public static Func<object, T> GetFunc(LambdaExpression lambdaExpression)
        {
            if (!CACHE.TryGetValue(lambdaExpression, out var func))
            {
                ParameterExpression newParam = Expression.Parameter(typeof(object), "x");
                Expression<Func<object, T>> newLambda = Expression.Lambda<Func<object, T>>(
                    Expression.Convert(Expression.Invoke(lambdaExpression, Expression.Convert(newParam, lambdaExpression.Parameters[0].Type)), typeof(T)),
                    newParam
                );

                CACHE[lambdaExpression] = func = newLambda.Compile();
            }

            return func;
        }
    }
}
