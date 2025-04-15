using Microsoft.Extensions.Localization;

namespace Shared.Theme;

public static class ThemeHelper
{
    private const string ThemeTypeString = nameof(Theme);
    private static IStringLocalizer<Theme> _localizer = null!;

    public static void Initialize(IStringLocalizer<Theme> localizer)
    {
        _localizer = localizer;
    }

    public static string Localize(Theme theme)
    {
        if (_localizer == null)
        {
            throw new InvalidOperationException("ThemeHelper has not been initialized.");
        }

        var valueString = theme.ToString();

        return _localizer[$"Enum.{ThemeTypeString}.{valueString}"];
    }
}