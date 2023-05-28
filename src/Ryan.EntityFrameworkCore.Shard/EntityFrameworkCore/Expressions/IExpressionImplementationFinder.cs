using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ryan.EntityFrameworkCore.Expressions
{
    /// <summary>
    /// 查找实现
    /// </summary>
    public interface IExpressionImplementationFinder
    {
        /// <summary>
        /// 查找实现
        /// </summary>
        public IEnumerable<Type> Find<TEntity>(Expression expression);
    }
}
