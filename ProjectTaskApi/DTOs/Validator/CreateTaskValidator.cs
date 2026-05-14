using FluentValidation;
using ProjectTaskApi.DTOs.Tasks;

namespace ProjectTaskApi.DTOs.Validator
{
    public class CreateTaskValidator : AbstractValidator<TaskCreateDto>
    {
        public CreateTaskValidator() 
        { 
            RuleFor(x=>x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ProjectId).NotEmpty();
        }
    }
}
