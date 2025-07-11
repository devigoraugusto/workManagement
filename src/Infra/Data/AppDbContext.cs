using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TaskItem> TaskItems { get; set; } = null!;
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TaskItem>()
        //        .Property(t => t.Status)
        //        .HasConversion<string>();
        //}
    }
}
