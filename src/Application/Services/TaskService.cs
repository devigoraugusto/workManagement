using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTaskAsync(TaskItem task)
        {
            await _unitOfWork.TaskRepository.CreateAsync(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task !=null)
            {
                _unitOfWork.TaskRepository.DeleteAsync(task);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }
        }

        public Task<TaskItem> GetTaskItemByIdAsync(int id)
        {
            return _unitOfWork.TaskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksItemsAsync(TaskStatusEnum? status, DateTime? dueDate)
        {
            return await _unitOfWork.TaskRepository.GetAllAsync(status, dueDate);
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
