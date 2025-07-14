using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTaskDto> _createValidator;
        private readonly IValidator<UpdateTaskDto> _updateValidator;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService, IMapper mapper, IValidator<CreateTaskDto> createValidador, IValidator<UpdateTaskDto> updateValidator)
        {
            _logger = logger;
            _taskService = taskService;
            _mapper = mapper;
            _createValidator = createValidador;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] PaginationParams paginationParams, [FromQuery] TaskStatusEnum? status, [FromQuery] DateTime? dueDate)
        {
            var result = await _taskService.GetTasksItemsPaginatedAsync(paginationParams, status, dueDate);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            _logger.LogInformation("Fetching task with ID {Id}", id);
            var entity = await _taskService.GetTaskItemByIdAsync(id);

            if (entity == null)
            {
                _logger.LogWarning("Task with ID {Id} not found", id);
                return NotFound();
            }
            return Ok(_mapper.Map<TaskItemDto>(entity));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto task)
        {
            _logger.LogInformation("Creating a new task with title: {Title}", task.Title);
            var result = await _createValidator.ValidateAsync(task);
            
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for task creation: {Errors}", result.Errors);
                return BadRequest(result.Errors);
            }

            var entity = _mapper.Map<TaskItem>(task);

            await _taskService.CreateTaskAsync(task);

            var response = _mapper.Map<TaskItemDto>(entity);

            return CreatedAtAction(nameof(GetTaskById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto task)
        {
            _logger.LogInformation("Updating task with ID {Id}", id);
            if (id != task.Id)
            {
                _logger.LogWarning("Task ID mismatch: URL ID {UrlId} does not match body ID {BodyId}", id, task.Id);
                return BadRequest("Task ID mismatch");
            }

            var result = await _updateValidator.ValidateAsync(task);

            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for task update: {Errors}", result.Errors);
                return BadRequest(result.Errors);
            }

            var entity = _mapper.Map<TaskItem>(task);

            await _taskService.UpdateTaskAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            _logger.LogInformation("Deleting task with ID {Id}", id);
            var task = await _taskService.GetTaskItemByIdAsync(id);

            if (task == null)
            {
                _logger.LogWarning("Task with ID {Id} not found for deletion", id);
                return NotFound();
            }

            await _taskService.DeleteTaskAsync(task.Id);

            return NoContent();
        }
    }
}
