using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.View.LearningElement;
using AuthoringTool.View.LearningSpace;
using AuthoringTool.View.LearningWorld;
using Microsoft.AspNetCore.Components;

namespace AuthoringTool.View.Toolbox;

public class ToolboxRenderFragmentFactory : IAbstractToolboxRenderFragmentFactory
{
    public RenderFragment GetRenderFragment(IDisplayableLearningObject obj)
    {
        return obj switch
        {
            LearningWorldViewModel learningWorldViewModel => b =>
            {
                b.OpenComponent<LearningWorldRenderFragment>(1);
                b.AddAttribute(1, "ViewModel", learningWorldViewModel);
                b.CloseComponent();
            },
            LearningSpaceViewModel learningSpaceViewModel => b =>
            {
                b.OpenComponent<LearningSpaceRenderFragment>(1);
                b.AddAttribute(1, "ViewModel", learningSpaceViewModel);
                b.CloseComponent();
            },
            LearningElementViewModel learningElementViewModel => b =>
            {
                b.OpenComponent<LearningElementRenderFragment>(1);
                b.AddAttribute(1, "ViewModel", learningElementViewModel);
                b.CloseComponent();
            },
            _ => throw new ArgumentOutOfRangeException(nameof(obj), "Unsupported type passed to GetRenderFragment")
        };
    }
}