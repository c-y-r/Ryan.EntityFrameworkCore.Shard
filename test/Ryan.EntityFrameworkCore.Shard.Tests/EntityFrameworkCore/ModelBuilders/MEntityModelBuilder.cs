using Microsoft.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Builder;
using Ryan.Models;
using System.Collections.Generic;

namespace Ryan.EntityFrameworkCore.ModelBuilders
{
    public class MEntityModelBuilder : EntityModelBuilder<M>
    {
        public override void Build<TImplementation>(ModelBuilder modelBuilder, string tableName)
        {
            modelBuilder.Entity<TImplementation>(b => 
            {
                b.ToTable(tableName).HasKey(x => x.Id).IsClustered(false);

                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Year).IsRequired();
                b.Property(x => x.Name).IsRequired().HasMaxLength(50);
            });
        }

        public override string GetTableName(Dictionary<string, string> value)
        {
            return $"M_{value["Year"]}";
        }

        protected override void EntityConfiguration()
        {
            Apply(x => x.Year);
        }
    }
}
