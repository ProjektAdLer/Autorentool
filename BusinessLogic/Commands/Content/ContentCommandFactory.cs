using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Content;

public class ContentCommandFactory : IContentCommandFactory
{
    public ContentCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }
    
    public IDeleteLearningContent GetDeleteCommand(AuthoringToolWorkspace workspace, ILearningContent content, Action<AuthoringToolWorkspace> mappingAction)
        => new DeleteLearningContent(workspace, content, mappingAction, LoggerFactory.CreateLogger<DeleteLearningContent>());
}