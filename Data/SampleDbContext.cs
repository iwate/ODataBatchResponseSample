using Microsoft.EntityFrameworkCore;
using ODataBatchResponseSample.Entities;

namespace ODataBatchResponseSample.Data
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Datum> Data { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Datum>().HasKey(o => o.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
