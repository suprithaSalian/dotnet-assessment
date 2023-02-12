using Microsoft.EntityFrameworkCore;
using TimeChimp.Backend.Assessment.DataModel;

namespace TimeChimp.Backend.Assessment.Repository
{
    public class RssDbContext : DbContext
    {
        public RssDbContext()
        {

        }
        public RssDbContext(DbContextOptions<RssDbContext> options):base(options) 
        {
            
        }

        public virtual DbSet<RssItem> Items { get; set; } = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name =ConnectionStrings.RssDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("db_owner");
            modelBuilder.Entity<RssItem>(entity =>
            {
                entity.HasKey(e => e.Guid);
                entity.ToTable(nameof(RssItem), "dbo");
                entity.Property(e => e.RssId).ValueGeneratedOnAdd();
            });
        }
    }
}
