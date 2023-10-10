using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.LearningSpace;

internal class ReplaceLearningElementData
{
    public ReplaceLearningElementData(ILearningWorldViewModel learningWorldVm, ILearningElementViewModel dropItem,
        int slotId)
    {
        LearningWorldVm = learningWorldVm;
        DropItem = dropItem;
        SlotId = slotId;
    }

    internal ILearningWorldViewModel LearningWorldVm { get; init; }
    public ILearningElementViewModel DropItem { get; init; }
    public int SlotId { get; init; }
}