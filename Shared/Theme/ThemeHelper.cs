using Microsoft.Extensions.Localization;

namespace Shared.Theme;

public static class ThemeHelper<TEnum> where TEnum : struct, Enum
{
    private static IStringLocalizer<TEnum> _localizer = null!;

    public static void Initialize(IStringLocalizer<TEnum> localizer)
    {
        _localizer = localizer;
    }

    public static string Localize(TEnum enumValue, string? context = null)
    {
        if (_localizer == null)
        {
            throw new InvalidOperationException("ThemeHelper has not been initialized.");
        }

        var baseKey = $"Enum.{typeof(TEnum).Name}.{enumValue}";

        if (string.IsNullOrWhiteSpace(context)) return _localizer[baseKey];
        var res = _localizer[$"{baseKey}.{context}"];
        return !res.ResourceNotFound ? res.Value : _localizer[baseKey];
    }
}