namespace BusinessLogic.Validation;

public interface ILearningElementNamesProvider
{
    
    /// <summary>
    /// Returns the id and name of all elements in the current space selected in the provider.
    /// </summary>
    public IEnumerable<(Guid, string)>? ElementNames { get; }
    /// <summary>
    /// Returns the id and short name of all elements in the current space selected in the provider.
    /// </summary>
    public IEnumerable<(Guid, string)>? ElementShortnames { get; }
}