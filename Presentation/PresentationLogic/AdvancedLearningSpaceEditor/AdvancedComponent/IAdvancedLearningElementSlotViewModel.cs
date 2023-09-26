namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

public interface IAdvancedLearningElementSlotViewModel : IAdvancedComponentViewModel
{
    string Identifier { get;}
    Guid SpaceId { get; }
    int SlotKey { get; }
}