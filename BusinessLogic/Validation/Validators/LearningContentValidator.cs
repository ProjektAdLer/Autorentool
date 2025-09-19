using BusinessLogic.Entities.LearningContent;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningContentValidator : AbstractValidator<ILearningContent>
{
    public LearningContentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty");
    }
}