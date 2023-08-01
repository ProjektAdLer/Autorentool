using Presentation.PresentationLogic.AdvancedComponent;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceGenerator;

public interface IAdvancedPositioningService
{
    void CreateAdvancedObject(IAdvancedComponentViewModel sourceObject, double x, double y);
    void DeleteAdvancedObject(IAdvancedComponentViewModel targetObject);
    void SetOnHoveredAdvancedObject(IAdvancedComponentViewModel sourceObject, double x, double y);

}