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
        // Validation for single response is solved by the radio group in the UI.
    }
}