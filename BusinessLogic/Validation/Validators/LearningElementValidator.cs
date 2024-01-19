using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Story;
using BusinessLogic.Validation.Validators.CustomValidators;
using FluentValidation;
using JetBrains.Annotations;
using ValidationException = FluentValidation.ValidationException;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningElementValidator : AbstractValidator<LearningElement>
{
    private readonly ILearningElementNamesProvider _learningElementNamesProvider;

    public LearningElementValidator(ILearningElementNamesProvider learningElementNamesProvider, IValidator<ILearningContent> learningContentValidator, IValidator<StoryContent> storyContentValidator)
    {
        _learningElementNamesProvider = learningElementNamesProvider;
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 60)
            .IsValidName()
            .Must((element, name) => IsUniqueName(element.Id, name))
            .WithMessage("Already in use.");
        RuleFor(x => x.LearningContent)
            //.SetValidator(learningContentValidator)
            .SetInheritanceValidator(v =>
            {
                v.Add(storyContentValidator);
            })
            .NotEmpty();
    }

    private bool IsUniqueName(Guid id, string name) => UniqueNameHelper.IsUnique(
        _learningElementNamesProvider.ElementNames ?? throw new ValidationException("No space to get elements from"),
        name, id);
}