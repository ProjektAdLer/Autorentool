using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
namespace AuthoringTool.Mapping;

/// <summary>
/// Configures mappings between ViewModels and Entity classes.
/// </summary>
public class ViewModelEntityMappingProfile : Profile
{
    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ViewModelEntityMappingProfile());
        cfg.AddCollectionMappersOnce();
    };

    private ViewModelEntityMappingProfile()
    {
        DisableConstructorMapping();
        CreateWorkspaceMap();
        CreateLearningWorldMap();
        CreateLearningSpaceMap();
        CreateLearningElementMap();
        CreateLearningContentMap();
        CreatePathwayMaps();
        CreateInterfaceMaps();
        CreateLearningSpaceLayoutMap();
        CreateTopicMap();
    }

    private void CreateTopicMap()
    {
        CreateMap<Topic, TopicViewModel>()
            .EqualityComparison((entity, vm) => entity.Id == vm.Id)
            .ReverseMap();
    }

    private void CreateLearningSpaceLayoutMap()
    {
        CreateMap<ILearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .ForMember(x => x.FloorPlanViewModel, opt => opt.Ignore())
            .ForMember(x => x.UsedIndices, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.LearningElements, opt => opt.Ignore())
            .AfterMap(MapSpaceLayoutElements);
        CreateMap<ILearningSpaceLayoutViewModel, LearningSpaceLayout>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());

        CreateMap<LearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .IncludeBase<ILearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .ReverseMap()
            .IncludeBase<ILearningSpaceLayoutViewModel, LearningSpaceLayout>();

    }

    private static void MapSpaceLayoutElements(ILearningSpaceLayout source, LearningSpaceLayoutViewModel destination, ResolutionContext ctx)
    {
        //gather view models for all elements that are in source but not in destination
        var sourceNewElementsViewModels = source.LearningElements
            .Where(x => !SameIdAtSameIndex(destination, x))
            .Select(tup =>
                new KeyValuePair<int, ILearningElementViewModel>(tup.Key,
                    ctx.Mapper.Map<LearningElementViewModel>(tup.Value)));
        
        //remove all elements from destination that are not in source
        foreach (var (key, _) in destination.LearningElements.Where(x =>
                     !source.LearningElements.Any(y => y.Key == x.Key && y.Value.Id == x.Value.Id)))
        {
            destination.LearningElements.Remove(key);
        }
        
        //map all elements that are in source and destination already into the respective destination element
        foreach (var (key, value) in destination.LearningElements)
        {
            var entity = source.LearningElements
                .First(x => x.Key == key && x.Value.Id == value.Id);
            ctx.Mapper.Map(entity.Value, value);
        }

        destination.LearningElements = sourceNewElementsViewModels
            .Union(destination.LearningElements)
            .ToDictionary(tup => tup.Key, tup => tup.Value);
    }

    private static bool SameIdAtSameIndex(ILearningSpaceLayoutViewModel destination,
        KeyValuePair<int, ILearningElement> kvp) =>
        destination.LearningElements.Any(y => y.Key == kvp.Key && y.Value.Id == kvp.Value.Id);

    private void CreateLearningContentMap()
    {
        CreateMap<FileContent, FileContentViewModel>()
            .ReverseMap();
        CreateMap<LinkContent, LinkContentViewModel>()
            .ReverseMap();
        CreateMap<FileContent, ILearningContentViewModel>()
            .As<FileContentViewModel>();
        CreateMap<LinkContent, ILearningContentViewModel>()
            .As<LinkContentViewModel>();
        CreateMap<FileContentViewModel, ILearningContent>()
            .As<FileContent>();
        CreateMap<LinkContentViewModel, ILearningContent>()
            .As<LinkContent>();
        CreateMap<ILearningContent, ILearningContentViewModel>()
            .IncludeAllDerived()
            .ReverseMap()
            .IncludeAllDerived();
    }

    private void CreateInterfaceMaps()
    {
        //We must tell the automapper what class to use when it has to map from a class to an interface
        CreateMap<LearningElement, ILearningElementViewModel>()
            .As<LearningElementViewModel>();
        CreateMap<LearningSpace, ILearningSpaceViewModel>()
            .As<LearningSpaceViewModel>();
        CreateMap<LearningPathway, ILearningPathWayViewModel>()
            .As<LearningPathwayViewModel>();
        
        CreateMap<LearningSpaceViewModel, ILearningSpace>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<LearningSpace>();
        CreateMap<ILearningElementViewModel, ILearningElement>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<LearningElement>();
        CreateMap<ILearningSpaceViewModel, ILearningSpace>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<LearningSpace>();
        CreateMap<ILearningSpace, ILearningSpaceViewModel>()
            .As<LearningSpaceViewModel>();
        
        CreateMap<LearningWorld, ILearningWorldViewModel>()
            .EqualityComparison((e, intf) => e.Id.Equals(intf.Id))
            .As<LearningWorldViewModel>();
        CreateMap<LearningWorldViewModel, ILearningWorld>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<LearningWorld>();
        CreateMap<ILearningWorld, ILearningWorldViewModel>()
            .As<LearningWorldViewModel>();
        CreateMap<ILearningWorldViewModel, ILearningWorld>()
            .As<LearningWorld>();
        
        CreateMap<LearningElementViewModel, ILearningElement>().As<LearningElement>();
        CreateMap<LearningElementViewModel, LearningElement>()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .IncludeAllDerived();

        CreateMap<LearningSpace, IObjectInPathWayViewModel>().As<LearningSpaceViewModel>();

        CreateMap<ISelectableObjectInWorld, ISelectableObjectInWorldViewModel>()
            .ReverseMap();

        CreateMap<LearningSpaceLayout, ILearningSpaceLayoutViewModel>().As<LearningSpaceLayoutViewModel>();
        CreateMap<LearningSpaceLayoutViewModel, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, ILearningSpaceLayoutViewModel>().As<LearningSpaceLayoutViewModel>();
        CreateMap<ILearningSpaceLayoutViewModel, ILearningSpaceLayout>().As<LearningSpaceLayout>();
    }

    private void CreateLearningElementMap()
    {
        CreateMap<LearningElement, LearningElementViewModel>()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore());
        CreateMap<ILearningElementViewModel, LearningElement>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore());
    }

    private void CreateLearningSpaceMap()
    {
        CreateMap<LearningSpace, LearningSpaceViewModel>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayViewModel>()
            .IncludeBase<ILearningSpace, ILearningSpaceViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }
            })
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }

            });
       CreateMap<ILearningSpaceViewModel, LearningSpace>() 
            .IncludeBase<IObjectInPathWayViewModel, IObjectInPathWay>()
            .IncludeBase<ILearningSpaceViewModel, ILearningSpace>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }

            });
       CreateMap<ILearningSpace, LearningSpaceViewModel>()
           .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
           .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
           .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
           .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
           .IncludeBase<IObjectInPathWay, IObjectInPathWayViewModel>()
           .IncludeBase<ILearningSpace, ILearningSpaceViewModel>()
           .EqualityComparison((x, y) => x.Id == y.Id)
           .AfterMap((s, d) =>
           {
               foreach (var element in d.ContainedLearningElements)
               {
                   element.Parent = d;
               }
           });
    }

    private void CreatePathwayMaps()
    {
        CreateMap<LearningPathwayViewModel, LearningPathway>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ReverseMap();

        CreateMap<PathWayCondition, PathWayConditionViewModel>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore());

        CreateMap<PathWayCondition, IObjectInPathWayViewModel>().As<PathWayConditionViewModel>();

        CreateMap<IObjectInPathWay, IObjectInPathWayViewModel>()
            .IncludeBase<ISelectableObjectInWorld, ISelectableObjectInWorldViewModel>()
            .ReverseMap();
    }

    private void CreateLearningWorldMap()
    {
        CreateMap<LearningWorld, LearningWorldViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.OnHoveredObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.ShowingLearningSpaceView, opt => opt.Ignore())
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
            .ForMember(x => x.UnplacedLearningElements, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.LearningPathWays)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject?.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var pathWayObject in d.ObjectsInPathWays)
                {
                    pathWayObject.InBoundObjects = d.LearningPathWays.Where(x => x.TargetObject.Id == pathWayObject.Id)
                        .Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.LearningPathWays.Where(x => x.SourceObject.Id == pathWayObject.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            })
            .AfterMap(MapUnplacedElements)
            .IncludeBase<ILearningWorld, ILearningWorldViewModel>()
            .ReverseMap()
            .IncludeBase<ILearningWorldViewModel, ILearningWorld>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
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
                foreach (var pathWayObject in d.ObjectsInPathWays)
                {
                    pathWayObject.InBoundObjects = d.LearningPathways.Where(x => x.TargetObject.Id == pathWayObject.Id)
                        .Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.LearningPathways.Where(x => x.SourceObject.Id == pathWayObject.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
        
        CreateMap<ILearningWorld, LearningWorldViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.OnHoveredObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.ShowingLearningSpaceView, opt => opt.Ignore())
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
            .ForMember(x => x.UnplacedLearningElements, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.LearningPathWays)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject?.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var pathWayObject in d.ObjectsInPathWays)
                {
                    pathWayObject.InBoundObjects = d.LearningPathWays.Where(x => x.TargetObject.Id == pathWayObject.Id)
                        .Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.LearningPathWays.Where(x => x.SourceObject.Id == pathWayObject.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            })
            .AfterMap(MapUnplacedElements);

        CreateMap<ILearningWorldViewModel, LearningWorld>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
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
                foreach (var pathWayObject in d.ObjectsInPathWays)
                {
                    pathWayObject.InBoundObjects = d.LearningPathways.Where(x => x.TargetObject.Id == pathWayObject.Id)
                        .Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.LearningPathways.Where(x => x.SourceObject.Id == pathWayObject.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
    }
    
    private static void MapUnplacedElements(ILearningWorld source, LearningWorldViewModel destination, ResolutionContext ctx)
    {
        //gather view models for all elements that are in source but not in destination
        var sourceNewElementsViewModels = source.UnplacedLearningElements.ToList()
            .FindAll(x => destination.UnplacedLearningElements.All(v => v.Id != x.Id))
            .Select(ele => ctx.Mapper.Map<LearningElementViewModel>(ele));
        
        //remove all elements from destination that are not in source
        foreach (var ele in destination.UnplacedLearningElements.ToList().Where(x => 
                     source.UnplacedLearningElements.All(y => y.Id != x.Id)))
        {
            destination.UnplacedLearningElements.Remove(ele);
        }
        
        //map all elements that are in source and destination already into the respective destination element
        foreach (var ele in destination.UnplacedLearningElements)
        {
            var entity = source.UnplacedLearningElements.First(x => x.Id == ele.Id);
            ctx.Mapper.Map(entity, ele);
        }

        foreach (var ele in sourceNewElementsViewModels)
        {
            destination.UnplacedLearningElements.Add(ele);
        }
    }

    private void CreateWorkspaceMap()
    {
        CreateMap<AuthoringToolWorkspace, AuthoringToolWorkspaceViewModel>()
            .ForMember(x => x.EditDialogInitialValues, opt => opt.Ignore())
            .ForMember(x => x.WorldNames, opt => opt.Ignore())
            .ForMember(x => x.WorldShortnames, opt => opt.Ignore())
            .ReverseMap();
    }
}