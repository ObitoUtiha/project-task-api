using Microsoft.EntityFrameworkCore;
using ProjectTaskApi.Entities;

namespace ProjectTaskApi.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<TaskItem> Tasks { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options):
            base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationContext).Assembly );
        }
    }
}
