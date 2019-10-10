using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VN.Example.Platform.Domain.BehaviorAggregation;

namespace VN.Example.Infrastructure.Database.MSSQL.Configurations
{
    public class BehaviorConfiguration : IEntityTypeConfiguration<Behavior>
    {
        public void Configure(EntityTypeBuilder<Behavior> builder)
        {
            builder.ToTable("Behaviors").HasIndex(b => b.Id);
            builder.Property(b => b.Id);
            builder.Property(b => b.IP).IsRequired();
            builder.Property(b => b.PageName).HasColumnName("Name").IsRequired();
            builder.Property(b => b.UserAgent).IsRequired();
            builder.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");

            // (de)serialize property
            builder.Property(b => b.PageParameters)
                   .HasColumnName("Parameters")
                   .HasConversion(
                        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                        v => JsonConvert.DeserializeObject<JObject>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                   );

            // indexes
            builder.HasIndex(b => new { b.IP, b.PageName, b.UserAgent })
                   .IsUnique();
        }
    }
}
