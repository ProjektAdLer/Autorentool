using FluentValidation;
using JetBrains.Annotations;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.AuthoringToolWorkspace;

namespace Presentation.Components.Forms.Validators;

[UsedImplicitly]
public class WorldValidator : AbstractValidator<WorldFormModel>
{
    private readonly IWorldNamesProvider _worldNamesProvider;

    public WorldValidator(IWorldNamesProvider worldNamesProvider)
    {
        _worldNamesProvider = worldNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(4, 100)
            .Must(IsUniqueName)
            .WithMessage("Already in use.");
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .Must(IsUniqueShortName)
            .WithMessage("Already in use.");
    }

    private bool IsUniqueName(string value) => !_worldNamesProvider.WorldNames.Contains(value);
    private bool IsUniqueShortName(string value)
    {
        if (value == "") return true;
        return !_worldNamesProvider.WorldShortNames.Contains(value);
    }
}