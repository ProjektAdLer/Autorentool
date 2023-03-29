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
    }

    private void CreateLearningSpaceLayoutMap()
    {
        CreateMap<ILearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .ForMember(x => x.FloorPlanViewModel, opt => opt.Ignore())
            .ForMember(x => x.UsedIndices, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.LearningElements, opt => opt.Ignore())
            .AfterMap(SpaceLayoutAfterMap);
        CreateMap<ILearningSpaceLayoutViewModel, LearningSpaceLayout>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());

        CreateMap<LearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .IncludeBase<ILearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .ReverseMap()
            .IncludeBase<ILearningSpaceLayoutViewModel, LearningSpaceLayout>();

    }

    private void SpaceLayoutAfterMap(ILearningSpaceLayout source, LearningSpaceLayoutViewModel destination, ResolutionContext ctx)
    {
        //gather view models for all elements that are in source but not in destination
        var sourceNewElementsViewModels = source.LearningElements
            .Where(x => !AnyLambda(destination, x))
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
            var entity = source.LearningElements.First(x => x.Key == key && x.Value.Id == value.Id);
            ctx.Mapper.Map(entity.Value, value);
        }

        destination.LearningElements = sourceNewElementsViewModels
            .Union(destination.LearningElements)
            .ToDictionary(tup => tup.Key, tup => tup.Value);
    }

    private static bool AnyLambda(ILearningSpaceLayoutViewModel destination, KeyValuePair<int, ILearningElement> x)
    {
        return destination.LearningElements.Any(y => y.Key == x.Key && y.Value.Id == x.Value.Id);
    }

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
    }

    private void CreateLearningSpaceMap()
    {
        CreateMap<LearningSpace, LearningSpaceViewModel>()
            .ForMember(x => x.SelectedLearningElement, opt => opt.Ignore())
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }

                d.SelectedLearningElement =
                    d.ContainedLearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
            })
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.SelectedLearningElement, opt => opt.Ignore())
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }

                d.SelectedLearningElement =
                    d.ContainedLearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
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
            .ForMember(x => x.SelectedLearningObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningElement, opt => opt.Ignore())
            .ForMember(x => x.OnHoveredObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.ShowingLearningSpaceView, opt => opt.Ignore())
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningObjectInPathWay =
                    d.SelectableWorldObjects.FirstOrDefault(z => z.Id == s.SelectedLearningObjectInPathWay?.Id);
            })
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
            .AfterMap((s, d) =>
            {
                d.SelectedLearningElement = d.UnplacedLearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
            })
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningElement, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningObjectInPathWay =
                    d.SelectableWorldObjects.FirstOrDefault(z => z.Id == s.SelectedLearningObjectInPathWay?.Id);
            })
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
            })
            .AfterMap((s, d) =>
            {
                d.SelectedLearningElement = d.UnplacedLearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
            });
    }

    private void CreateWorkspaceMap()
    {
        CreateMap<AuthoringToolWorkspace, AuthoringToolWorkspaceViewModel>()
            .ForMember(x => x.EditDialogInitialValues, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningWorld, opt => opt.Ignore())
            .ForMember(x => x.WorldNames, opt => opt.Ignore())
            .ForMember(x => x.WorldShortnames, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Id == s.SelectedLearningWorld?.Id);
            })
            .ReverseMap()
            .ForMember(x => x.SelectedLearningWorld, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Id == s.SelectedLearningWorld?.Id);
            });
    }
}