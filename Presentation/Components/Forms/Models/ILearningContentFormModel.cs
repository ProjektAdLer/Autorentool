using Shared;

namespace Presentation.Components.Forms.Models;

public interface ILearningContentFormModel
{
    string Name { get; set; }

    public static IEnumerable<string> GetSearchableStrings(ILearningContentFormModel arg) => arg switch
    {
        FileContentFormModel fileContentFormModel => new[] { fileContentFormModel.Name, fileContentFormModel.Type },
        LinkContentFormModel linkContentFormModel => new[] { linkContentFormModel.Name, "Link" },
        _ => throw new ArgumentOutOfRangeException(nameof(arg))
    };
}