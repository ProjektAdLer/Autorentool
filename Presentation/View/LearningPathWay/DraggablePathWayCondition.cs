using Microsoft.AspNetCore.Components;
using Presentation.Components.RightClickMenu;
using Presentation.PresentationLogic.LearningPathway;

namespace Presentation.View.LearningPathWay;

public class DraggablePathWayCondition : DraggableObjectInPathWay
{
    protected override string OnHoveredObjectShape => @"<circle r=""25"" style=""fill:lightblue""></circle>
                                       <circle r=""6"" transform=""translate(0,-25)"" fill=""lightblue""/>";

    protected override string ObjectShape => @"<circle r=""20"" style=""fill:darkgray;stroke:black;stroke-width:{0}""></circle>
                                       <text x=""3"" y=""15"" transform=""translate(-16,-10)"">{1}</text>";
    protected override string DeletePathButtonShape => @"<circle r=""6"" transform=""translate(0, -19)"" fill=""white"" stroke=""gray""/>
                                       <polyline points=""0,0 2,0 -2,0"" transform=""translate(0, -19)"" 
                                        style=""fill:none;stroke:red;stroke-width:1""/>";

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