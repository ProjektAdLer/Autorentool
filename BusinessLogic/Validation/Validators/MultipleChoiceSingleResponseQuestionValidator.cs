using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class MultipleChoiceSingleResponseQuestionValidator : AbstractValidator<MultipleChoiceSingleResponseQuestion>
{
    public MultipleChoiceSingleResponseQuestionValidator(MultipleChoiceQuestionValidator baseValidator)
    {
        Include(baseValidator);
    }
}