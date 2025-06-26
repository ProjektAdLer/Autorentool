using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Presentation.PresentationLogic.LearningPathway;
using Shared;

namespace Presentation.View.LearningPathWay;

public class DraggablePathWayCondition : DraggableObjectInPathWay
{
    protected override string ObjectInPathwayDeletionTitle =>
        Localizer["DraggablePathWayCondition.Delete"].Value;

    protected override string ObjectName => ((PathWayConditionViewModel)ObjectInPathWay).Condition.ToString().ToUpper();

    protected override string Text
    {
        get
        {
            switch (((PathWayConditionViewModel)ObjectInPathWay).Condition)
            {
                case ConditionEnum.Or:
                    return
                        @"<text x={1} y={2} font-size=""12"" transform=""translate(43,26)"" pointer-events=""none"" fill=""white"" style=""user-select:none;"">{0}</text>";
                case ConditionEnum.And:
                    return
                        @"<text x={1} y={2} font-size=""12"" transform=""translate(12,26)"" pointer-events=""none"" fill=""white"" style=""user-select:none;"">{0}</text>";
                default:
                    throw new ApplicationException("No valid condition set");
            }
        }
    }

    protected override string ObjectInPathwayTitle => "";

    protected override string ObjectStyleWhenSelected =>
        @"fill:rgba(226,234,242,255);opacity:80%;stroke:rgba(61,200,229,255);stroke-width:1";

    protected override string ObjectStyleWhenNotSelected =>
        @"fill:#e9e9e9;opacity:80%;stroke:rgb(204,204,204);stroke-width:1";

    protected override string OnHoveredObjectShape =>
        @"<rect x=""0"" y=""0"" width=""75"" height=""41.5"" rx=""2"" style=""fill:rgb(229,189,115);stroke:rgba(229,189,115,0.5);stroke-width:8""></rect>";

    protected override string ObjectShape =>
        @"<rect x=""0"" y=""0"" width=""75"" height=""41.5"" rx=""2"" style={0}></rect>";

    protected override string DeletePathButtonShape =>
        @"<circle r=""7"" transform=""translate(37.5, -3)"" fill=""rgb(23,45,77)"" stroke=""rgb(23,45,77)""/>
                                       <polyline points=""0,0 4,0 -4,0 4,0 -4,0"" transform=""translate(37.5, -3)"" 
                                        style=""fill:none;stroke:white;stroke-width:1""/>";

    protected override string DeleteObjectButtonShape =>
        @"<g transform=""translate(59,-1) scale(0.8,0.8)"">
        <path d=""M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"" fill=""gray"" style=""user-select:none; cursor: pointer""/>
    </g>";

    protected override string DeleteObjectConfirmationDialogText => 
        Localizer["DraggablePathWayCondition.DeleteConfirmationDialog.Text", 
        Localizer["DraggablePathWayCondition." + ((PathWayConditionViewModel)ObjectInPathWay).Condition].Value].Value;
    protected override string DeleteObjectConfirmationDialogTitle  => Localizer["DraggablePathWayCondition.Delete"].Value;

    [Parameter, EditorRequired] public EventCallback<PathWayConditionViewModel> OnDeletePathWayCondition { get; set; }
    
    [Inject, AllowNull] internal IStringLocalizer<DraggablePathWayCondition> Localizer { get; set; }

    protected override string DeleteObjectConfirmationDialogSubmitButtonText =>
        Localizer["DraggablePathWayCondition.DeleteObjectConfirmationDialog.SubmitButtonText"].Value;

    protected override string SnackBarDeletionMessage => Localizer["DraggablePathWayCondition.SnackbarDeletionMessage"].Value;
}