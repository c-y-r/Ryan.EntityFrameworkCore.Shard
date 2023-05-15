using Ryan.Expressions;

namespace Ryan.EntityFrameworkCore.Shard.Tests
{
    /// <inheritdoc/>
    public class RyanModelDescriptor : ShardDescriptor<RyanModel>
    {
        public RyanModelDescriptor()
        {
            this.Apply(x => x.Year);
        }
    }
}
