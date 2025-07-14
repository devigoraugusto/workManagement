using Application.DTOs;
using FluentValidation;
using FluentValidation.Internal;

namespace Application.Validator
{
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 100).WithMessage("Title cannot be longer than 100 characters.");

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
