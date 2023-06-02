namespace BusinessLogic.Validation;

public interface ILearningWorldNamesProvider
{
    /// <summary>
    /// Returns the id and name of all worlds in the provider.
    /// </summary>
    public IEnumerable<(Guid, string)> WorldNames { get; }
    /// <summary>
    /// Returns the id and short name of all worlds in the provider.
    /// </summary>
    public IEnumerable<(Guid, string)> WorldShortnames { get; }
}