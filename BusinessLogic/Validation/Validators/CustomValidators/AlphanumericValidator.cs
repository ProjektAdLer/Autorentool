using FluentValidation;

namespace BusinessLogic.Validation.Validators.CustomValidators;

public static class AlphanumericValidator
{
    public static IRuleBuilderOptions<T, string> IsAlphanumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => s.All(char.IsLetterOrDigit))
            .WithMessage("Must only contain letters and digits.");
    }
}