using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infra.Data;

namespace Infra.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<TaskItem> CreateAsync(TaskItem task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task cannot be null");
            }

            return Task.FromResult(_context.TaskItems.Add(task).Entity);
        }

        public void DeleteAsync(TaskItem task)
        {
            _context.TaskItems.Remove(task);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(TaskStatusEnum? status = null, DateTime? dueDate = null)
        {
            var query = _context.TaskItems.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }
            if (dueDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date == dueDate.Value.Date);
            }

            return await query.ToListAsync();
        }

        public async Task<PagedResult<TaskItem>> GetAllPaginatedAsync(PaginationParams paginationParams, TaskStatusEnum? status = null, DateTime? dueDate = null)
        {
            var query = _context.TaskItems.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date == dueDate.Value.Date);
            }

            var totalCount = await query.CountAsync();

            var pagedItems = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return new PagedResult<TaskItem>(
                pagedItems,
                totalCount,
                paginationParams.PageNumber,
                paginationParams.PageSize
            );
        }

        public async Task<TaskItem> GetByIdAsync(int id) => await _context.TaskItems.FindAsync(id);

        public void UpdateAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
        }
    }
}
