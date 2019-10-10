using Microsoft.EntityFrameworkCore;
using VN.Example.Infrastructure.Database.MSSQL.Configurations;

namespace VN.Example.Infrastructure.Database.MSSQL
{
    public class ExampleContext : BaseContext
    {
        // for EF migrations only
        public ExampleContext()
            : base("Server=localhost;Database=example;User Id=sa;Password=S3cr3t123;")
        { }

        public ExampleContext(IDataSource source)
            : base(source)
        {
            Database.EnsureCreated();
            //Database.Migrate(); // applies any pending migrations
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            //optionsBuilder.UseLazyLoadingProxies(); // use lazy in a more complex scenario

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BehaviorConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
