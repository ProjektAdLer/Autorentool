using BusinessLogic.Entities.LearningContent.Story;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class StoryContentValidator : AbstractValidator<StoryContent>
{
    private readonly IStringLocalizer<StoryContentValidator> _localizer;

    public StoryContentValidator(IStringLocalizer<StoryContentValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(x => x.StoryText)
            .NotEmpty().WithMessage(_localizer["StoryContentValidator.StoryTextNoBlocks.ErrorMessage"])
            .ForEach(IsValidStoryBlock);
        RuleFor(x => x.NpcName)
            .MaximumLength(60).WithMessage(_localizer["StoryContentValidator.NpcNameTooLong.ErrorMessage"]);
    }

    private void IsValidStoryBlock(IRuleBuilderInitialCollection<IEnumerable<string>, string> builder) =>
        builder
            .NotEmpty().WithMessage(_localizer["StoryContentValidator.BlockEmpty.ErrorMessage"])
            .MaximumLength(400).WithMessage(_localizer["StoryContentValidator.BlockTooLong.ErrorMessage"]);
}