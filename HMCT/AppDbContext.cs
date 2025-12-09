using Microsoft.EntityFrameworkCore;
using HMCT.Models;
namespace HMCT
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<TaskItem> TasksItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskItem>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Title).IsRequired().HasMaxLength(25);
                    entity.Property(e => e.Description).HasMaxLength(150);
                    entity.Property(e => e.DueDateTime).IsRequired();
                    entity.Property(e => e.Taskstatus);

                });
        }
    }
}
