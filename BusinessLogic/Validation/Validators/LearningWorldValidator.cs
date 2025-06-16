using BusinessLogic.Entities;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningWorldValidator : AbstractValidator<LearningWorld>
{
    private readonly ILearningWorldNamesProvider _learningWorldNamesProvider;

    public LearningWorldValidator(ILearningWorldNamesProvider learningWorldNamesProvider, IStringLocalizer<LearningWorldValidator> localizer)
    {
        _learningWorldNamesProvider = learningWorldNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 60)
            .Must((world, name) => IsUniqueName(world.Id, name))
            .WithMessage(localizer["LearningWorldValidator.Name.Duplicate"])
            .IsValidWorldName(localizer["LearningWorldValidator.Name.Valid"]);
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .IsValidElementName(localizer["LearningWorldValidator.Shortname.Valid"])
            .Must((world, name) => IsUniqueShortname(world.Id, name))
            .WithMessage(localizer["LearningWorldValidator.Shortname.Duplicate"]);
        RuleFor(x => x.StoryStart)
            .Length(0, 1200);
        RuleFor(x => x.StoryEnd)
            .Length(0, 1200);
    }

    private bool IsUniqueName(Guid id, string name) =>
        UniqueNameHelper.IsUnique(_learningWorldNamesProvider.WorldNames, name, id);

    private bool IsUniqueShortname(Guid id, string name) => name == "" || UniqueNameHelper.IsUnique(_learningWorldNamesProvider.WorldShortnames, name, id);
}