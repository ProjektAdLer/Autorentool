using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class ChoiceValidator : AbstractValidator<Choice>
{
    public ChoiceValidator(IStringLocalizer<ChoiceValidator> localizer)
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage(localizer["ChoiceValidator.ChoiceTextEmpty.ErrorMessage"])
            .MaximumLength(1000)
            .WithMessage(localizer["ChoiceValidator.ChoiceTextTooLong.ErrorMessage"]);
    }
}