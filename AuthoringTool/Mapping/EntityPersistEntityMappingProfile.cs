using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using PersistEntities;
using PersistEntities.LearningContent;

namespace AuthoringTool.Mapping;

/// <summary>
/// Configures mappings between Entity and PersistEntity classes
/// </summary>
public class EntityPersistEntityMappingProfile : Profile
{
    private EntityPersistEntityMappingProfile()
    {
        DisableConstructorMapping();
        CreateLearningWorldMap();
        CreateLearningSpaceMap();
        CreateLearningElementMap();
        CreateLearningContentMap();
        CreatePathwayMaps();
        CreateInterfaceMaps();
        CreateLearningSpaceLayoutMap();
        CreateTopicMap();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new EntityPersistEntityMappingProfile());
        cfg.AddCollectionMappersOnce();
    };

    private void CreateTopicMap()
    {
        CreateMap<Topic, TopicPe>()
            .EqualityComparison((entity, pe) => entity.Id == pe.Id)
            .ReverseMap();
    }

    private void CreateLearningSpaceLayoutMap()
    {
        CreateMap<ILearningSpaceLayout, LearningSpaceLayoutPe>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.Capacity, opt => opt.Ignore());
        CreateMap<ILearningSpaceLayoutPe, LearningSpaceLayout>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        CreateMap<LearningSpaceLayout, LearningSpaceLayoutPe>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.LearningElements, opt => opt.Ignore())
            .ForMember(x => x.Capacity, opt => opt.Ignore())
            .AfterMap(MapSpaceLayoutElements)
            .ReverseMap()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.LearningElements, opt => opt.Ignore())
            .AfterMap(MapSpaceLayoutElements);
    }

    private void MapSpaceLayoutElements(LearningSpaceLayout source, LearningSpaceLayoutPe destination,
        ResolutionContext ctx)
    {
        var sourceNewElements = source.LearningElements
            .Where(x => !SameIdAtSameIndex(destination, x))
            .Select(tup =>
                new KeyValuePair<int, ILearningElementPe>(tup.Key,
                    ctx.Mapper.Map<LearningElementPe>(tup.Value)));
        foreach (var (key, _) in destination.LearningElements
                     .Where(x => !SameIdAtSameIndex(source, x)))
        {
            destination.LearningElements.Remove(key);
        }

        foreach (var (key, value) in destination.LearningElements)
        {
            var sourceElement = source.LearningElements
                .First(x => x.Key == key && x.Value.Id == value.Id);
            ctx.Mapper.Map(sourceElement.Value, value);
        }

        destination.LearningElements = sourceNewElements
            .Union(destination.LearningElements)
            .ToDictionary(tup => tup.Key, tup => tup.Value);
    }

    private void MapSpaceLayoutElements(LearningSpaceLayoutPe source, LearningSpaceLayout destination,
        ResolutionContext ctx)
    {
        var sourceNewElements = source.LearningElements
            .Where(x => !SameIdAtSameIndex(destination, x))
            .Select(tup =>
                new KeyValuePair<int, ILearningElement>(tup.Key,
                    ctx.Mapper.Map<LearningElement>(tup.Value)));
        foreach (var (key, _) in destination.LearningElements
                     .Where(x => !SameIdAtSameIndex(source, x)))
        {
            destination.LearningElements.Remove(key);
        }

        foreach (var (key, value) in destination.LearningElements)
        {
            var sourceElement = source.LearningElements
                .First(x => x.Key == key && x.Value.Id == value.Id);
            ctx.Mapper.Map(sourceElement.Value, value);
        }

        destination.LearningElements = sourceNewElements
            .Union(destination.LearningElements)
            .ToDictionary(tup => tup.Key, tup => tup.Value);
    }

    private static bool
        SameIdAtSameIndex(ILearningSpaceLayoutPe destination, KeyValuePair<int, ILearningElement> kvp) =>
        destination.LearningElements.Any(y => y.Key == kvp.Key && y.Value.Id == kvp.Value.Id);

    private static bool
        SameIdAtSameIndex(ILearningSpaceLayout destination, KeyValuePair<int, ILearningElementPe> kvp) =>
        destination.LearningElements.Any(y => y.Key == kvp.Key && y.Value.Id == kvp.Value.Id);

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
        CreateMap<FileContent, FileContentPe>()
            .ReverseMap();
        CreateMap<LinkContent, LinkContentPe>()
            .ReverseMap();
        CreateMap<FileContent, ILearningContentPe>()
            .As<FileContentPe>();
        CreateMap<LinkContent, ILearningContentPe>()
            .As<LinkContentPe>();
        CreateMap<FileContentPe, ILearningContent>()
            .As<FileContent>();
        CreateMap<LinkContentPe, ILearningContent>()
            .As<LinkContent>();
        CreateMap<ILearningContent, ILearningContentPe>()
            .IncludeAllDerived()
            .ReverseMap()
            .IncludeAllDerived();
    }

    private void CreateInterfaceMaps()
    {
        CreateMap<PathWayCondition, IObjectInPathWayPe>().As<PathWayConditionPe>();
        CreateMap<LearningSpace, IObjectInPathWayPe>().As<LearningSpacePe>();

        CreateMap<LearningElement, ILearningElementPe>().As<LearningElementPe>();
        CreateMap<LearningElementPe, ILearningElement>().As<LearningElement>();

        CreateMap<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap();

        CreateMap<LearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<LearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<ILearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
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
            .ForMember(x => x.AdvancedMode, opt =>opt.Ignore())
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
            .AfterMap((_, d) =>
            {
                foreach (var pathWay in d.LearningPathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.SourceObject.Id);
                    pathWay.TargetObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.TargetObject.Id);
                }
            })
            .AfterMap((_, d) =>
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
            .AfterMap((_, d) =>
            {
                foreach (var pathWay in d.LearningPathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject.Id);
                }
            })
            .AfterMap((_, d) =>
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