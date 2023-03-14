using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LinkContentValidator : AbstractValidator<LinkContent>
{
    public LinkContentValidator(IValidator<LearningContent> baseValidator)
    {
        Include(baseValidator);
        RuleFor(x => x.Link)
            .NotEmpty();
        RuleFor(x => x.Link)
            .IsHttpOrHttpsUrl()
            .When(x => !string.IsNullOrEmpty(x.Link));
    }
}