using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TaskItem> TaskItems { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed para TaskItems
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Task 1",
                    Description = "Description for Task 1",
                    Status = TaskStatusEnum.Pending,
                    DueDate = DateTime.Today.AddDays(3)
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Task 2",
                    Description = "Description for Task 2",
                    Status = TaskStatusEnum.InProgress,
                    DueDate = DateTime.Today.AddDays(5)
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Task 3",
                    Description = "Description for Task 3",
                    Status = TaskStatusEnum.Completed,
                    DueDate = DateTime.Today.AddDays(7)
                },
                new TaskItem
                {
                    Id = 4,
                    Title = "Urgent Fix",
                    Description = "Urgent fix for production issue",
                    Status = TaskStatusEnum.InProgress,
                    DueDate = DateTime.Today
                },
                new TaskItem
                {
                    Id = 5,
                    Title = "Review Docs",
                    Description = "Review API documentation",
                    Status = TaskStatusEnum.Pending,
                    DueDate = DateTime.Today.AddDays(2)
                },
                new TaskItem
                {
                    Id = 6,
                    Title = "Team Meeting",
                    Description = "Weekly team sync-up meeting",
                    Status = TaskStatusEnum.Pending,
                    DueDate = DateTime.Today.AddDays(1)
                },
                new TaskItem
                {
                    Id = 7,
                    Title = "Code Refactor",
                    Status = TaskStatusEnum.Pending,
                    Description = "Refactor"
                },
                new TaskItem
                {
                    Id = 8,
                    Title = "Database Migration",
                    Status = TaskStatusEnum.InProgress,
                    Description = "Migrate"
                },
                new TaskItem
                {
                    Id = 9,
                    Title = "Performance Optimization",
                    Description = "Optimize",
                    Status = TaskStatusEnum.Completed,
                },
                new TaskItem
                {
                    Id = 10,
                    Title = "Security Audit",
                    Description = "Conduct security audit",
                    Status = TaskStatusEnum.Pending,
                    DueDate = DateTime.Today.AddDays(4)
                }
            );
        }
    }
}
