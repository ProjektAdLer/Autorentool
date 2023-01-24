using FluentValidation;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.Components.Forms.Validators;

public class LearningSpaceValidator : AbstractValidator<LearningSpaceFormModel>
{
    private ILearningSpaceNamesProvider LearningSpaceNamesProvider { get; }

    public LearningSpaceValidator(ILearningSpaceNamesProvider learningSpaceNamesProvider)
    {
        LearningSpaceNamesProvider = learningSpaceNamesProvider;
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
        return !LearningSpaceNamesProvider
            .LearningSpaceNames
            ?.Contains(arg) ?? throw new ValidationException("No learning world selected.");
    }
    
    private bool IsUniqueShortNameInWorld(string arg)
    {
        return !LearningSpaceNamesProvider
            .LearningSpaceShortnames
            ?.Contains(arg) ?? throw new ValidationException("No learning world selected.");
    }
}