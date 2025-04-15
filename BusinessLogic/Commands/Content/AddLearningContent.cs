using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Content;

public class AddLearningContent : IAddLearningContent
{
    
    public AddLearningContent(AuthoringToolWorkspace authoringToolWorkspace, ILearningContent content, Action<AuthoringToolWorkspace> mappingAction,
        ILogger<AddLearningContent> logger)
    {
        Workspace = authoringToolWorkspace;
        Content = content;
        MappingAction = mappingAction;
        Logger = logger;
    }
    public string Name { get; } = nameof(AddLearningContent);
    
    internal ILearningContent Content { get; }
    internal AuthoringToolWorkspace Workspace { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    private ILogger<AddLearningContent> Logger { get; }
    
    
    public void Execute()
    {
        var existingContent = Workspace.LearningContents.FirstOrDefault(x => x.Equals(Content));
        
        if(existingContent is FileContent fileContent)
        {
            fileContent.IsDeleted = false;
            Logger.LogTrace("Set LearningContent.IsDeleted {ContentName} to false", Content.Name);
        }
        else
        {
            Workspace.LearningContents.Add(Content);
            Logger.LogTrace("Added LearningContent {ContentName} to Workspace.LearningContents", Content.Name);
        }
        
        MappingAction.Invoke(Workspace);
    }

    public void Undo()
    {
        if (Workspace.LearningContents.FirstOrDefault(x => x.Equals(Content)) is FileContent fileContent)
        {
            fileContent.IsDeleted = true;
        }
        MappingAction.Invoke(Workspace);
    }

    public void Redo()
    {
        if (Workspace.LearningContents.FirstOrDefault(x => x.Equals(Content)) is FileContent fileContent)
        {
            fileContent.IsDeleted = false;
        }
        MappingAction.Invoke(Workspace);
    }
}