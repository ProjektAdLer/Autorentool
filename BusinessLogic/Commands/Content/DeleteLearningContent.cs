using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Content;

public class DeleteLearningContent : IDeleteLearningContent
{
    public DeleteLearningContent(AuthoringToolWorkspace authoringToolWorkspace,ILearningContent content, Action<AuthoringToolWorkspace> mappingAction,
        ILogger<DeleteLearningContent> logger)
    {
        Workspace = authoringToolWorkspace;
        Content = Workspace.LearningContents.First(wsContent => wsContent.Equals(content));
        MappingAction = mappingAction;
        Logger = logger;
    }
    
    internal ILearningContent Content { get; }
    internal AuthoringToolWorkspace Workspace { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    private ILogger<DeleteLearningContent> Logger { get; }
    public string Name => nameof(DeleteLearningContent);
    public void Execute()
    {
        switch (Content)
        {
            case IFileContent fileContent:
                fileContent.IsDeleted = true;
                break;
            case ILinkContent linkContent:
                linkContent.IsDeleted = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Content) + " cannot be deleted");
        }
        
        Logger.LogTrace("Deleted LearningContent {ContentName}", Content.Name);
        
        MappingAction.Invoke(Workspace);
    }

    public void Undo()
    {
        switch (Content)
        {
            case IFileContent fileContent:
                fileContent.IsDeleted = false;
                break;
            case ILinkContent linkContent:
                linkContent.IsDeleted = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Content) + " cannot be undeleted");
        }
        
        Logger.LogTrace("Undone deletion of LearningContent {ContentName}", Content.Name);
        
        MappingAction.Invoke(Workspace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningContent");
        Execute();
    }
}