namespace Application.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatusEnum Status { get; set; }
        public DateTime DueDate { get; set; }
        // Additional properties can be added here if needed
        // For example, you might want to include CreatedAt, UpdatedAt, etc.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
