using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Presentation.View.Element;
using Presentation.View.Space;
using Presentation.View.World;

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

    public RenderFragment GetRenderFragment(IDisplayableObject obj)
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
    private RenderFragment GetRenderFragmentInternal(IDisplayableObject obj)
    {
        return obj switch
        {
            WorldViewModel worldViewModel => b =>
            {
                b.OpenComponent<WorldRenderFragment>(1);
                b.AddAttribute(1, "ViewModel", worldViewModel);
                b.CloseComponent();
            },
            SpaceViewModel spaceViewModel => b =>
            {
                b.OpenComponent<SpaceRenderFragment>(1);
                b.AddAttribute(1, "ViewModel", spaceViewModel);
                b.CloseComponent();
            },
            ElementViewModel elementViewModel => b =>
            {
                b.OpenComponent<ElementRenderFragment>(1);
                b.AddAttribute(1, "ViewModel", elementViewModel);
                b.CloseComponent();
            },
            _ => throw new ArgumentOutOfRangeException(nameof(obj), "Unsupported type passed to GetRenderFragment")
        };
    }
}