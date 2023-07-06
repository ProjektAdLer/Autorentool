using FluentValidation;

namespace Presentation.Components.Forms;

public class ValidationWrapper<TEntity> : IValidationWrapper<TEntity>
{
    public IValidator<TEntity> Validator { get; }

    public ValidationWrapper(IValidator<TEntity> validator)
    {
        Validator = validator;
    }

    public async Task<IEnumerable<string>> ValidateAsync(TEntity entity, string propertyName)
    {
        var result = await Validator.ValidateAsync(
            ValidationContext<TEntity>.CreateWithOptions(entity,
                x => x.IncludeProperties(propertyName))
        );
        return result.IsValid ? Enumerable.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    }
}