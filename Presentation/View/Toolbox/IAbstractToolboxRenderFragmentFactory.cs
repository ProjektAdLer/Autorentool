using Microsoft.AspNetCore.Components;
using Presentation.PresentationLogic;

namespace Presentation.View.Toolbox;

public interface IAbstractToolboxRenderFragmentFactory
{
    RenderFragment GetRenderFragment(IDisplayableLearningObject obj);
}