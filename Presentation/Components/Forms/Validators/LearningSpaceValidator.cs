using FluentValidation;
using Presentation.Components.Forms.Models;

namespace Presentation.Components.Forms.Validators;

public class LearningSpaceValidator : AbstractValidator<LearningSpaceFormModel>
{
    public LearningSpaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(4, 100)
            .Must(IsUniqueName)
            .WithMessage("Already in use.");
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .Must(IsUniqueShortName)
            .WithMessage("Already in use.");
        RuleFor(x => x.RequiredPoints)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
    }

    private bool IsUniqueName(string arg)
    {
        throw new NotImplementedException();
    }
    
    private bool IsUniqueShortName(string arg)
    {
        throw new NotImplementedException();
    }
}