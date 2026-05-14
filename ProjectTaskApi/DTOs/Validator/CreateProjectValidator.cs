using FluentValidation;

namespace ProjectTaskApi.DTOs.Validator
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectValidator() 
        {
            RuleFor(x=>x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
