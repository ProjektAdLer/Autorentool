namespace Presentation.Components.Forms;

public interface IValidationWrapper<in TEntity>
{
    Task<IEnumerable<string>> ValidateAsync(TEntity entity, string propertyName);
}