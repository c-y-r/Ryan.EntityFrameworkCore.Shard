using Microsoft.Extensions.DependencyInjection;
using Ryan.DependencyInjection;
using Ryan.EntityFrameworkCore.Caches;
using System;
using Xunit;

namespace Ryan.EntityFrameworkCore.Shard.Tests
{
    public class ShardDbContextTests : IDisposable
    {
        public IServiceProvider ServiceProvider { get; }

        public ShardDbContextTests()
        {
            ServiceCollection sc = new ServiceCollection();
            sc.AddSingleton<ShardDescriptor<RyanModel>, RyanModelDescriptor>();
            sc.AddSingleton<RyanModelModelBuilder>();
            sc.AddSingleton<IShardMapper<RyanModel>, RyanModelMapper>();
            sc.AddSingleton<ShardCache<RyanModel>>();
            sc.AddTransient<ShardDbContext>();
            sc.AddScoped<ShardDbContext<ShardDbContext>>();

            ServiceProvider = sc.BuildServiceProvider();

            RyanService.Replace(ServiceProvider);
        }

        [Fact]
        public void Test1()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                // scope.ServiceProvider.GetService<ShardCache<RyanModel>>().Tables.Add(ShardTableCache.FromTableName("RyanModels_2023", typeof(RyanModel)));

                var dbContext = scope.ServiceProvider.GetService<ShardDbContext<ShardDbContext>>();
                dbContext.Add(new RyanModel()
                {
                    Name = "Name",
                    Year = 2023
                });

                dbContext.DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
        }
    }
}
