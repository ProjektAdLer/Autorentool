using MudBlazor;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.DropZone;

public class LearningElementDropZoneHelper : ILearningElementDropZoneHelper
{
    public LearningElementDropZoneHelper(ILearningWorldPresenter worldPresenter, ILearningSpacePresenter spacePresenter)
    {
        WorldPresenter = worldPresenter;
        SpacePresenter = spacePresenter;
    }

    private ILearningSpacePresenter SpacePresenter { get; set; }

    private ILearningWorldPresenter WorldPresenter { get; set; }

    public IEnumerable<ILearningElementViewModel> GetWorldAndSpaceElements()
    {
        var list = Enumerable.Empty<ILearningElementViewModel>();
        if (SpacePresenter.LearningSpaceVm != null)
            list = list.Union(SpacePresenter.LearningSpaceVm.ContainedLearningElements);
        if (WorldPresenter.LearningWorldVm != null)
            list = list.Union(WorldPresenter.LearningWorldVm.UnplacedLearningElements);
        return list;
    }

    public void ItemUpdated(MudItemDropInfo<ILearningElementViewModel> dropItem)
    {
        var oldSlot = -1;
        if (dropItem.Item.Parent != null && dropItem.Item.Parent.ContainedLearningElements.Contains(dropItem.Item))
        {
            oldSlot = Array.IndexOf(dropItem.Item.Parent.LearningSpaceLayout.LearningElements, dropItem.Item);
        }

        if (dropItem.DropzoneIdentifier == "unplacedElements")
        {
            if (oldSlot >= 0 && dropItem.Item.Parent != null)
            {
                dropItem.Item.Parent.LearningSpaceLayout.LearningElements[oldSlot] = null;
                dropItem.Item.Parent = null;
            }

            if (WorldPresenter.LearningWorldVm?.UnplacedLearningElements.Contains(dropItem.Item) == false)
            {
                WorldPresenter.LearningWorldVm?.UnplacedLearningElements.Add(dropItem.Item);
            }
        }
        else
        {
            var spaceId = dropItem.DropzoneIdentifier.Substring(0, Guid.Empty.ToString().Length);
            var spaceVm = WorldPresenter.LearningWorldVm?.LearningSpaces.First(x => x.Id.ToString() == spaceId);
            if (spaceVm == null) return;
            var slotId = int.Parse(dropItem.DropzoneIdentifier.Substring(Guid.Empty.ToString().Length));

            if (oldSlot >= 0)
            {
                dropItem.Item.Parent!.LearningSpaceLayout.LearningElements[oldSlot] =
                    spaceVm.LearningSpaceLayout.LearningElements[slotId];
            }
            else
            {
                if (spaceVm.LearningSpaceLayout.LearningElements[slotId] is { } oldElement)
                {
                    oldElement.Parent = null;
                    if (WorldPresenter.LearningWorldVm?.UnplacedLearningElements.Contains(oldElement) == false)
                    {
                        WorldPresenter.LearningWorldVm?.UnplacedLearningElements.Add(oldElement);
                    }
                }
            }

            dropItem.Item.Parent = spaceVm;

            dropItem.Item.Parent.LearningSpaceLayout.LearningElements[slotId] = dropItem.Item;
            WorldPresenter.LearningWorldVm?.UnplacedLearningElements.Remove(dropItem.Item);
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
            if (WorldPresenter.LearningWorldVm != null)
            {
                isInDropZone = WorldPresenter.LearningWorldVm.UnplacedLearningElements.Contains(item);
            }
        }

        return isInDropZone;
    }
}