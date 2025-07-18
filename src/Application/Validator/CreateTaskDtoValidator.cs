using FluentValidation;
using Application.DTOs;

namespace Application.Validator
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 10).WithMessage("Title cannot be longer than 10 characters.");
            
            RuleFor(task => task.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(1, 500).WithMessage("Description cannot be longer than 500 characters.");
            
            RuleFor(task => task.Status)
                .IsInEnum().WithMessage("Status must be a valid TaskStatusEnum value.");
            
            RuleFor(task => task.DueDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Due date must be in the future or today.");

        }

    }
}
