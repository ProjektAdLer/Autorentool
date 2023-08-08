using MudBlazor;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.DropZone;

public class LearningElementDropZoneHelper : ILearningElementDropZoneHelper
{
    public LearningElementDropZoneHelper(IPresentationLogic presentationLogic, ILearningWorldPresenter worldPresenter,
        ILearningSpacePresenter spacePresenter)
    {
        PresentationL = presentationLogic;
        WorldP = worldPresenter;
        SpaceP = spacePresenter;
    }

    private IPresentationLogic PresentationL { get; set; }
    private ILearningWorldPresenter WorldP { get; set; }
    private ILearningSpacePresenter SpaceP { get; set; }


    public IEnumerable<ILearningElementViewModel> GetWorldAndSpaceElements()
    {
        var list = Enumerable.Empty<ILearningElementViewModel>();
        if (SpaceP.LearningSpaceVm != null)
            list = list.Union(SpaceP.LearningSpaceVm.ContainedLearningElements);
        if (WorldP.LearningWorldVm != null)
            list = list.Union(WorldP.LearningWorldVm.UnplacedLearningElements);
        return list;
    }

    public void ItemUpdated(MudItemDropInfo<ILearningElementViewModel> dropItem)
    {
        if (dropItem.DropzoneIdentifier == "unplacedElements")
        {
            DragItemToUnplaced(dropItem);
        }
        else
        {
            DragItemToLayoutSlot(dropItem);
        }
    }

    public bool IsItemInDropZone(ILearningElementViewModel item, string dropzoneIdentifier)
    {
        if (item.Parent != null)
        {
            return
                item.Parent.Id.ToString() +
                item.Parent.LearningSpaceLayout.LearningElements.First(kvP => kvP.Value.Equals(item)).Key
                == dropzoneIdentifier;
        }

        if (dropzoneIdentifier != "unplacedElements") return false;
        return WorldP.LearningWorldVm != null && WorldP.LearningWorldVm.UnplacedLearningElements.Contains(item);
    }

    private void DragItemToUnplaced(MudItemDropInfo<ILearningElementViewModel> dropItem)
    {
        if (SpaceP.LearningSpaceVm == null) throw new ApplicationException("LearningSpaceVm is null");
        if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");
        switch (dropItem.Item)
        {
            case { Parent: not null } when
                dropItem.Item.Parent.ContainedLearningElements.Contains(dropItem.Item) &&
                dropItem.Item.Parent == SpaceP.LearningSpaceVm:
            {
                PresentationL.DragLearningElementToUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm,
                    dropItem.Item);
                break;
            }
            case { Parent: null } when
                WorldP.LearningWorldVm.UnplacedLearningElements.Contains(dropItem.Item):
            {
                // do nothing, because the item is already in the unplaced elements
                break;
            }
            default:
                throw new ApplicationException("DragDropItem is neither in unplaced elements nor in a learning space");
        }
    }

    private void DragItemToLayoutSlot(MudItemDropInfo<ILearningElementViewModel> dropItem)
    {
        if (dropItem.Item == null) return;
        if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");
        var oldSlot = -1;
        if (dropItem.Item.Parent != null && dropItem.Item.Parent.ContainedLearningElements.Contains(dropItem.Item))
        {
            oldSlot = dropItem.Item.Parent.LearningSpaceLayout.LearningElements
                .First(kvP => kvP.Value.Equals(dropItem.Item)).Key;
        }

        var spaceId = dropItem.DropzoneIdentifier.Substring(0, Guid.Empty.ToString().Length);
        var spaceVm = WorldP.LearningWorldVm.LearningSpaces.FirstOrDefault(x => x.Id.ToString() == spaceId);
        if (spaceVm == null) throw new ApplicationException("The space to drop to is not in the world");
        if (spaceVm != SpaceP.LearningSpaceVm)
            throw new ApplicationException("The space to drop to is not the currently selected space");
        var slotId = int.Parse(dropItem.DropzoneIdentifier.Substring(Guid.Empty.ToString().Length));

        if (oldSlot >= 0)
        {
            PresentationL.SwitchLearningElementSlot(SpaceP.LearningSpaceVm, dropItem.Item, slotId);
        }
        else
        {
            if (!WorldP.LearningWorldVm.UnplacedLearningElements.Contains(dropItem.Item))
                throw new ApplicationException("DragDropItem should be in unplaced elements");

            if (SpaceP.LearningSpaceVm.LearningSpaceLayout.LearningElements.ContainsKey(slotId))
            {
                SpaceP.OpenReplaceLearningElementDialog(WorldP.LearningWorldVm, dropItem.Item, slotId);
            }
            else
            {
                PresentationL.DragLearningElementFromUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm,
                    dropItem.Item, slotId);
            }
        }
    }
}