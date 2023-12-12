using System.Text.RegularExpressions;

namespace Shared.Extensions;

public static class StringHelper
{
    public static string IncrementName(string name)
    {
        var match = Regex.Match(name, @"^(.*)\((\d+)\)$");
        if (!match.Success) return $"{name}(1)";
        var baseName = match.Groups[1].Value;
        var number = int.Parse(match.Groups[2].Value);
        return $"{baseName}({number + 1})";

    }
    
    public static string GetUniqueName(IEnumerable<string?> takenNames, string name)
    {
        var enumerable = takenNames as string[] ?? takenNames.ToArray();
        while (enumerable.Any(takenName => takenName == name))
        {
            name = StringHelper.IncrementName(name);
        }

        return name;
    }
}