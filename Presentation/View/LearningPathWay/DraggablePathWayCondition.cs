using Microsoft.AspNetCore.Components;
using Presentation.Components.RightClickMenu;
using Presentation.PresentationLogic.LearningPathway;
using Shared;

namespace Presentation.View.LearningPathWay;

public class DraggablePathWayCondition : DraggableObjectInPathWay
{
    protected override string ObjectName => ((PathWayConditionViewModel)ObjectInPathWay).Condition.ToString().ToUpper();

    protected override string Text
    {
        get
        {
            switch (((PathWayConditionViewModel)ObjectInPathWay).Condition)
            {
                case ConditionEnum.Or:
                    return @"<text x={1} y={2} font-size=""12"" transform=""translate(43,26)"" pointer-events=""none"" fill=""white"" style=""user-select:none;"">{0}</text>";
                case ConditionEnum.And:
                    return @"<text x={1} y={2} font-size=""12"" transform=""translate(12,26)"" pointer-events=""none"" fill=""white"" style=""user-select:none;"">{0}</text>";
                default:
                    throw new ApplicationException("No valid condition set");
            }
        }
    }
    
    
    
    protected override string ObjectStyleWhenSelected => @"fill:rgba(226,234,242,255);opacity:80%;stroke:rgba(69,160,229,255);stroke-width:2";
    protected override string ObjectStyleWhenNotSelected => @"fill:rgba(226,234,242,255);opacity:80%;stroke:rgba(61,200,229,255);stroke-width:1";

    protected override string OnHoveredObjectShape =>
        @"<rect x=""0"" y=""0"" width=""75"" height=""41.5"" rx=""2"" style=""fill:rgb(229,189,115)""></rect>";

    protected override string ObjectShape =>
        @"<rect x=""0"" y=""0"" width=""75"" height=""41.5"" rx=""2"" style={0}></rect>";
    
    protected override string DeletePathButtonShape => @"<circle r=""7"" transform=""translate(37.5, -3)"" fill=""red"" stroke=""red""/>
                                       <polyline points=""0,0 4,0 -4,0 4,0 -4,0"" transform=""translate(37.5, -3)"" 
                                        style=""fill:none;stroke:white;stroke-width:1""/>";
    
    protected override string DeleteObjectButtonShape => @"<text font-size=""12"" transform=""translate(65,12)"" fill=""gray"" style=""user-select:none; cursor: pointer"">X</text>";

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