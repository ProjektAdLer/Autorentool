using BusinessLogic.Entities;
using FluentValidation;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace BusinessLogic.Validation.Validators;

public class LearningSpaceValidator : AbstractValidator<LearningSpace>
{
    private readonly ILearningSpaceNamesProvider _learningSpaceNamesProvider;

    public LearningSpaceValidator(ILearningSpaceNamesProvider learningSpaceNamesProvider)
    {
        _learningSpaceNamesProvider = learningSpaceNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(4, 100)
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

    private bool IsUniqueNameInWorld(Guid id, string name)
    {
        var allSpacesExceptSelf = _learningSpaceNamesProvider
            .SpaceNames
            ?.Where(tup => tup.Item1 != id)
            .Select(tup => tup.Item2);
        return !allSpacesExceptSelf?.Contains(name) ?? throw new ValidationException("No learning world selected.");
    }
    
    private bool IsUniqueShortnameInWorld(Guid id, string name)
    {
        if (name == "") return true;
        var allSpacesExceptSelf = _learningSpaceNamesProvider
            .SpaceShortnames
            ?.Where(tup => tup.Item1 != id)
            .Select(tup => tup.Item2);
        return !allSpacesExceptSelf?.Contains(name) ?? throw new ValidationException("No learning world selected.");
    }
}