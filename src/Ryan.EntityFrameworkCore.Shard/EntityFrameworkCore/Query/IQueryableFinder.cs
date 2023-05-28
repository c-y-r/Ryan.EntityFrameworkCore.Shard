using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Query
{
    /// <summary>
    /// 查询搜索器
    /// </summary>
    public interface IQueryableFinder
    {
        /// <summary>
        /// 查询
        /// </summary>
        IQueryable<TEntity> Find<TEntity>(DbContext context, Type implementationType) where TEntity : class;
    }
}
