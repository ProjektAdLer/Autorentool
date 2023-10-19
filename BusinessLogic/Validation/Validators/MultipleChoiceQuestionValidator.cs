using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class MultipleChoiceQuestionValidator : AbstractValidator<IMultipleChoiceQuestion>
{
    public MultipleChoiceQuestionValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Question text is required.")
            .MaximumLength(1000)
            .WithMessage("Question text cannot be longer than 1000 characters.");
        RuleFor(x => x.Choices)
            .NotEmpty()
            .WithMessage("Question must have at least two choice.")
            .Must(x => x.Count >= 2)
            .WithMessage("Question must have at least two choices.")
            .Must(x => x.Count <= 10)
            .WithMessage("Question cannot have more than ten choices.");
    }
}