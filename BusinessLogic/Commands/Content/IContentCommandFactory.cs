using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;

namespace BusinessLogic.Commands.Content;

public interface IContentCommandFactory
{
    IDeleteLearningContent GetDeleteCommand(AuthoringToolWorkspace workspace, ILearningContent content,
        Action<AuthoringToolWorkspace> mappingAction);
    
    IAddLearningContent GetAddCommand(AuthoringToolWorkspace workspace, ILearningContent content,
        Action<AuthoringToolWorkspace> mappingAction);
}