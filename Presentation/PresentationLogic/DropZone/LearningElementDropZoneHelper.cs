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
            var spaceId = dropItem.DropzoneIdentifier.Substring(0, Guid.Empty.ToString().Length);
            var spaceVm = WorldP.LearningWorldVm?.LearningSpaces.First(x => x.Id.ToString() == spaceId);
            if (spaceVm != SpaceP.LearningSpaceVm) throw new ApplicationException("SpaceVm is not the same");
            var slotId = int.Parse(dropItem.DropzoneIdentifier.Substring(Guid.Empty.ToString().Length));
            
            if (dropItem.Item.Parent != null && dropItem.Item.Parent.ContainedLearningElements.Contains(dropItem.Item))
            {
                PresentationL.SwitchLearningElementSlot(SpaceP.LearningSpaceVm, dropItem.Item, slotId);
            }
            else
            {
                if (WorldP.LearningWorldVm == null) throw new ApplicationException("LearningWorldVm is null");
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

    public bool IsItemInDropZone(ILearningElementViewModel item, string dropzoneIdentifier)
    {
        if (item.Parent != null)
        {
           return 
                item.Parent.Id.ToString() +
                item.Parent.LearningSpaceLayout.LearningElements.First(kvP => kvP.Value.Equals(item))
                == dropzoneIdentifier;
        }

        if (dropzoneIdentifier != "unplacedElements") return false;
        return WorldP.LearningWorldVm != null && WorldP.LearningWorldVm.UnplacedLearningElements.Contains(item);
    }
}