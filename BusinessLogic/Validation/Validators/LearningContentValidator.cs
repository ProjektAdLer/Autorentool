using BusinessLogic.Entities;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningContentValidator : AbstractValidator<LearningContent>
{
    public LearningContentValidator()
    {
        RuleFor(x => x.Filepath)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Type)
            .NotEmpty();
    }
}