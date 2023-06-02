using MudBlazor;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.DropZone;

public interface ILearningElementDropZoneHelper
{
    IEnumerable<ILearningElementViewModel> GetWorldAndSpaceElements();
    void ItemUpdated(MudItemDropInfo<ILearningElementViewModel> dropItem);
    bool IsItemInDropZone(ILearningElementViewModel item, string dropzoneIdentifier);
}