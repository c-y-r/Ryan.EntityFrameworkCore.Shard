using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ryan.EntityFrameworkCore.Infrastructure
{
    /// <inheritdoc/>
    public class ShardModelCacheKeyFactory : IModelCacheKeyFactory
    {
        /// <inheritdoc/>
        public object Create(DbContext context)
        {
            return new ShardModelCacheKey(context);
        }
    }
}
