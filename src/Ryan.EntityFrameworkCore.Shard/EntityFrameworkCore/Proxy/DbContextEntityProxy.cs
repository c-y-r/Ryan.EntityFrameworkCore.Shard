using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <summary>
    /// 上下文实体代理
    /// </summary>
    public class DbContextEntityProxy
    {
        /// <summary>
        /// 上下文
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        /// 实体代理
        /// </summary>
        public List<EntityProxy> EntityProxies { get; }

        /// <summary>
        /// 创建上下文实体代理
        /// </summary>
        public DbContextEntityProxy(DbContext context)
        {
            Context = context;
            EntityProxies = new List<EntityProxy>();
        }
    }
}
