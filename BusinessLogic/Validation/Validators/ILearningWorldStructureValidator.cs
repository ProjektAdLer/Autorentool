using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Validation.Validators;

public interface ILearningWorldStructureValidator
{
    public ValidationResult ValidateForExport(ILearningWorld world);
    public ValidationResult ValidateForGeneration(ILearningWorld world);
}