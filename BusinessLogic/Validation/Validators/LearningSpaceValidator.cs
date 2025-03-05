using BusinessLogic.Entities;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using ValidationException = FluentValidation.ValidationException;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningSpaceValidator : AbstractValidator<LearningSpace>
{
    private readonly ILearningSpaceNamesProvider _learningSpaceNamesProvider;

    public LearningSpaceValidator(ILearningSpaceNamesProvider learningSpaceNamesProvider, IStringLocalizer<LearningSpaceValidator> localizer)
    {
        _learningSpaceNamesProvider = learningSpaceNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 60)
            .IsValidSpaceName(localizer["LearningSpaceValidator.Name.Valid"])
            .Must((space, name) => IsUniqueNameInWorld(space.Id, name))
            .WithMessage(localizer["LearningSpaceValidator.Name.Duplicate"]);
        RuleFor(x => x.RequiredPoints)
            .GreaterThanOrEqualTo(0);
    }

    private bool IsUniqueNameInWorld(Guid id, string name) =>
        UniqueNameHelper.IsUnique(
            _learningSpaceNamesProvider.SpaceNames ?? throw new ValidationException("No world to get spaces from"),
            name, id);
}