using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Ryan.Expressions.Caches
{
    /// <summary>
    /// Member 缓存
    /// </summary>
    internal class MemberCache<T>
    {
        /// <summary>
        /// 缓存
        /// </summary>
        public static Dictionary<MemberExpression, Func<T>> CACHE = new Dictionary<MemberExpression, Func<T>>();

        /// <summary>
        /// 获取委托
        /// </summary>
        /// <param name="memberExpression">成员表达式</param>
        /// <returns>委托</returns>
        public static Func<T> GetFunc(MemberExpression memberExpression)
        {
            if (!CACHE.TryGetValue(memberExpression, out var func))
            {
                CACHE[memberExpression] = func = Expression.Lambda<Func<T>>(memberExpression).Compile();
            }

            return func;
        }
    }
}
