using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Ryan.EntityFrameworkCore.Infrastructure;
using System;

namespace Ryan.EntityFrameworkCore.Shard.Tests
{
    public class ShardDbContext : DbContext, IShard<RyanModel>
    {
        public IServiceProvider ServiceProvider { get; }

        public ShardDbContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, ShardModelCacheKeyFactory>();

            optionsBuilder.UseSqlServer("Data Source = (local); Database = M; User Id = sa; Password = sa;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ServiceProvider.GetService<RyanModelModelBuilder>().ApplyShard(modelBuilder);
        }
    }
}
