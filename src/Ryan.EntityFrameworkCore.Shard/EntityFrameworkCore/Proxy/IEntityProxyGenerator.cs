using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <summary>
    /// 实体代理生成器
    /// </summary>
    public interface IEntityProxyGenerator
    {
        /// <summary>
        /// 生成实体代理
        /// </summary>
        EntityProxy Create(object entity, EntityProxyType type, DbContext dbContext);
    }
}
