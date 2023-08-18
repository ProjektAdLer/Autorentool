using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor;

public interface IAdvancedPositioningService 
{
    void CreateAdvancedComponent();
    void DeleteAdvancedComponent(IAdvancedComponentViewModel targetObject);
    
}