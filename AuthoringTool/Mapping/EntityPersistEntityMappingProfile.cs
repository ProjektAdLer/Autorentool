using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using PersistEntities;
using PersistEntities.LearningContent;
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
        CreateLearningWorldMap();
        CreateLearningSpaceMap();
        CreateLearningElementMap();
        CreateLearningContentMap();
        CreatePathwayMaps();
        CreateDerivedElementMaps();
        CreateInterfaceMaps();
        CreateEnumMaps();
        CreateLearningSpaceLayoutMap();
    }

    private void CreateLearningSpaceLayoutMap()
    {
        CreateMap<ILearningSpaceLayout, LearningSpaceLayoutPe>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        CreateMap<ILearningSpaceLayoutPe, LearningSpaceLayout>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        CreateMap<LearningSpaceLayout, LearningSpaceLayoutPe>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
    }

    private void CreateEnumMaps()
    {
        CreateMap<LearningElementDifficultyEnum, LearningElementDifficultyEnumPe>()
            .ReverseMap();
        CreateMap<ConditionEnum, ConditionEnumPe>()
            .ReverseMap();
    }

    private void CreatePathwayMaps()
    {
        CreateMap<LearningPathway, LearningPathwayPe>()
            .ReverseMap();
        CreateMap<PathWayCondition, PathWayConditionPe>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore());
    }

    private void CreateLearningContentMap()
    {
        CreateMap<LearningContent, LearningContentPe>()
            .Include<FileContent, FileContentPe>()
            .Include<LinkContent, LinkContentPe>()
            .ReverseMap();
        CreateMap<FileContent, FileContentPe>()
            .ReverseMap();
        CreateMap<LinkContent, LinkContentPe>()
            .ReverseMap();
    }

    private void CreateInterfaceMaps()
    {
        CreateMap<PathWayCondition, IObjectInPathWayPe>().As<PathWayConditionPe>();
        CreateMap<LearningSpace, IObjectInPathWayPe>().As<LearningSpacePe>();

        CreateMap<H5PActivationElement, ILearningElementPe>().As<H5PActivationElementPe>();
        CreateMap<H5PInteractionElement, ILearningElementPe>().As<H5PInteractionElementPe>();
        CreateMap<H5PTestElement, ILearningElementPe>().As<H5PTestElementPe>();
        CreateMap<ImageTransferElement, ILearningElementPe>().As<ImageTransferElementPe>();
        CreateMap<PdfTransferElement, ILearningElementPe>().As<PdfTransferElementPe>();
        CreateMap<VideoActivationElement, ILearningElementPe>().As<VideoActivationElementPe>();
        CreateMap<VideoTransferElement, ILearningElementPe>().As<VideoTransferElementPe>();
        CreateMap<TextTransferElement, ILearningElementPe>().As<TextTransferElementPe>();

        CreateMap<H5PActivationElementPe, ILearningElement>().As<H5PActivationElement>();
        CreateMap<H5PInteractionElementPe, ILearningElement>().As<H5PInteractionElement>();
        CreateMap<H5PTestElementPe, ILearningElement>().As<H5PTestElement>();
        CreateMap<ImageTransferElementPe, ILearningElement>().As<ImageTransferElement>();
        CreateMap<PdfTransferElementPe, ILearningElement>().As<PdfTransferElement>();
        CreateMap<VideoActivationElementPe, ILearningElement>().As<VideoActivationElement>();
        CreateMap<VideoTransferElementPe, ILearningElement>().As<VideoTransferElement>();
        CreateMap<TextTransferElementPe, ILearningElement>().As<TextTransferElement>();

        CreateMap<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap();

        CreateMap<LearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<LearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<ILearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
    }

    private void CreateDerivedElementMaps()
    {
        CreateMap<H5PActivationElement, H5PActivationElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
        CreateMap<H5PInteractionElement, H5PInteractionElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
        CreateMap<H5PTestElement, H5PTestElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
        CreateMap<ImageTransferElement, ImageTransferElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
        CreateMap<PdfTransferElement, PdfTransferElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
        CreateMap<VideoActivationElement, VideoActivationElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
        CreateMap<VideoTransferElement, VideoTransferElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
        CreateMap<TextTransferElement, TextTransferElementPe>()
            .IncludeBase<LearningElement, LearningElementPe>()
            .ReverseMap();
    }

    private void CreateLearningElementMap()
    {
        CreateMap<LearningElement, LearningElementPe>()
            .ReverseMap()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .ForMember(x => x.Id, opt => opt.Ignore());
    }

    private void CreateLearningSpaceMap()
    {
        CreateMap<LearningSpace, LearningSpacePe>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }
            });
    }

    private void CreateLearningWorldMap()
    {
        CreateMap<LearningWorld, LearningWorldPe>()
            .ForMember(x => x.ObjectsInPathWaysPe, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.LearningPathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.SourceObject?.Id);
                    pathWay.TargetObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.TargetObject?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var objectInPathWayPe in d.ObjectsInPathWaysPe)
                {
                    objectInPathWayPe.InBoundObjects = d.LearningPathways
                        .Where(x => x.TargetObject.Id == objectInPathWayPe.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWayPe.OutBoundObjects = d.LearningPathways
                        .Where(x => x.SourceObject.Id == objectInPathWayPe.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            })
            .ReverseMap()
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.LearningPathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject?.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var objectInPathWay in d.ObjectsInPathWays)
                {
                    objectInPathWay.InBoundObjects = d.LearningPathways
                        .Where(x => x.TargetObject.Id == objectInPathWay.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWay.OutBoundObjects = d.LearningPathways
                        .Where(x => x.SourceObject.Id == objectInPathWay.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
    }
}