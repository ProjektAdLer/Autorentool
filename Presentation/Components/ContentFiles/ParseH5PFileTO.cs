using Microsoft.AspNetCore.Components;
using Presentation.PresentationLogic.LearningContent.FileContent;

namespace Presentation.Components.ContentFiles;

public struct ParseH5PFileTO
{
    public string FileEnding { get; set; }
    public string FileName { get; set; }
    public IFileContentViewModel FileContentVm { get; set; }
    public NavigationManager NavigationManager { get; set; }
}