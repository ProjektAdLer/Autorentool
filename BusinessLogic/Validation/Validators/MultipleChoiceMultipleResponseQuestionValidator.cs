using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class MultipleChoiceMultipleResponseQuestionValidator : AbstractValidator<IMultipleChoiceQuestion>
{
    public MultipleChoiceMultipleResponseQuestionValidator()
    {
        Include(new MultipleChoiceQuestionValidator());
    }
}