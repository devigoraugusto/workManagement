using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;

namespace Tests.Services
{
    public class TarskServiceTests
    {
        [Fact]
        public async Task AddTaskAsync_CallsRepositoryAndSave()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);

            var task = new TaskItem
            {
                Title = "Test Task 1",
                Description = "Task de exemplo número 1",
                Status = TaskStatusEnum.Pending,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            await service.CreateTaskAsync(task);

            mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<TaskItem>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
