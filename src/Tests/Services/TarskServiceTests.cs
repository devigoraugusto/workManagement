using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
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

            var taskDto = new CreateTaskDto
            {
                Title = "Test Task 1",
                Description = "Task de exemplo número 1",
                Status = TaskStatusEnum.Pending,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            await service.CreateTaskAsync(taskDto);

            mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<TaskItem>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsCorrectTask()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var taskId = 1;
            var expectedTask = new TaskItem
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Task de exemplo",
                Status = TaskStatusEnum.Pending,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(expectedTask);

            var result = await service.GetTaskItemByIdAsync(taskId);

            Assert.Equal(expectedTask, result);
        }

        [Fact]
        public async Task UpdateTaskAsync_UpdatesExistingTask()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var taskToUpdate = new TaskItem
            {
                Id = 1,
                Title = "Updated Task",
                Description = "Updated description",
                Status = TaskStatusEnum.Completed,
                DueDate = DateTime.UtcNow.AddDays(2)
            };

            await service.UpdateTaskAsync(taskToUpdate);

            mockRepository.Setup(repo => repo.GetByIdAsync(taskToUpdate.Id)).ReturnsAsync(taskToUpdate);
            mockRepository.Verify(repo => repo.UpdateAsync(taskToUpdate), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);

        }

        [Fact]
        public async Task DeleteTaskAsync_DeletesExistingTask()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var taskId = 1;

            mockRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(new TaskItem { Id = taskId });

            await service.DeleteTaskAsync(taskId);

            mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<TaskItem>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);

        }

        [Fact]
        public async Task GetTasksItemsAsync_ReturnsAllTasks()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);

            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", Description = "Description 1", Status = TaskStatusEnum.Pending, DueDate = DateTime.UtcNow.AddDays(1) },
                new TaskItem { Id = 2, Title = "Task 2", Description = "Description 2", Status = TaskStatusEnum.Completed, DueDate = DateTime.UtcNow.AddDays(2) }
            };

            mockRepository.Setup(repo => repo.GetAllAsync(null, null)).ReturnsAsync(expectedTasks);

            var result = await service.GetTasksItemsAsync(null, null);

            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTasksItemsAsync_WithStatusAndDueDate_ReturnsFilteredTasks()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var status = TaskStatusEnum.Pending;
            var dueDate = DateTime.UtcNow.AddDays(1);
            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", Description = "Description 1", Status = status, DueDate = dueDate }
            };

            mockRepository.Setup(repo => repo.GetAllAsync(status, dueDate)).ReturnsAsync(expectedTasks);

            var result = await service.GetTasksItemsAsync(status, dueDate);

            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task CreateTaskAsync_ValidatesTaskDto()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var taskDto = new CreateTaskDto
            {
                Title = "Valid Task",
                Description = "This is a valid task description.",
                Status = TaskStatusEnum.Pending,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            await service.CreateTaskAsync(taskDto);

            mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<TaskItem>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        //[Fact]
        //public async Task CreateTaskAsync_InvalidTaskDto_ThrowsValidationException()
        //{
        //    var mockRepository = new Mock<ITaskRepository>();
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

        //    var service = new TaskService(mockUnitOfWork.Object);
        //    var taskDto = new CreateTaskDto
        //    {
        //        Title = "", // Invalid title
        //        Description = "This is a valid task description.",
        //        Status = TaskStatusEnum.Pending,
        //        DueDate = DateTime.UtcNow.AddDays(1)
        //    };

        //    Assert.ThrowsAsync<ValidationException>(async () => await service.CreateTaskAsync(taskDto));

        //    mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<TaskItem>()), Times.Never);
        //    mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        //    mockUnitOfWork.Verify(uow => uow.TaskRepository, Times.Never);
        //    mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        //    mockUnitOfWork.Verify(uow => uow.TaskRepository.GetByIdAsync(It.IsAny<int>()), Times.Never);
        //    mockUnitOfWork.Verify(uow => uow.TaskRepository.GetAllAsync(It.IsAny<TaskStatusEnum?>(), It.IsAny<DateTime?>()), Times.Never);
        //    mockUnitOfWork.Verify(uow => uow.TaskRepository.CreateAsync(It.IsAny<TaskItem>()), Times.Never);
            
        //}
        
        [Fact]
        public async Task DeleteTaskAsync_TaskNotFound_ThrowsKeyNotFoundException()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var taskId = 1;

            mockRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((TaskItem)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteTaskAsync(taskId));

            mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<TaskItem>()), Times.Never);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }

        //[Fact]
        //public async Task UpdateTaskAsync_TaskNotFound_ThrowsKeyNotFoundException()
        //{
        //    var mockRepository = new Mock<ITaskRepository>();
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

        //    var service = new TaskService(mockUnitOfWork.Object);
        //    var taskToUpdate = new TaskItem
        //    {
        //        Id = 1,
        //        Title = "Non-existent Task",
        //        Description = "This task does not exist.",
        //        Status = TaskStatusEnum.Pending,
        //        DueDate = DateTime.UtcNow.AddDays(1)
        //    };

        //    mockRepository.Setup(repo => repo.GetByIdAsync(taskToUpdate.Id)).ReturnsAsync((TaskItem)null);

        //    await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateTaskAsync(taskToUpdate));

        //    mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
        //    mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        //}

        [Fact]
        public async Task GetTaskByIdAsync_TaskNotFound_ReturnsNull()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var taskId = 1;

            mockRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((TaskItem)null);

            var result = await service.GetTaskItemByIdAsync(taskId);

            Assert.Null(result);
        }

        //[Fact]
        //public async Task GetTasksItemsAsync_NoFilters_ReturnsAllTasks()
        //{
        //    var mockRepository = new Mock<ITaskRepository>();
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

        //    var service = new TaskService(mockUnitOfWork.Object);
        //    var expectedTasks = new List<TaskItem>
        //    {
        //        new TaskItem { Id = 1, Title = "Task 1", Description = "Description 1", Status = TaskStatusEnum.Pending, DueDate = DateTime.UtcNow.AddDays(1) },
        //        new TaskItem { Id = 2, Title = "Task 2", Description = "Description 2", Status = TaskStatusEnum.Completed, DueDate = DateTime.UtcNow.AddDays(2) }
        //    };

        //    mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedTasks);

        //    var result = await service.GetTasksItemsAsync(null, null);
        //    Assert.Equal(expectedTasks, result);
        //}

        [Fact]
        public async Task GetTasksItemsAsync_WithStatus_ReturnsFilteredTasks()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var status = TaskStatusEnum.Pending;
            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", Description = "Description 1", Status = status, DueDate = DateTime.UtcNow.AddDays(1) }
            };

            mockRepository.Setup(repo => repo.GetAllAsync(status, null)).ReturnsAsync(expectedTasks);

            var result = await service.GetTasksItemsAsync(status, null);

            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTasksItemsAsync_WithDueDate_ReturnsFilteredTasks()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var dueDate = DateTime.UtcNow.AddDays(1);
            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", Description = "Description 1", Status = TaskStatusEnum.Pending, DueDate = dueDate }
            };

            mockRepository.Setup(repo => repo.GetAllAsync(null, dueDate)).ReturnsAsync(expectedTasks);

            var result = await service.GetTasksItemsAsync(null, dueDate);

            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTasksItemsAsync_StatusAndDueDate_ReturnsFilteredTasks()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var status = TaskStatusEnum.Pending;
            var dueDate = DateTime.UtcNow.AddDays(1);
            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", Description = "Description 1", Status = status, DueDate = dueDate }
            };

            mockRepository.Setup(repo => repo.GetAllAsync(status, dueDate)).ReturnsAsync(expectedTasks);

            var result = await service.GetTasksItemsAsync(status, dueDate);

            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task CreateTaskAsync_ValidatesTaskDtoWithFluentValidation()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.TaskRepository).Returns(mockRepository.Object);

            var service = new TaskService(mockUnitOfWork.Object);
            var taskDto = new CreateTaskDto
            {
                Title = "Valid Task",
                Description = "This is a valid task description.",
                Status = TaskStatusEnum.Pending,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            await service.CreateTaskAsync(taskDto);

            mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<TaskItem>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
