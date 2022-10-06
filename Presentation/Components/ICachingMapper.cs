using BusinessLogic.Entities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.Components;

public interface ICachingMapper
{
    void Map(AuthoringToolWorkspace authoringToolWorkspaceEntity, IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm);
    void Map(LearningWorld learningWorldEntity, ILearningWorldViewModel learningWorldVm);
    void Map(LearningSpace learningSpaceEntity, ILearningSpaceViewModel learningSpaceVm);
    void Map(LearningElement learningElementEntity, ILearningElementViewModel learningElementVm);
}