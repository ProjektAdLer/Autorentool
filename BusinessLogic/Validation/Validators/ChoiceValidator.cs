using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using FluentValidation;

namespace BusinessLogic.Validation.Validators;

public class ChoiceValidator : AbstractValidator<Choice>
{
    public ChoiceValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Choice text is required.")
            .MaximumLength(1000)
            .WithMessage("Choice text cannot be longer than 1000 characters.");
    }
}