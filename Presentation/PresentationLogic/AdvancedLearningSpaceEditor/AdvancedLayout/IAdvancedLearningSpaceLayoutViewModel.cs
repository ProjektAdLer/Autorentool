using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.LearningElement;
using Shared;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLayout;

public interface IAdvancedLearningSpaceLayoutViewModel
{
    IDictionary<int, ILearningElementViewModel> LearningElements { get; set; }
    IDictionary<int, IAdvancedLearningElementSlotViewModel>  AdvancedLearningElementSlots { get; set; }
    IDictionary<int, IAdvancedDecorationViewModel> AdvancedDecorations { get; set; }
    IDictionary<int, DoublePoint> AdvancedCornerPoints { get; set; }
    DoublePoint EntryDoorPosition { get; set; }
    DoublePoint ExitDoorPosition { get; set; }
    int Count { get; }
    IEnumerable<int> UsedIndices { get; }
    IEnumerable<ILearningElementViewModel> ContainedLearningElements { get; }
    IEnumerable<IAdvancedLearningElementSlotViewModel> ContainedAdvancedLearningElementSlots { get; }
    IEnumerable<IAdvancedDecorationViewModel> ContainedAdvancedDecorations { get; }
    IEnumerable<DoublePoint> ContainedAdvancedCornerPoints { get; }
    ILearningElementViewModel? GetElement(int index);
    void PutElement(int index, ILearningElementViewModel element);
    void RemoveElement(int index);
    void ClearAllElements();
    void AddAdvancedLearningElementSlot(Guid spaceId, int slotKey, double positionX, double positionY);
    void AddAdvancedDecoration(Guid spaceId, int decorationKey, double positionX, double positionY);
}