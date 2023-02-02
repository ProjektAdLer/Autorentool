using MudBlazor;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.DropZone;

public class ElementDropZoneHelper : IElementDropZoneHelper
{
    public ElementDropZoneHelper(IPresentationLogic presentationLogic, IWorldPresenter worldPresenter,
        ISpacePresenter spacePresenter)
    {
        PresentationL = presentationLogic;
        WorldP = worldPresenter;
        SpaceP = spacePresenter;
    }

    private IPresentationLogic PresentationL { get; set; }
    private IWorldPresenter WorldP { get; set; }
    private ISpacePresenter SpaceP { get; set; }


    public IEnumerable<IElementViewModel> GetWorldAndSpaceElements()
    {
        var list = Enumerable.Empty<IElementViewModel>();
        if (SpaceP.SpaceVm != null)
            list = list.Union(SpaceP.SpaceVm.ContainedElements);
        if (WorldP.WorldVm != null)
            list = list.Union(WorldP.WorldVm.UnplacedElements);
        return list;
    }

    public void ItemUpdated(MudItemDropInfo<IElementViewModel> dropItem)
    {
        if (SpaceP.SpaceVm == null) throw new ApplicationException("SpaceVm is null");
        if (dropItem.DropzoneIdentifier == "unplacedElements")
        {
            if (WorldP.WorldVm == null) throw new ApplicationException("WorldVm is null");
            PresentationL.DragElementToUnplaced(WorldP.WorldVm, SpaceP.SpaceVm, dropItem.Item);
        }
        else
        {
            var oldSlot = -1;
            if (dropItem.Item.Parent != null && dropItem.Item.Parent.ContainedElements.Contains(dropItem.Item))
            {
                oldSlot = Array.IndexOf(dropItem.Item.Parent.SpaceLayout.Elements, dropItem.Item);
            }

            var spaceId = dropItem.DropzoneIdentifier.Substring(0, Guid.Empty.ToString().Length);
            var spaceVm = WorldP.WorldVm?.Spaces.First(x => x.Id.ToString() == spaceId);
            if (spaceVm != SpaceP.SpaceVm) throw new ApplicationException("SpaceVm is not the same");
            var slotId = int.Parse(dropItem.DropzoneIdentifier.Substring(Guid.Empty.ToString().Length));

            if (oldSlot >= 0)
            {
                PresentationL.SwitchElementSlot(SpaceP.SpaceVm, dropItem.Item, slotId);
            }
            else
            {
                if (WorldP.WorldVm == null) throw new ApplicationException("WorldVm is null");
                PresentationL.DragElementFromUnplaced(WorldP.WorldVm, SpaceP.SpaceVm,
                    dropItem.Item, slotId);
            }
        }
    }

    public bool IsItemInDropZone(IElementViewModel item, string dropzoneIdentifier)
    {
        var isInDropZone = false;
        if (item.Parent != null)
        {
            isInDropZone =
                item.Parent.Id.ToString() +
                Array.IndexOf(item.Parent.SpaceLayout.Elements, item).ToString() == dropzoneIdentifier;
        }

        if (dropzoneIdentifier == "unplacedElements")
        {
            if (WorldP.WorldVm != null)
            {
                isInDropZone = WorldP.WorldVm.UnplacedElements.Contains(item);
            }
        }

        return isInDropZone;
    }
}