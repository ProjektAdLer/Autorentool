using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.Entities.LearningContent.Story;
using MudBlazor;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningContent.Story;
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
            switch (item.LearningContent)
            {
                case IFileContentViewModel or ILinkContentViewModel or IAdaptivityContentViewModel:
                    return CheckInElementDropZone();
                case IStoryContentViewModel: return CheckInStoryDropZone();
            }
        }

        if (dropzoneIdentifier != "unplacedElements") return false;
        return WorldP.LearningWorldVm != null && WorldP.LearningWorldVm.UnplacedLearningElements.Contains(item);

        bool CheckInElementDropZone() => CheckInDropZone("ele");
        bool CheckInStoryDropZone() => CheckInDropZone("story");

        bool CheckInDropZone(string dropzoneType) =>
            $"{item.Parent.Id.ToString()}_{dropzoneType}_{item.Parent.LearningSpaceLayout.LearningElements.First(kvP => kvP.Value.Equals(item)).Key}" ==
            dropzoneIdentifier;
    }

    private void DragItemToUnplaced(MudItemDropInfo<ILearningElementViewModel> dropItem)
    {
        if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");
        var element = dropItem.Item;

        if (element is null)
            throw new ApplicationException("Received null element from MudItemDropInfo");
        if (element.Parent is null && WorldP.LearningWorldVm.UnplacedLearningElements.Contains(element))
            return; //we can simply return because the item is already in unplaced elements, where it's trying to be placed

        //at this point selected space can't be null, because the element is in a space
        if (SpaceP.LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        if (element.Parent is null ||
            !element.Parent.ContainedLearningElements.Contains(element) ||
            element.Parent != SpaceP.LearningSpaceVm)
        {
            //if we got here, the element is neither in unplaced elements nor in the selected space
            throw new ApplicationException("DragDropItem is neither in unplaced elements nor in a learning space");
        }

        PresentationL.DragLearningElementToUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm,
            element);
    }

    private void DragItemToLayoutSlot(MudItemDropInfo<ILearningElementViewModel> dropItem)
    {
        if (dropItem.Item == null) return;
        if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");

        var split = dropItem.DropzoneIdentifier.Split("_");
        var spaceId = split[0];
        var spaceVm = WorldP.LearningWorldVm.LearningSpaces.FirstOrDefault(x => x.Id.ToString() == spaceId);
        if (spaceVm == null) throw new ApplicationException("The space to drop to is not in the world");
        if (spaceVm != SpaceP.LearningSpaceVm)
            throw new ApplicationException("The space to drop to is not the currently selected space");
        var dropzoneType = split[1];
        var slotId = int.Parse(split[2]);

        switch (dropzoneType)
        {
            case "ele" when dropItem.Item.LearningContent is IFileContentViewModel or ILinkContentViewModel
                or IAdaptivityContentViewModel:
                DragLearningElmeentToLayoutSlot(dropItem, slotId);
                break;
            case "ele":
                return;
            case "story" when dropItem.Item.LearningContent is IStoryContentViewModel:
                DragStoryElementToLayoutSlot(dropItem, slotId);
                break;
            case "story":
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(dropItem), dropItem.DropzoneIdentifier,
                    "DropzoneIdentifier is not recognized");
        }
    }

    private void DragLearningElmeentToLayoutSlot(MudItemDropInfo<ILearningElementViewModel> dropItem, int slotId)
    {
        var oldSlot = dropItem.Item!.Parent?.LearningSpaceLayout.LearningElements
            .First(kvP => kvP.Value.Equals(dropItem.Item)).Key;
        if (oldSlot is not null)
        {
            PresentationL.SwitchLearningElementSlot(SpaceP.LearningSpaceVm!, dropItem.Item, slotId);
        }
        else
        {
            if (!WorldP.LearningWorldVm!.UnplacedLearningElements.Contains(dropItem.Item))
                throw new ApplicationException("DragDropItem should be in unplaced elements");

            if (SpaceP.LearningSpaceVm!.LearningSpaceLayout.LearningElements.ContainsKey(slotId))
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

    private void DragStoryElementToLayoutSlot(MudItemDropInfo<ILearningElementViewModel> dropItem, int slotId)
    {
        var oldSlot = dropItem.Item!.Parent?.LearningSpaceLayout.StoryElements
            .First(kvp => kvp.Value.Equals(dropItem.Item)).Key;
        if (oldSlot is not null)
        {
            PresentationL.SwitchStoryElementSlot(SpaceP.LearningSpaceVm!, dropItem.Item, slotId);
        }
        else
        {
            if (!WorldP.LearningWorldVm!.UnplacedLearningElements.Contains(dropItem.Item))
                throw new ApplicationException("DragDropItem should be in unplaced elements");

            if (SpaceP.LearningSpaceVm!.LearningSpaceLayout.StoryElements.ContainsKey(slotId))
            {
                SpaceP.OpenReplaceStoryElementDialog(WorldP.LearningWorldVm, dropItem.Item, slotId);
            }
            else
            {
                PresentationL.DragStoryElementFromUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm,
                    dropItem.Item, slotId);
            }
        }
    }
}