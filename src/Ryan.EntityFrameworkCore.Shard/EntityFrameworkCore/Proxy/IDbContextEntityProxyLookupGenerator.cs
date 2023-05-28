using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <summary>
    /// 上下文实体代理字典生成器
    /// </summary>
    public interface IDbContextEntityProxyLookupGenerator
    {
        /// <summary>
        /// 创建上下文实体代理字典
        /// </summary>
        public DbContextEntityProxyLookup Create(DbContext dbContext);

        /// <summary>
        /// 删除
        /// </summary>
        void Delete(DbContext dbContext);
    }
}
