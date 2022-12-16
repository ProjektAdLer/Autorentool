using Microsoft.AspNetCore.Components;
using Presentation.Components.RightClickMenu;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.View.LearningWorld;

namespace Presentation.View.LearningPathWay;

public class DraggableLearningSpace : DraggableObjectInPathWay
{
    protected override string Text => ((ILearningSpaceViewModel)ObjectInPathWay).Name + Topic;
    protected override string OnHoveredObjectShape =>  @"<rect transform=""translate(-3, -3)"" height=""56"" width=""106"" fill=""lightblue""></rect>
                                    <circle r=""6"" transform=""translate(50, 0)"" fill=""lightblue""/>";

    protected override string ObjectShape =>  @"<rect height=""50"" width=""100"" style=""fill:lightgreen;stroke:black;stroke-width:{0}""></rect> 
                                    <text x=""3"" y=""15"">{1}</text>";

    protected override string DeletePathButtonShape => @"<circle r=""6"" transform=""translate(50, 0)"" fill=""white"" stroke=""gray""/>
                                    <polyline points=""0,0 2,0 -2,0"" transform=""translate(50,0)"" 
                                    style=""fill:none;stroke:red;stroke-width:1""/>";
    
    private string Topic => ((ILearningSpaceViewModel)ObjectInPathWay).AssignedTopic == null ? "" : "(" + ((ILearningSpaceViewModel)ObjectInPathWay).AssignedTopic!.Name + ")";

    [Parameter, EditorRequired]
    public EventCallback<ILearningSpaceViewModel> OnOpenLearningSpace { get; set; }
    [Parameter, EditorRequired]
    public EventCallback<ILearningSpaceViewModel> OnEditLearningSpace { get; set; }
    [Parameter, EditorRequired]
    public EventCallback<ILearningSpaceViewModel> OnDeleteLearningSpace { get; set; }
    [Parameter, EditorRequired]
    public EventCallback<ILearningSpaceViewModel> OnRemoveLearningSpaceFromTopic { get; set; }
    protected override List<RightClickMenuEntry> GetRightClickMenuEntries()
    {
        var menuEntries = new List<RightClickMenuEntry>()
        {
            new("Open", () => OnOpenLearningSpace.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay)),
            new("Edit", () => OnEditLearningSpace.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay)),
            new("Delete", () => OnDeleteLearningSpace.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay))
        };

        if (((LearningSpaceViewModel)ObjectInPathWay).AssignedTopic != null)
        {
            menuEntries.Add(new("Remove topic", () => OnRemoveLearningSpaceFromTopic.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay)));
        }

        return menuEntries;
    }
}