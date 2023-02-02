using FluentValidation;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.World;

namespace Presentation.Components.Forms.Validators;

public class SpaceValidator : AbstractValidator<SpaceFormModel>
{
    private ISpaceNamesProvider SpaceNamesProvider { get; }

    public SpaceValidator(ISpaceNamesProvider spaceNamesProvider)
    {
        SpaceNamesProvider = spaceNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(4, 100)
            .Must(IsUniqueNameInWorld)
            .WithMessage("Already in use.");
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .Must(IsUniqueShortNameInWorld)
            .WithMessage("Already in use.");
        RuleFor(x => x.RequiredPoints)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
    }

    private bool IsUniqueNameInWorld(string arg)
    {
        return !SpaceNamesProvider
            .SpaceNames
            ?.Contains(arg) ?? throw new ValidationException("No world selected.");
    }
    
    private bool IsUniqueShortNameInWorld(string arg)
    {
        return !SpaceNamesProvider
            .SpaceShortnames
            ?.Contains(arg) ?? throw new ValidationException("No world selected.");
    }
}