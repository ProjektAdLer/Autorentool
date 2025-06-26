using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Validation.Validators;

public interface ILearningWorldStructureValidator
{
    public ValidationResult ValidateForExport(ILearningWorld world, List<ILearningContent> listLearningContent);
    public ValidationResult ValidateForGeneration(ILearningWorld world, List<ILearningContent> listLearningContent);
}