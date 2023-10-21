namespace PersistEntities.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningSpacePe : ILearningSpacePe
{
    IAdvancedLearningSpaceLayoutPe AdvancedLearningSpaceLayout { get; set; }
}