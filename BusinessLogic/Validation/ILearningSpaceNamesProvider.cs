namespace BusinessLogic.Validation;

public interface ILearningSpaceNamesProvider
{
    
    /// <summary>
    /// Returns the id and name of all spaces in the current world selected in the provider.
    /// </summary>
    public IEnumerable<(Guid, string)>? SpaceNames { get; }
}