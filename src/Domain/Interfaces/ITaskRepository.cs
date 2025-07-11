using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(TaskStatusEnum? status = null, DateTime? dueDate = null);
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        void UpdateAsync(TaskItem task);
        void DeleteAsync(TaskItem task);
    }
}
