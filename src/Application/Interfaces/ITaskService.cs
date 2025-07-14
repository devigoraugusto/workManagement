using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksItemsAsync(TaskStatusEnum? status, DateTime? dueDate);
        Task<TaskItem> GetTaskItemByIdAsync(int id);
        Task CreateTaskAsync(CreateTaskDto task);
        Task UpdateTaskAsync(TaskItem task);
        Task DeleteTaskAsync(int id);
    }
}
