using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class MultipleChoiceQuestionValidator : AbstractValidator<IMultipleChoiceQuestion>
{
    public MultipleChoiceQuestionValidator(IStringLocalizer<MultipleChoiceQuestionValidator> localizer)
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage(localizer["MultipleChoiceQuestionValidator.TextEmpty.ErrorMessage"])
            .MaximumLength(1000)
            .WithMessage(localizer["MultipleChoiceQuestionValidator.TextLength.ErrorMessage"]);
        RuleFor(x => x.Choices)
            .NotEmpty()
            .WithMessage(localizer["MultipleChoiceQuestionValidator.Choices.ErrorMessage"])
            .Must(x => x.Count >= 2)
            .WithMessage(localizer["MultipleChoiceQuestionValidator.Choices.ErrorMessage"])
            .Must(x => x.Count <= 10)
            .WithMessage(localizer["MultipleChoiceQuestionValidator.ChoiceLimit.ErrorMessage"]);
        RuleFor(x => x.CorrectChoices)
            .NotEmpty()
            .WithMessage(localizer["MultipleChoiceQuestionValidator.OneChoice.ErrorMessage"]);
    }
}