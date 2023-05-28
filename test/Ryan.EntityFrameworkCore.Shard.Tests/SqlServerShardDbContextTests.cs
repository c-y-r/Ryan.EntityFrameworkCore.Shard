using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ryan.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Builder;
using Ryan.EntityFrameworkCore.ModelBuilders;
using Ryan.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ryan
{
    public class SqlServerShardDbContextTests : IDisposable
    {
        public ServiceProvider ServiceProvider { get; set; }

        public SqlServerShardDbContextTests()
        {
            var collection = new ServiceCollection();
            collection.AddShard();
            collection.AddSingleton<EntityModelBuilder<M>, MEntityModelBuilder>();
            collection.AddScoped<SqlServerShardDbContext>();

            ServiceProvider = collection.BuildServiceProvider();

            // 获取分表配置
            var entityShardConfiguration = ServiceProvider.GetRequiredService<IEntityShardConfiguration>();
            entityShardConfiguration.AddShard<M>("M_2022");
            entityShardConfiguration.AddShard<M>("M_2023");
        }

        [Fact]
        public async Task Query()
        {
            var context = ServiceProvider.GetRequiredService<SqlServerShardDbContext>();

            var years = new[] { 2022, 2023 };
            var collection = await context.AsQueryable<M>(x => years.Contains(x.Year)).ToListAsync();

            Assert.True(collection.Any());
        }

        public void Dispose()
        {
            ServiceProvider.Dispose();
        }
    }
}
