using BusinessLogic.Entities;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningWorldValidator : AbstractValidator<LearningWorld>
{
    private readonly ILearningWorldNamesProvider _learningWorldNamesProvider;

    public LearningWorldValidator(ILearningWorldNamesProvider learningWorldNamesProvider)
    {
        _learningWorldNamesProvider = learningWorldNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 60)
            .Must((world, name) => IsUniqueName(world.Id, name))
            .WithMessage("Already in use.");
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .IsValidName()
            .Must((world, name) => IsUniqueShortname(world.Id, name))
            .WithMessage("Already in use.");
    }

    private bool IsUniqueName(Guid id, string name) =>
        UniqueNameHelper.IsUnique(_learningWorldNamesProvider.WorldNames, name, id);

    private bool IsUniqueShortname(Guid id, string name) => name == "" || UniqueNameHelper.IsUnique(_learningWorldNamesProvider.WorldShortnames, name, id);
}