using Microsoft.AspNetCore.Components;
using Presentation.Components.RightClickMenu;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.View.LearningPathWay;

public class DraggableLearningSpace : DraggableObjectInPathWay
{
    protected override string ObjectName => ((ILearningSpaceViewModel)ObjectInPathWay).Name + Topic;
    protected override string Text => "";
    
    protected override string Title => ((ILearningSpaceViewModel)ObjectInPathWay).Name;

    protected override string ObjectStyleWhenSelected =>
        @"fill:rgb(184,210,229);opacity:80%;stroke:rgb(138,190,229);stroke-width:75";

    protected override string ObjectStyleWhenNotSelected =>
        @"fill:#e9e9e9;opacity:80%;stroke:rgb(204,204,204);stroke-width:50";

    protected override string OnHoveredObjectShape =>
        @"<rect transform=""translate(0,0)"" height=""5rem"" width=""5rem"" rx=5 style=""fill:rgb(229,189,115);stroke:rgba(229,189,115,0.5);stroke-width:8""></rect>
        ";

    protected override string ObjectShape =>
        @"<?xml version=""1.0"" encoding=""UTF-8""?>
<svg id=""uuid-ed2d2eeb-03d9-453b-94f0-6f55eb9b4a74"" data-name=""R-Lernraum"" xmlns=""http://www.w3.org/2000/svg"" width=""5rem"" height=""5rem"" viewBox=""-100 -100 2200 2200"">
  <polygon points=""152 1409.6 956.37 945.2 1760.73 1409.6 956.37 1874 152 1409.6"" style=""fill: #e9f2fa; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""/>
  <path d=""m977.38,899.86c-4.98-2.88-11.13-2.88-16.12,0L64.03,1417.89c-4.99,2.88-8.07,8.21-8.07,13.97,0,5.76,3.08,11.09,8.08,13.98l897.23,518.02c4.98,2.88,11.13,2.88,16.12,0l897.23-518.02c4.99-2.88,8.07-8.21,8.08-13.97,0-5.77-3.08-11.09-8.06-13.97l-897.24-518.02Zm-8.06,32.6l864.97,499.39-864.97,499.39L104.35,1431.85l864.97-499.39h0Z"" style=""fill: #08234d; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""/>
  <g>
    <polygon points=""999.57 138 1834 594.38 1834 1336 999.57 879.62 999.57 138"" style=""fill: #e9f2fa; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""/>
    <path d=""m1901,576.36c0-5.75-3.08-11.07-8.07-13.95L995.11,45.16c-4.99-2.88-11.15-2.87-16.14,0-4.99,2.87-8.07,8.19-8.07,13.95v840.54c0,5.76,3.08,11.07,8.07,13.95l897.81,517.25c4.99,2.88,11.15,2.88,16.14,0,4.99-2.87,8.07-8.19,8.07-13.95v-840.54Zm-32.29,9.3v803.33s-777.55-447.97-865.53-498.66V87.01l865.53,498.66h0Z"" style=""fill: #172d4d; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""/>
  </g>
  <g>
    <polygon points=""140 594.38 937 138 937 879.62 140 1336 140 594.38"" style=""fill: #e9f2fa; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""/>
    <path d=""m991.23,59.27c0-5.82-3.1-11.19-8.13-14.09-5.03-2.9-11.24-2.91-16.27,0L61.9,567.64c-5.03,2.91-8.13,8.28-8.13,14.09v849c0,5.82,3.1,11.19,8.13,14.09,5.03,2.91,11.24,2.91,16.27,0l904.93-522.46c5.03-2.91,8.13-8.27,8.13-14.09V59.27Zm-32.54,28.18v811.42L86.3,1402.54v-811.42L958.69,87.45h0Z"" style=""fill: #172d4d; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""/>
  </g>
    <rect transform=""translate(-50,-50)"" height=""2100"" width=""2100"" rx=100 style={0}></rect>
            <foreignObject x=""80"" y=""750"" width=""1800"" height=""600"">
                <p style=""font-family: Roboto, sans-serif; font-size: 350px; font-weight:bold; user-select:none; color:#111111; text-align:center;"">{1}</p>
            </foreignObject>
</svg>";

    protected override string DeletePathButtonShape =>
        @"<circle r=""7"" transform=""translate(40,0)"" fill=""rgb(23,45,77)"" stroke=""rgb(23,45,77)""/>
                                    <polyline points=""0,0 4,0 -4,0 4,0 -4,0"" transform=""translate(40,0)"" 
                                    style=""fill:none;stroke:white;stroke-width:1""/>";

    protected override string DeleteObjectButtonShape =>
        @"<text font-size=""16"" transform=""translate(66,16)"" font-weight=""bold"" fill=""gray"" style=""user-select:none; cursor: pointer"">x</text>";

    private string Topic => ((ILearningSpaceViewModel)ObjectInPathWay).AssignedTopic == null
        ? ""
        : "(" + ((ILearningSpaceViewModel)ObjectInPathWay).AssignedTopic!.Name + ")";

    [Parameter, EditorRequired] public EventCallback<ILearningSpaceViewModel> OnOpenLearningSpace { get; set; }

    [Parameter, EditorRequired] public EventCallback<ILearningSpaceViewModel> OnEditLearningSpace { get; set; }

    [Parameter, EditorRequired] public EventCallback<ILearningSpaceViewModel> OnDeleteLearningSpace { get; set; }

    [Parameter] public EventCallback<ILearningSpaceViewModel>? OnRemoveLearningSpaceFromTopic { get; set; }

    protected override List<RightClickMenuEntry> GetRightClickMenuEntries()
    {
        var menuEntries = new List<RightClickMenuEntry>
        {
            new("Open", () => OnOpenLearningSpace.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay)),
            new("Edit", () => OnEditLearningSpace.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay)),
            new("Delete", () => OnDeleteLearningSpace.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay))
        };

        if (((LearningSpaceViewModel)ObjectInPathWay).AssignedTopic != null)
        {
            menuEntries.Add(new RightClickMenuEntry("Remove topic",
                () => OnRemoveLearningSpaceFromTopic?.InvokeAsync((ILearningSpaceViewModel)ObjectInPathWay)));
        }

        return menuEntries;
    }
}