using Microsoft.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Caches;

namespace Ryan.EntityFrameworkCore.Shard.Tests
{
    public class RyanModelModelBuilder : ShardModelBuilder<RyanModel>
    {
        public RyanModelModelBuilder(ShardCache<RyanModel> shardCache) : base(shardCache)
        {
        }

        public override void Apply<TEntityImpl>(ModelBuilder modelBuilder, ShardTableCache cache)
        {
            modelBuilder.Entity<TEntityImpl>(builder => 
            {
                builder.ToTable(cache.TableName).HasKey(x => x.Id).IsClustered(false);

                builder.Property(x => x.Year);
                builder.Property(x => x.Name);
            });
        }
    }
}
