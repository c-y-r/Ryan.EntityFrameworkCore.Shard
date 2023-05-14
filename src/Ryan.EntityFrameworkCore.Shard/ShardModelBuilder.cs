using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace Ryan
{
    /// <summary>
    /// 分表 ModelBuilder
    /// </summary>
    public abstract class ShardModelBuilder<TEntity>
    {
        protected virtual void ApplyShard()
        {
        }

        /// <summary>
        /// 配置 ModelBuilder
        /// </summary>
        public virtual void Apply(ModelBuilder modelBuilder)
        {
            var type = ShardService.GetTypeImplementGenericType(typeof(ShardModelBuilder<>), this.GetType()).GenericTypeArguments[0];
            var annotations = modelBuilder.Model.FindEntityType(type).GetAnnotations();

        }
    }
}
