using Microsoft.Extensions.Localization;

namespace Shared;

public static class LearningElementDifficultyHelper
{
    private const string DifficultyTypeString = nameof(LearningElementDifficultyEnum);
    private static IStringLocalizer<LearningElementDifficultyEnum> _localizer = null!;

    public static void Initialize(IStringLocalizer<LearningElementDifficultyEnum> localizer)
    {
        _localizer = localizer;
    }

    public static string Localize(LearningElementDifficultyEnum difficulty)
    {
        if (_localizer == null)
        {
            throw new InvalidOperationException("LearningElementDifficultyHelper has not been initialized.");
        }

        var valueString = difficulty.ToString();

        return _localizer[$"Enum.{DifficultyTypeString}.{valueString}"];
    }
}