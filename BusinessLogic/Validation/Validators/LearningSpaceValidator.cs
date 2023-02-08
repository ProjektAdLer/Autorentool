using BusinessLogic.Entities;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;
using ValidationException = FluentValidation.ValidationException;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningSpaceValidator : AbstractValidator<LearningSpace>
{
    private readonly ILearningSpaceNamesProvider _learningSpaceNamesProvider;

    public LearningSpaceValidator(ILearningSpaceNamesProvider learningSpaceNamesProvider)
    {
        _learningSpaceNamesProvider = learningSpaceNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(4, 100)
            .IsAlphanumeric()
            .Must((space, name) => IsUniqueNameInWorld(space.Id, name))
            .WithMessage("Already in use.");
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .Must((space, shortName) => IsUniqueShortnameInWorld(space.Id, shortName))
            .WithMessage("Already in use.");
        RuleFor(x => x.RequiredPoints)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
    }

    private bool IsUniqueNameInWorld(Guid id, string name) =>
        UniqueNameHelper.IsUnique(
            _learningSpaceNamesProvider.SpaceNames ?? throw new ValidationException("No world to get spaces from"),
            name, id);

    private bool IsUniqueShortnameInWorld(Guid id, string name) => name == "" || UniqueNameHelper.IsUnique(
        _learningSpaceNamesProvider.SpaceShortnames ?? throw new ValidationException("No world to get spaces from"),
        name, id);
}