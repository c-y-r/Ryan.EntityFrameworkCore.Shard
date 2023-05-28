using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <summary>
    /// 数据上下文实体代理生成器
    /// </summary>
    public interface IDbContextEntityProxyGenerator
    {
        /// <summary>
        /// 创建上下文实体代理
        /// </summary>
        DbContextEntityProxy Create(DbContext context);
    }
}
