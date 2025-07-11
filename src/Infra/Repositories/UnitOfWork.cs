using Application.Interfaces;
using Domain.Interfaces;
using Infra.Data;

namespace Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ITaskRepository TaskRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            TaskRepository = new TaskRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
