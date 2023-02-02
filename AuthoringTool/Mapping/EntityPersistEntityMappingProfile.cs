using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using PersistEntities;
using Shared;

namespace AuthoringTool.Mapping;

/// <summary>
/// Configures mappings between Entity and PersistEntity classes
/// </summary>
public class EntityPersistEntityMappingProfile : Profile
{
    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new EntityPersistEntityMappingProfile());
    };

    private EntityPersistEntityMappingProfile()
    {
        DisableConstructorMapping();
        CreateWorldMap();
        CreateSpaceMap();
        CreateElementMap();
        CreateContentMap();
        CreatePathwayMaps();
        CreateDerivedElementMaps();
        CreateInterfaceMaps();
        CreateEnumMaps();
        CreateSpaceLayoutMap();
    }

    private void CreateSpaceLayoutMap()
    {
        CreateMap<ISpaceLayout, SpaceLayoutPe>()
            .ForMember(x => x.ContainedElements, opt => opt.Ignore());
        CreateMap<ISpaceLayoutPe, SpaceLayout>()
            .ForMember(x => x.ContainedElements, opt => opt.Ignore());
        CreateMap<SpaceLayout, SpaceLayoutPe>()
            .ForMember(x => x.ContainedElements, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(x => x.ContainedElements, opt => opt.Ignore());
    }

    private void CreateEnumMaps()
    {
        CreateMap<ElementDifficultyEnum, ElementDifficultyEnumPe>()
            .ReverseMap();
        CreateMap<ConditionEnum, ConditionEnumPe>()
            .ReverseMap();
    }

    private void CreatePathwayMaps()
    {
        CreateMap<Pathway, PathwayPe>()
            .ReverseMap();
        CreateMap<PathWayCondition, PathWayConditionPe>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore());
    }

    private void CreateContentMap()
    {
        CreateMap<Content, ContentPe>()
            .ReverseMap();
    }

    private void CreateInterfaceMaps()
    {
        CreateMap<PathWayCondition, IObjectInPathWayPe>().As<PathWayConditionPe>();
        CreateMap<Space, IObjectInPathWayPe>().As<SpacePe>();

        CreateMap<H5PActivationElement, IElementPe>().As<H5PActivationElementPe>();
        CreateMap<H5PInteractionElement, IElementPe>().As<H5PInteractionElementPe>();
        CreateMap<H5PTestElement, IElementPe>().As<H5PTestElementPe>();
        CreateMap<ImageTransferElement, IElementPe>().As<ImageTransferElementPe>();
        CreateMap<PdfTransferElement, IElementPe>().As<PdfTransferElementPe>();
        CreateMap<VideoActivationElement, IElementPe>().As<VideoActivationElementPe>();
        CreateMap<VideoTransferElement, IElementPe>().As<VideoTransferElementPe>();
        CreateMap<TextTransferElement, IElementPe>().As<TextTransferElementPe>();

        CreateMap<H5PActivationElementPe, IElement>().As<H5PActivationElement>();
        CreateMap<H5PInteractionElementPe, IElement>().As<H5PInteractionElement>();
        CreateMap<H5PTestElementPe, IElement>().As<H5PTestElement>();
        CreateMap<ImageTransferElementPe, IElement>().As<ImageTransferElement>();
        CreateMap<PdfTransferElementPe, IElement>().As<PdfTransferElement>();
        CreateMap<VideoActivationElementPe, IElement>().As<VideoActivationElement>();
        CreateMap<VideoTransferElementPe, IElement>().As<VideoTransferElement>();
        CreateMap<TextTransferElementPe, IElement>().As<TextTransferElement>();

        CreateMap<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap();

        CreateMap<SpaceLayout, ISpaceLayoutPe>().As<SpaceLayoutPe>();
        CreateMap<SpaceLayoutPe, ISpaceLayout>().As<SpaceLayout>();
        CreateMap<ISpaceLayout, ISpaceLayoutPe>().As<SpaceLayoutPe>();
        CreateMap<ISpaceLayoutPe, ISpaceLayout>().As<SpaceLayout>();
    }

    private void CreateDerivedElementMaps()
    {
        CreateMap<H5PActivationElement, H5PActivationElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
        CreateMap<H5PInteractionElement, H5PInteractionElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
        CreateMap<H5PTestElement, H5PTestElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
        CreateMap<ImageTransferElement, ImageTransferElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
        CreateMap<PdfTransferElement, PdfTransferElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
        CreateMap<VideoActivationElement, VideoActivationElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
        CreateMap<VideoTransferElement, VideoTransferElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
        CreateMap<TextTransferElement, TextTransferElementPe>()
            .IncludeBase<Element, ElementPe>()
            .ReverseMap();
    }

    private void CreateElementMap()
    {
        CreateMap<Element, ElementPe>()
            .ReverseMap()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .ForMember(x => x.Id, opt => opt.Ignore());
    }

    private void CreateSpaceMap()
    {
        CreateMap<Space, SpacePe>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var element in d.ContainedElements)
                {
                    element.Parent = d;
                }
            });
    }

    private void CreateWorldMap()
    {
        CreateMap<World, WorldPe>()
            .ForMember(x => x.ObjectsInPathWaysPe, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.Pathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.SourceObject?.Id);
                    pathWay.TargetObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.TargetObject?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var objectInPathWayPe in d.ObjectsInPathWaysPe)
                {
                    objectInPathWayPe.InBoundObjects = d.Pathways
                        .Where(x => x.TargetObject.Id == objectInPathWayPe.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWayPe.OutBoundObjects = d.Pathways
                        .Where(x => x.SourceObject.Id == objectInPathWayPe.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            })
            .ReverseMap()
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.Pathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject?.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var objectInPathWay in d.ObjectsInPathWays)
                {
                    objectInPathWay.InBoundObjects = d.Pathways
                        .Where(x => x.TargetObject.Id == objectInPathWay.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWay.OutBoundObjects = d.Pathways
                        .Where(x => x.SourceObject.Id == objectInPathWay.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
    }
}