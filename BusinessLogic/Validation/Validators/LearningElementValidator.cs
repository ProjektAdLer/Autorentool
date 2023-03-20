using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;
using ValidationException = FluentValidation.ValidationException;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningElementValidator : AbstractValidator<LearningElement>
{
    private readonly ILearningElementNamesProvider _learningElementNamesProvider;

    public LearningElementValidator(ILearningElementNamesProvider learningElementNamesProvider, IValidator<LearningContent> learningContentValidator)
    {
        _learningElementNamesProvider = learningElementNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(4, 100)
            .IsAlphanumeric()
            .Must((element, name) => IsUniqueName(element.Id, name))
            .WithMessage("Already in use.");
        RuleFor(x => x.Shortname)
            .MaximumLength(30)
            .Must((element, name) => IsUniqueShortname(element.Id, name))
            .WithMessage("Already in use.");
        RuleFor(x => x.LearningContent)
            .SetValidator(learningContentValidator)
            .NotEmpty();
    }

    private bool IsUniqueName(Guid id, string name) => UniqueNameHelper.IsUnique(
        _learningElementNamesProvider.ElementNames ?? throw new ValidationException("No space to get elements from"),
        name, id);

    private bool IsUniqueShortname(Guid id, string name) => name == "" || UniqueNameHelper.IsUnique(
        _learningElementNamesProvider.ElementNames ?? throw new ValidationException("No space to get elements from"),
        name, id);
}