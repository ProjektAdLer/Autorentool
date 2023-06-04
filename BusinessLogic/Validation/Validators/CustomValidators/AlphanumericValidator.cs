using FluentValidation;

namespace BusinessLogic.Validation.Validators.CustomValidators;

public static class AlphanumericValidator
{
    public static IRuleBuilderOptions<T, string> IsAlphanumeric<T>(this IRuleBuilder<T, string> ruleBuilder, IEnumerable<char>? additionalAllowedChars = null, string? message = null)
    {
        return ruleBuilder
            .Must(s => s.All(c => char.IsLetterOrDigit(c) || additionalAllowedChars?.Contains(c) == true))
            .WithMessage(message == null ? "Must only contain letters and digits." : $"{message}");
    }
}