using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLayout;

public interface IAdvancedLearningSpaceLayoutViewModel
{
    IDictionary<int, ILearningElementViewModel> LearningElements { get; set; }
    IDictionary<int, IAdvancedLearningElementSlotViewModel>  AdvancedLearningElementSlots { get; set; }
    IDictionary<int, IAdvancedDecorationViewModel> AdvancedDecorations { get; set; }
    int Capacity { get; }
    int Count { get; }
    IEnumerable<int> UsedIndices { get; }
    IEnumerable<ILearningElementViewModel> ContainedLearningElements { get; }
    IEnumerable<IAdvancedLearningElementSlotViewModel> ContainedAdvancedLearningElementSlots { get; }
    ILearningElementViewModel? GetElement(int index);
    void PutElement(int index, ILearningElementViewModel element);
    void RemoveElement(int index);
    void ClearAllElements();
    void AddAdvancedLearningElementSlot(Guid spaceId, int slotKey, double positionX, double positionY);
    void AddAdvancedDecoration(Guid spaceId, int decorationKey, double positionX, double positionY);
}