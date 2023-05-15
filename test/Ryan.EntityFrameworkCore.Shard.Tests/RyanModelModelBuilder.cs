using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Shard.Tests
{
    public class RyanModelModelBuilder : ShardModelBuilder<RyanModel>
    {
        public ShardCache<RyanModel> ShardCache { get; }

        public RyanModelModelBuilder(ShardCache<RyanModel> shardCache)
        {
            ShardCache = shardCache;
        }
    }
}
