using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using FluentValidation;
using JetBrains.Annotations;

namespace BusinessLogic.Validation.Validators;

[UsedImplicitly]
public class FileContentValidator : AbstractValidator<IFileContent>
{
    public FileContentValidator(IValidator<ILearningContent> baseValidator)
    {
        Include(baseValidator);
        RuleFor(x => x.Filepath)
            .NotEmpty();
        
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Filetype should not be empty");

        RuleFor(x => x.Type)
            .Must(type => AllowedFileEndings.Endings.Contains(type.ToLowerInvariant()))
            .When(x => !string.IsNullOrWhiteSpace(x.Type))
            .WithMessage("Filetype not valide");
    }
}