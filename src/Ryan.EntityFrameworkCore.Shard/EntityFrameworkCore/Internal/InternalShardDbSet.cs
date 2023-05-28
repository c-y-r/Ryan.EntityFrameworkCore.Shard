using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ryan.EntityFrameworkCore.Internal
{
    public class InternalShardDbSet<TEntity> : DbSet<TEntity> where TEntity : class
    {
        /// <summary>
        /// 直接异常
        /// </summary>
        public override IEntityType EntityType => throw new NotImplementedException();

        public override IQueryable<TEntity> AsQueryable()
        {
            return base.AsQueryable();
        }
    }
}
