using Microsoft.EntityFrameworkCore;
using Ryan.DependencyInjection;
using Ryan.Models;

namespace Ryan.EntityFrameworkCore
{
    [Shard(typeof(M))]
    public class SqlServerShardDbContext : ShardDbContext
    {
        public SqlServerShardDbContext(IShardDependency shardDependency) : base(shardDependency)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(local);Database=M;User Id=sa;Password=sa;");
        }
    }
}
