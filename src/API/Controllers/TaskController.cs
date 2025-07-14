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
        public async Task<IActionResult> GetTasks([FromQuery] TaskStatusEnum? status, [FromQuery] DateTime? dueDate)
        {
            var list = await _taskService.GetTasksItemsAsync(status, dueDate);
            
            return Ok(_mapper.Map<IEnumerable<TaskItemDto>>(list));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var entity = await _taskService.GetTaskItemByIdAsync(id);
            
            if (entity == null) return NotFound();

            return Ok(_mapper.Map<TaskItemDto>(entity));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto task)
        {
            var result = await _createValidator.ValidateAsync(task);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            var entity = _mapper.Map<TaskItem>(task);

            await _taskService.CreateTaskAsync(task);

            var response = _mapper.Map<TaskItemDto>(entity);

            return CreatedAtAction(nameof(GetTaskById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto task)
        {
            if (id != task.Id) return BadRequest("Task ID mismatch");

            var result = await _updateValidator.ValidateAsync(task);
            
            if (!result.IsValid) return BadRequest(result.Errors);

            var entity = _mapper.Map<TaskItem>(task);

            await _taskService.UpdateTaskAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskService.GetTaskItemByIdAsync(id);

            if (task == null) return NotFound();

            await _taskService.DeleteTaskAsync(task.Id);

            return NoContent();
        }
    }
}
