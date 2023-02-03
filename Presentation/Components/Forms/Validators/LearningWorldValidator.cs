using FluentValidation;
using JetBrains.Annotations;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.AuthoringToolWorkspace;

namespace Presentation.Components.Forms.Validators;

[UsedImplicitly]
public class LearningWorldValidator : AbstractValidator<LearningWorldFormModel>
{
    private readonly ILearningWorldNamesProvider _learningWorldNamesProvider;

    public LearningWorldValidator(ILearningWorldNamesProvider learningWorldNamesProvider)
    {
        _learningWorldNamesProvider = learningWorldNamesProvider;
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

    private bool IsUniqueName(string value) => !_learningWorldNamesProvider.WorldNames.Contains(value);
    private bool IsUniqueShortName(string value)
    {
        if (value == "") return true;
        return !_learningWorldNamesProvider.WorldShortNames.Contains(value);
    }
}