using Microsoft.Extensions.Localization;

namespace Shared.Theme;

public static class ThemeHelper<TEnum> where TEnum : struct, Enum
{
    private static IStringLocalizer<TEnum> _localizer = null!;

    public static void Initialize(IStringLocalizer<TEnum> localizer)
    {
        _localizer = localizer;
    }

    public static string Localize(TEnum enumValue)
    {
        if (_localizer == null)
        {
            throw new InvalidOperationException("ThemeHelper has not been initialized.");
        }

        string enumName = typeof(TEnum).Name;
        var valueString = enumValue.ToString();

        return _localizer[$"Enum.{enumName}.{valueString}"];
    }
}