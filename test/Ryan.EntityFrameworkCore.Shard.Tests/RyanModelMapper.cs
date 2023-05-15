using System;
using Ryan.EntityFrameworkCore.Mapper;

namespace Ryan.EntityFrameworkCore.Shard.Tests
{
    public class RyanModelMapper : IShardMapper<RyanModel>
    {
        public RyanModel Map(RyanModel entity, Type shardType)
        {
            var m = (RyanModel)Activator.CreateInstance(shardType);

            m.Id = entity.Id;
            m.Name = entity.Name;
            m.Year = entity.Year;

            return m;
        }
    }
}
