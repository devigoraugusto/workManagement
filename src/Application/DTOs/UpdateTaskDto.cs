using Domain.Enums;

namespace Application.DTOs
{
    public class UpdateTaskDto
    {
        public required int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}
