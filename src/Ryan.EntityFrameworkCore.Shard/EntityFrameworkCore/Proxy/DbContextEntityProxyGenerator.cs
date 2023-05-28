using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <inheritdoc cref="IDbContextEntityProxyGenerator"/>
    public class DbContextEntityProxyGenerator : IDbContextEntityProxyGenerator
    {
        /// <inheritdoc/>
        public DbContextEntityProxy Create(DbContext context)
        {
            return new DbContextEntityProxy(context);
        }
    }
}
