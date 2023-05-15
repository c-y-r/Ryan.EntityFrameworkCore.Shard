using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ryan.DependencyInjection;

namespace Ryan.EntityFrameworkCore
{
    /// <summary>
    /// 分表上下文
    /// </summary>
    public class ShardDbContext<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 实际上下文
        /// </summary>
        public TDbContext DbContext { get; protected set; }

        /// <summary>
        /// 创建分表上下文
        /// </summary>
        public ShardDbContext()
        {
            ReloadDbContext();
        }

        /// <summary>
        /// 重载上下文
        /// </summary>
        public void ReloadDbContext()
        {
            DbContext = RyanService.ServiceProvider.GetService<TDbContext>();
        }
    }
}
