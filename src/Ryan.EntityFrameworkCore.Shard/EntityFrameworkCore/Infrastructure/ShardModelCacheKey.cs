using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ryan.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ryan.EntityFrameworkCore.Infrastructure
{
    /// <inheritdoc/>
    public class ShardModelCacheKey : ModelCacheKey
    {
        /// <summary>
        /// 分表数量
        /// </summary>
        public int Count { get; protected set; }

        public ShardModelCacheKey([NotNull] DbContext context) : base(context)
        {
            Count = ShardService.GetTypeImplementGenericInterfaces(typeof(IShard<>), context.GetType()).Sum(shardType => 
            {
                var cache = RyanService.ServiceProvider.GetService(typeof(ShardCache<>).MakeGenericType(shardType.GenericTypeArguments[0])) as ShardCache;
                return cache.TableNames.Count;
            });
        }

        public override bool Equals(object obj)
        {
            if (obj is ShardModelCacheKey other)
            {
                return Count == other.Count;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
