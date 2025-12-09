using Microsoft.EntityFrameworkCore;
using HMCT.Models;
namespace HMCT
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Creates a new instance of the application DB context with the given options.
        /// </summary>
        /// <param name="options">Configuration options for the DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<TaskItem> TasksItem { get; set; }
        /// <summary>
        /// Configures entity mappings and constraints when the model is being created.
        /// </summary>
        /// <param name="modelBuilder">Builder used to configure entity schemas.</param>
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
