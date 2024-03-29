using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;

namespace Presentation.PresentationLogic.LearningContent;

public interface ILearningContentViewModel
{
    string Name { get; init; }
    bool UnsavedChanges { get; set; }

    public static IEnumerable<string> GetSearchableStrings(ILearningContentViewModel arg) => arg switch
    {
        FileContentViewModel fileContentViewModel => new[] { fileContentViewModel.Name, fileContentViewModel.Type },
        LinkContentViewModel linkContentViewModel => new[] { linkContentViewModel.Name, "Link" },
        _ => throw new ArgumentOutOfRangeException(nameof(arg))
    };
}