using Microsoft.AspNetCore.Components;
using Presentation.Components.RightClickMenu;
using Presentation.PresentationLogic.LearningPathway;

namespace Presentation.View.LearningPathWay;

public class DraggablePathWayCondition : DraggableObjectInPathWay
{
    protected override string Text => ((PathWayConditionViewModel)ObjectInPathWay).Condition.ToString();
    protected override string ObjectStyleWhenSelected => @"fill:darkgray;stroke:black;stroke-width:2";
    protected override string ObjectStyleWhenNotSelected => @"fill:darkgray;stroke:black;stroke-width:1";
    protected override string OnHoveredObjectShape => @"<circle r=""25"" style=""fill:lightblue""></circle>
                                       <circle r=""6"" transform=""translate(0,-25)"" fill=""lightblue""/>";

    protected override string ObjectShape => @"<circle r=""20"" style={0}></circle>
                                       <text x=""3"" y=""15"" style=""user-select:none;"" transform=""translate(-16,-10)"">{1}</text>";
    protected override string DeletePathButtonShape => @"<circle r=""7"" transform=""translate(0, -19)"" fill=""red"" stroke=""red""/>
                                       <polyline points=""0,0 4,0 -4,0 4,0 -4,0"" transform=""translate(0, -19)"" 
                                        style=""fill:none;stroke:white;stroke-width:1""/>";

    [Parameter, EditorRequired]
    public EventCallback<PathWayConditionViewModel> OnEditPathWayCondition { get; set; }
    [Parameter, EditorRequired]
    public EventCallback<PathWayConditionViewModel> OnDeletePathWayCondition { get; set; }

    protected override List<RightClickMenuEntry> GetRightClickMenuEntries()
    {
        return new List<RightClickMenuEntry>()
        {
            new("Edit", () => OnEditPathWayCondition.InvokeAsync((PathWayConditionViewModel)ObjectInPathWay)),
            new("Delete", () => OnDeletePathWayCondition.InvokeAsync((PathWayConditionViewModel)ObjectInPathWay)),
        };
    }
}