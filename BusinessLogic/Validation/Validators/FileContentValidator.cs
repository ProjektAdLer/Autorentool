using BusinessLogic.Entities.LearningContent;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class FileContentValidator : AbstractValidator<FileContent>
{
    public FileContentValidator(IValidator<LearningContent> baseValidator)
    {
        Include(baseValidator);
        RuleFor(x => x.Filepath)
            .NotEmpty();
        RuleFor(x => x.Type)
            .NotEmpty();
    }
}