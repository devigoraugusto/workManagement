using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        ITaskRepository TaskRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
