using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View.LearningElement;
using Presentation.View.LearningSpace;
using Presentation.View.LearningWorld;

namespace Presentation.View.Toolbox;

public class ToolboxRenderFragmentFactory : IAbstractToolboxRenderFragmentFactory
{
    public ToolboxRenderFragmentFactory(ILogger<ToolboxRenderFragmentFactory> logger, IMemoryCache memoryCache)
    {
        Cache = memoryCache;
            //new MemoryCache(new MemoryCacheOptions());
        Logger = logger;
    }

    private IMemoryCache Cache { get; }
    private ILogger<ToolboxRenderFragmentFactory> Logger { get; }

    public RenderFragment GetRenderFragment(IDisplayableLearningObject obj)
    {
        // ReSharper disable once InvertIf - unnecessary here
        if (!Cache.TryGetValue(obj, out RenderFragment fragment))
        {
            //cache miss
            Logger.LogTrace("cache miss for type {}, name {} - rebuilding", obj.GetType().Name, obj.Name);
            fragment = GetRenderFragmentInternal(obj);
            Cache.Set(obj, fragment);
        }

        return fragment;
    }
    private RenderFragment GetRenderFragmentInternal(IDisplayableLearningObject obj)
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