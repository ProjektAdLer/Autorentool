using AuthoringTool.PresentationLogic;
using Microsoft.AspNetCore.Components;

namespace AuthoringTool.View.Toolbox;

public interface IAbstractToolboxRenderFragmentFactory
{
    RenderFragment GetRenderFragment(IDisplayableLearningObject obj);
}