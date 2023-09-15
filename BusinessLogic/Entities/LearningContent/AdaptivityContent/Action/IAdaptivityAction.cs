namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

public interface IAdaptivityAction : IEquatable<IAdaptivityAction>
{
    public Guid Id { get; }
}