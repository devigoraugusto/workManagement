using Application.Validator;
using Domain.Enums;

namespace Application.DTOs
{
    public class CreateTaskDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required TaskStatusEnum Status { get; set; }
        public required DateTime DueDate { get; set; }
    }
}
