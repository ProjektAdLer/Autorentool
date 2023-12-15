namespace BusinessLogic.Entities.LearningContent;

public interface ILearningContent : IEquatable<ILearningContent>
{
    string Name { get; set; }
    bool UnsavedChanges { get; set; }
}