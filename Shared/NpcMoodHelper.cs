using Microsoft.Extensions.Localization;

namespace Shared;

public static class NpcMoodHelper
{
    private const string NpcMoodTypeString = nameof(NpcMood);
    private static IStringLocalizer<NpcMood> _localizer = null!;

    public static void Initialize(IStringLocalizer<NpcMood> localizer)
    {
        _localizer = localizer;
    }

    public static string Localize(NpcMood npcMood)
    {
        if (_localizer == null)
        {
            throw new InvalidOperationException("NpcMoodHelper has not been initialized.");
        }

        var valueString = npcMood.ToString();

        return _localizer[$"Enum.{NpcMoodTypeString}.{valueString}"];
    }
}