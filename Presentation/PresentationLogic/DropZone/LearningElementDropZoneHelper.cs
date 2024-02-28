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
        {
            list = list.Union(SpaceP.LearningSpaceVm.ContainedLearningElements);
            list = list.Union(SpaceP.LearningSpaceVm.LearningSpaceLayout.StoryElements.Values);
        }

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

        bool CheckInElementDropZone() => CheckInDropZone("ele", item.Parent.LearningSpaceLayout.LearningElements);
        bool CheckInStoryDropZone() => CheckInDropZone("story", item.Parent.LearningSpaceLayout.StoryElements);

        bool CheckInDropZone(string dropzoneType, IDictionary<int, ILearningElementViewModel> dict) =>
            $"{item.Parent.Id.ToString()}_{dropzoneType}_{dict.First(kvP => kvP.Value.Equals(item)).Key}" ==
            dropzoneIdentifier;
    }

    private void DragItemToUnplaced(MudItemDropInfo<ILearningElementViewModel> dropItem)
    {
        if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");
        var element = dropItem.Item;

        if (element is null)
            throw new ApplicationException("Received null element from MudItemDropInfo");
        switch (element.Parent)
        {
            case null when WorldP.LearningWorldVm.UnplacedLearningElements.Contains(element):
                return; //we can simply return because the item is already in unplaced elements, where it's trying to be placed
            case null:
                throw new ApplicationException("DragDropItem has no parent");
        }

        if (element.Parent != SpaceP.LearningSpaceVm)
            throw new ApplicationException("DragDropItem's parent is not the selected space");

        //at this point selected space can't be null, because the element is in a space
        if (SpaceP.LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        switch (dropItem.Item!.LearningContent)
        {
            case IStoryContentViewModel when !element.Parent.LearningSpaceLayout.StoryElements.Values.Contains(element):
            case IFileContentViewModel or ILinkContentViewModel or IAdaptivityContentViewModel
                when !element.Parent.LearningSpaceLayout.LearningElements.Values.Contains(element):
                throw new InvalidOperationException("Space does not contain element");
            case IStoryContentViewModel:
                PresentationL.DragStoryElementToUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm, element);
                break;
            case IFileContentViewModel or ILinkContentViewModel or IAdaptivityContentViewModel:
                PresentationL.DragLearningElementToUnplaced(WorldP.LearningWorldVm, SpaceP.LearningSpaceVm,
                    element);
                break;
            default:
            {
                throw new InvalidOperationException("LearningContent type is not recognized");
            }
        }
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
            case "ele":
                DragLearningElmeentToLayoutSlot(dropItem, slotId);
                break;
            case "story":
                DragStoryElementToLayoutSlot(dropItem, slotId);
                break;
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