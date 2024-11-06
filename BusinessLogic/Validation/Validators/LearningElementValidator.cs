using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Story;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using ValidationException = FluentValidation.ValidationException;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningElementValidator : AbstractValidator<LearningElement>
{
    private readonly ILearningElementNamesProvider _learningElementNamesProvider;

    public LearningElementValidator(ILearningElementNamesProvider learningElementNamesProvider, IValidator<ILearningContent> learningContentValidator, IValidator<StoryContent> storyContentValidator, IStringLocalizer<LearningElementValidator> localizer)
    {
        _learningElementNamesProvider = learningElementNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 60)
            .IsValidElementName(localizer["LearningElementValidator.Name.Valid"])
            .Must((element, name) => IsUniqueName(element.Id, name))
            .WithMessage(localizer["LearningElementValidator.Name.Duplicate"]);
        RuleFor(x => x.LearningContent)
            //.SetValidator(learningContentValidator)
            .SetInheritanceValidator(v =>
            {
                v.Add(storyContentValidator);
            })
            .NotEmpty()
            .WithMessage(localizer["LearningElementValidator.NoContent.Error"]);
    }

    private bool IsUniqueName(Guid id, string name) => UniqueNameHelper.IsUnique(
        _learningElementNamesProvider.ElementNames ?? throw new ValidationException("No space to get elements from"),
        name, id);
}