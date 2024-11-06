using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators.CustomValidators;

public static class NameValidator
{
    private static IEnumerable<char> AllowedWorldSpecialCharacters => new[] {'-', '_'};

    private static IEnumerable<char> AllowedSpaceSpecialCharacters => AllowedElementSpecialCharacters;
    private static IEnumerable<char> AllowedElementSpecialCharacters =>
        AllowedWorldSpecialCharacters.Union(new[] { ' ' });
    
    public static IRuleBuilderOptions<T, string> IsValidWorldName<T>(this IRuleBuilder<T, string> ruleBuilder, string message)
    {
        return ruleBuilder
            .Must(s => s.All(c => char.IsLetterOrDigit(c) || AllowedWorldSpecialCharacters.Contains(c)))
            .WithMessage(message);
    }
    
    public static IRuleBuilderOptions<T, string> IsValidSpaceName<T>(this IRuleBuilder<T, string> ruleBuilder, string message)
    {
        return ruleBuilder
            .Must(s => s.All(c => char.IsLetterOrDigit(c) || AllowedSpaceSpecialCharacters.Contains(c)))
            .WithMessage(message);
    }
    
    public static IRuleBuilderOptions<T, string> IsValidElementName<T>(this IRuleBuilder<T, string> ruleBuilder, string message)
    {
        return ruleBuilder
            .Must(s => s.All(c => char.IsLetterOrDigit(c) || AllowedElementSpecialCharacters.Contains(c)))
            .WithMessage(message);
    }
    
}