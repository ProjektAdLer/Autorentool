using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Story;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class LearningContentValidator : AbstractValidator<ILearningContent>
{
    public LearningContentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

[UsedImplicitly]
public class StoryContentValidator : AbstractValidator<StoryContent>
{
    public StoryContentValidator()
    {
        RuleFor(x => x.StoryText)
            .NotEmpty();
    }
}