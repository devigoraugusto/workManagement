using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskStatusEnum? status, [FromQuery] DateTime? dueDate)
        {
            var tasks = await _taskService.GetTasksItemsAsync(status, dueDate);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskItemByIdAsync(id);

            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            await _taskService.CreateTaskAsync(task);

            return CreatedAtAction(nameof(GetTaskById), new { Id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem task)
        {
            if (id != task.Id) return BadRequest("Task ID mismatch");

            await _taskService.UpdateTaskAsync(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskService.GetTaskItemByIdAsync(id);

            if (task == null) return NotFound();

            await _taskService.DeleteTaskAsync(id);

            return NoContent();
        }
    }
}
