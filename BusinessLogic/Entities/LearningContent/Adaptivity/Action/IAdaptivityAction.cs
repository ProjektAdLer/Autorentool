namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public interface IAdaptivityAction : IEquatable<IAdaptivityAction>, IOriginator
{
    public Guid Id { get; }
    bool UnsavedChanges { get; set; }
}