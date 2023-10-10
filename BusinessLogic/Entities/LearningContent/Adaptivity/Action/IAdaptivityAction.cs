namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public interface IAdaptivityAction : IEquatable<IAdaptivityAction>
{
    public Guid Id { get; }
}