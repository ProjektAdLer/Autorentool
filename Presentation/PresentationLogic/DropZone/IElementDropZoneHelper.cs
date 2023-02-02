using MudBlazor;
using Presentation.PresentationLogic.Element;

namespace Presentation.PresentationLogic.DropZone;

public interface IElementDropZoneHelper
{
    IEnumerable<IElementViewModel> GetWorldAndSpaceElements();
    void ItemUpdated(MudItemDropInfo<IElementViewModel> dropItem);
    bool IsItemInDropZone(IElementViewModel item, string dropzoneIdentifier);
}