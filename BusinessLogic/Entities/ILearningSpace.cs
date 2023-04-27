namespace BusinessLogic.Entities;

public interface ILearningSpace
{
    Guid Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int RequiredPoints { get; set; }
    bool UnsavedChanges { get; set; }
    ILearningSpaceLayout LearningSpaceLayout { get; set; }
    IEnumerable<ILearningElement> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;
}