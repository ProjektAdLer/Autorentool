using Microsoft.AspNetCore.Components;
using Presentation.PresentationLogic.LearningContent.FileContent;

namespace Presentation.Components.ContentFiles;

public record StartH5PPlayerTO
{
    public IFileContentViewModel? FileContentVm { get; set; }
    public NavigationManager? NavigationManager { get; set; }
}