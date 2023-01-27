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
        if (SpaceP.LearningSpaceVm == null) throw new ApplicationException("LearningSpaceVm is null");
        if (dropItem.DropzoneIdentifier == "unplacedElements")
        {
            if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");
            PresentationL.DragLearningElementToUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm, dropItem.Item);
        }
        else
        {
            var oldSlot = -1;
            if (dropItem.Item.Parent != null && dropItem.Item.Parent.ContainedLearningElements.Contains(dropItem.Item))
            {
                oldSlot = Array.IndexOf(dropItem.Item.Parent.LearningSpaceLayout.LearningElements, dropItem.Item);
            }

            var spaceId = dropItem.DropzoneIdentifier.Substring(0, Guid.Empty.ToString().Length);
            var spaceVm = WorldP.LearningWorldVm?.LearningSpaces.First(x => x.Id.ToString() == spaceId);
            if (spaceVm != SpaceP.LearningSpaceVm) throw new ApplicationException("SpaceVm is not the same");
            var slotId = int.Parse(dropItem.DropzoneIdentifier.Substring(Guid.Empty.ToString().Length));

            if (oldSlot >= 0)
            {
                PresentationL.SwitchLearningElementSlot(SpaceP.LearningSpaceVm, dropItem.Item, slotId);
            }
            else
            {
                if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");
                PresentationL.DragLearningElementFromUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm,
                    dropItem.Item, slotId);
            }
        }
    }

    public bool IsItemInDropZone(ILearningElementViewModel item, string dropzoneIdentifier)
    {
        var isInDropZone = false;
        if (item.Parent != null)
        {
            isInDropZone =
                item.Parent.Id.ToString() +
                Array.IndexOf(item.Parent.LearningSpaceLayout.LearningElements, item).ToString() == dropzoneIdentifier;
        }

        if (dropzoneIdentifier == "unplacedElements")
        {
            if (WorldP.LearningWorldVm != null)
            {
                isInDropZone = WorldP.LearningWorldVm.UnplacedLearningElements.Contains(item);
            }
        }

        return isInDropZone;
    }
}