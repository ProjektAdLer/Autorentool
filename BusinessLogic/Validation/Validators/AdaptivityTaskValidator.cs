using BusinessLogic.Entities.LearningContent.Adaptivity;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class AdaptivityTaskValidator : AbstractValidator<AdaptivityTask>
{
    public AdaptivityTaskValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Task name is required.")
            .MaximumLength(100)
            .WithMessage("Task name cannot be longer than 100 characters.");
    }
}