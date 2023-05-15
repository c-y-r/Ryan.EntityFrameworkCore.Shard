using Ryan.Expressions;

namespace Ryan.EntityFrameworkCore.Shard.Tests
{
    /// <inheritdoc/>
    public class RyanModelDescriptor : ShardDescriptor<RyanModel>
    {
        public RyanModelDescriptor()
        {
            this.Template = "RyanModels_{Year}";
            this.Apply(x => x.Year);
        }
    }
}
