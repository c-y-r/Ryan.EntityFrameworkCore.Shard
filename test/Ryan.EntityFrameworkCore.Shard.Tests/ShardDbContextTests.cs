using Microsoft.Extensions.DependencyInjection;
using Ryan.DependencyInjection;
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
            sc.AddSingleton<RyanModelDescriptor>();
            sc.AddSingleton<RyanModelModelBuilder>();
            sc.AddSingleton<ShardCache<RyanModel>>();
            sc.AddSingleton<ShardDbContext>();

            ServiceProvider = sc.BuildServiceProvider();

            RyanService.Replace(ServiceProvider);
        }

        [Fact]
        public void Test1()
        {
            using var dbContext = ServiceProvider.GetService<ShardDbContext>();
            dbContext.Set<RyanModel>().Add(new RyanModel()
            {
                Name = "test",
                Year = 2023
            });

            dbContext.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}
