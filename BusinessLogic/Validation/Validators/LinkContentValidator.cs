using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LinkContentValidator : AbstractValidator<LinkContent>
{
    public LinkContentValidator(IValidator<ILearningContent> baseValidator, IStringLocalizer<LinkContentValidator> localizer)
    {
        Include(baseValidator);
        RuleFor(x => x.Link)
            .NotEmpty();
        RuleFor(x => x.Link)
            .IsHttpOrHttpsUrl(localizer["LinkContentValidator.Url.Invalid"])
            .When(x => !string.IsNullOrEmpty(x.Link));
    }
}