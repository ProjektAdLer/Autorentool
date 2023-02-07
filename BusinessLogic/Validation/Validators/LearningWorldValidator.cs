using BusinessLogic.Entities;
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
            .Length(4, 100)
            .Must((world, name) => IsUniqueName(world.Id, name))
            .WithMessage("Already in use.");
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .Must((world, name) => IsUniqueShortname(world.Id, name))
            .WithMessage("Already in use.");
    }

    private bool IsUniqueName(Guid id, string name)
    {
        var allWorldsExceptSelf = _learningWorldNamesProvider
            .WorldNames
            .Where(tup => tup.Item1 != id)
            .Select(tup => tup.Item2); 
        return !allWorldsExceptSelf.Contains(name);
    }

    private bool IsUniqueShortname(Guid id, string name)
    {
        if (name == "") return true;
        var allWorldsExceptSelf = _learningWorldNamesProvider
            .WorldShortnames
            .Where(tup => tup.Item1 != id)
            .Select(tup => tup.Item2); 
        return !allWorldsExceptSelf.Contains(name);
    }
}