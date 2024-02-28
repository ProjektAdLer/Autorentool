namespace Presentation.Components.Forms;

public interface IBaseForm
{
    void Reset();
    /// <summary>
    /// Submits form (and changes object if validation passes).
    /// </summary>
    /// <returns>Whether or not validation passed successfully.</returns>
    Task<bool> SubmitAsync();
}