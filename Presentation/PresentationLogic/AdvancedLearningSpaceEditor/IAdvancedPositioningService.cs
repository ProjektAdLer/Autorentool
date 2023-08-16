using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor;

public interface IAdvancedPositioningService 
{
    void CreateAdvancedComponent(IAdvancedComponentViewModel sourceObject, double x, double y);
    void DeleteAdvancedComponent(IAdvancedComponentViewModel targetObject);
    
}