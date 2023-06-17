using FluentValidation;

namespace BusinessLogic.Validation.Validators.CustomValidators;

public static class NameValidator
{
    private static IEnumerable<char> AllowedSpecialCharacters => new[] {'-', '_', ' '};
    
    public static IRuleBuilderOptions<T, string> IsValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => s.All(c => char.IsLetterOrDigit(c) || AllowedSpecialCharacters.Contains(c)))
            .WithMessage("Must only contain letters, digits, spaces, dashes and underscores.");
    }
}