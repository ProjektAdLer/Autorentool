using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class MultipleChoiceSingleResponseQuestionValidator : AbstractValidator<MultipleChoiceSingleResponseQuestion>
{
    public MultipleChoiceSingleResponseQuestionValidator()
    {
        Include(new MultipleChoiceQuestionValidator());
        RuleFor(x => x.CorrectChoices)
            .Must(x => x.Count == 1)
            .WithMessage("Question must have exactly one correct choice.");
    }
}