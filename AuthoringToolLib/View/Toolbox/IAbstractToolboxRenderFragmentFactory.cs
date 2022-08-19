using AuthoringToolLib.PresentationLogic;
using Microsoft.AspNetCore.Components;

namespace AuthoringToolLib.View.Toolbox;

public interface IAbstractToolboxRenderFragmentFactory
{
    RenderFragment GetRenderFragment(IDisplayableLearningObject obj);
}