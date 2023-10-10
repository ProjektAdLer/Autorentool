using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLayout;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
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
    private ViewModelEntityMappingProfile()
    {
        DisableConstructorMapping();
        CreateWorkspaceMap();
        CreateLearningWorldMap();
        CreateLearningSpaceMap();
        CreateAdvancedLearningSpaceMap();
        CreateLearningElementMap();
        CreateLearningContentMap();
        CreatePathwayMaps();
        CreateInterfaceMaps();
        CreateLearningSpaceLayoutMap();
        CreateAdvancedLearningSpaceLayoutMap();
        CreateTopicMap();
        CreateAdaptivityMap();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ViewModelEntityMappingProfile());
        cfg.AddCollectionMappersOnce();
    };

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

    private void CreateAdvancedLearningSpaceLayoutMap()
    {
        CreateMap<IAdvancedLearningSpaceLayout, IAdvancedLearningSpaceLayoutViewModel>()
            .ForMember(x => x.UsedIndices, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.ContainedAdvancedLearningElementSlots, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());

        CreateMap<IAdvancedLearningSpaceLayoutViewModel, AdvancedLearningSpaceLayout>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        CreateMap<IAdvancedLearningSpaceLayout, AdvancedLearningSpaceLayoutViewModel>()
            .ForMember(x => x.UsedIndices, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.ContainedAdvancedLearningElementSlots, opt => opt.Ignore());

        CreateMap<AdvancedLearningSpaceLayout, IAdvancedLearningSpaceLayoutViewModel>()
            .As<AdvancedLearningSpaceLayoutViewModel>();
        CreateMap<AdvancedLearningSpaceLayoutViewModel, IAdvancedLearningSpaceLayout>()
            .As<AdvancedLearningSpaceLayout>();
        
        CreateMap<AdvancedLearningSpaceLayout, AdvancedLearningSpaceLayoutViewModel>()
            /// your config here!
            .ForMember(x => x.UsedIndices, opt => opt.Ignore()) 
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.ContainedAdvancedLearningElementSlots, opt => opt.Ignore())
            .IncludeBase<IAdvancedLearningSpaceLayout, IAdvancedLearningSpaceLayoutViewModel>()
            .ReverseMap()
            /// your config here! aber andersrum
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .IncludeBase<IAdvancedLearningSpaceLayoutViewModel, IAdvancedLearningSpaceLayout>()
            ;
    }

    private static void MapSpaceLayoutElements(ILearningSpaceLayout source, LearningSpaceLayoutViewModel destination,
        ResolutionContext ctx)
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
        CreateMap<LearningPathway, ILearningPathWayViewModel>()
            .As<LearningPathwayViewModel>();

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
            .ForMember(x => x.LearningContent, opt => opt.Ignore())
            .AfterMap(ElementContentAfterMap)
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore());
        CreateMap<ILearningElementViewModel, LearningElement>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .ForMember(x => x.LearningContent, opt => opt.DoNotUseDestinationValue())
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore());
    }

    /// <summary>
    /// We require this method because AutoMapper is stupid when mapping children when using the update syntax.
    /// It tries to forcibly cast whatever the content type is into the type currently in vm.LearningContent.
    /// If the type changed however, this will result in a InvalidCastException. We catch that exception here and
    /// instead just map the content into a new view model without the update syntax.
    /// </summary>
    private void ElementContentAfterMap(LearningElement entity, LearningElementViewModel vm, ResolutionContext context)
    {
        try
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (vm.LearningContent == null)
            {
                RemapWithoutUpdate();
                return;
            }

            context.Mapper.Map(entity.LearningContent, vm.LearningContent);
        }
        catch
        {
            RemapWithoutUpdate();
        }

        void RemapWithoutUpdate()
        {
            vm.LearningContent = context.Mapper.Map<ILearningContentViewModel>(entity.LearningContent);
        }
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
            .AfterMap((_, d) =>
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
            .AfterMap((_, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }
            });
        
        CreateMap<ILearningSpace, ILearningSpaceViewModel>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ReverseMap()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        
        CreateMap<LearningSpace, ILearningSpaceViewModel>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<LearningSpaceViewModel>();
        CreateMap<LearningSpaceViewModel, ILearningSpace>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<LearningSpace>();
        
        CreateMap<AdvancedLearningSpace, ILearningSpaceViewModel>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<AdvancedLearningSpaceViewModel>();
        CreateMap<AdvancedLearningSpaceViewModel, ILearningSpace>()
            .EqualityComparison((vm, intf) => vm.Id.Equals(intf.Id))
            .As<AdvancedLearningSpace>();
        
    }

    private void CreateAdvancedLearningSpaceMap()
    {
        CreateMap<AdvancedLearningSpace, AdvancedLearningSpaceViewModel>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
            .ForMember(x => x.ContainedAdvancedLearningElementSlots, opt => opt.Ignore())
            .ForMember(x => x.LearningSpaceLayout, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayViewModel>()
            .IncludeBase<ILearningSpace, ILearningSpaceViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((_, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }
            })
            .ReverseMap()
            .IncludeBase<IObjectInPathWayViewModel, IObjectInPathWay>()
            .IncludeBase<ILearningSpaceViewModel, ILearningSpace>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.LearningSpaceLayout, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }
            });
        CreateMap<IAdvancedLearningSpaceViewModel, IAdvancedLearningSpace>()
            .As<AdvancedLearningSpace>();
        CreateMap<IAdvancedLearningSpace, IAdvancedLearningSpaceViewModel>()
            .As<AdvancedLearningSpaceViewModel>();
        CreateMap<IAdvancedLearningSpaceViewModel, AdvancedLearningSpace>()
            .IncludeBase<IObjectInPathWayViewModel, IObjectInPathWay>()
            .IncludeBase<IAdvancedLearningSpaceViewModel, IAdvancedLearningSpace>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }
            });
        CreateMap<IAdvancedLearningSpace, AdvancedLearningSpaceViewModel>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.ContainedAdvancedLearningElementSlots, opt => opt.Ignore())
            .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayViewModel>()
            .IncludeBase<IAdvancedLearningSpace, IAdvancedLearningSpaceViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((_, d) =>
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
            .ForMember(x => x.AllLearningElements, opt => opt.Ignore())
            .ForMember(x => x.OnHoveredObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
            .ForMember(x => x.UnplacedLearningElements, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var pathWay in d.LearningPathWays)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject.Id);
                }
            })
            .AfterMap((_, d) =>
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
            .ForMember(x => x.AllLearningElements, opt => opt.Ignore())
            .ForMember(x => x.OnHoveredObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .ForMember(x => x.UnsavedChanges, opt => opt.Ignore())
            .ForMember(x => x.UnplacedLearningElements, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var pathWay in d.LearningPathWays)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject.Id);
                }
            })
            .AfterMap((_, d) =>
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
                foreach (var pathWayObject in d.ObjectsInPathWays)
                {
                    pathWayObject.InBoundObjects = d.LearningPathways.Where(x => x.TargetObject.Id == pathWayObject.Id)
                        .Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.LearningPathways.Where(x => x.SourceObject.Id == pathWayObject.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
    }

    private static void MapUnplacedElements(ILearningWorld source, LearningWorldViewModel destination,
        ResolutionContext ctx)
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

        foreach (var ele in destination.UnplacedLearningElements)
        {
            ele.Parent = null;
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

    private void CreateAdaptivityMap()
    {
        CreateAdaptivityTriggerMap();
        CreateAdaptivityActionMap();
        CreateAdaptivityQuestionMap();
        CreateChoiceMap();
        CreateAdaptivityRuleMap();
        CreateAdaptivityTaskMap();
        CreateAdaptivityContentMap();
    }

    private void CreateAdaptivityTriggerMap()
    {
        CreateMap<IAdaptivityTrigger, IAdaptivityTriggerViewModel>()
            .ReverseMap();

        CreateMap<CorrectnessTrigger, IAdaptivityTriggerViewModel>()
            .As<CorrectnessTriggerViewModel>();
        CreateMap<CorrectnessTriggerViewModel, IAdaptivityTrigger>()
            .As<CorrectnessTrigger>();

        CreateMap<TimeTrigger, IAdaptivityTriggerViewModel>()
            .As<TimeTriggerViewModel>();
        CreateMap<TimeTriggerViewModel, IAdaptivityTrigger>()
            .As<TimeTrigger>();

        CreateMap<CompositeTrigger, IAdaptivityTriggerViewModel>()
            .As<CompositeTriggerViewModel>();
        CreateMap<CompositeTriggerViewModel, IAdaptivityTrigger>()
            .As<CompositeTrigger>();

        CreateMap<CorrectnessTrigger, CorrectnessTriggerViewModel>()
            .IncludeBase<IAdaptivityTrigger, IAdaptivityTriggerViewModel>()
            .ReverseMap()
            .IncludeBase<IAdaptivityTriggerViewModel, IAdaptivityTrigger>();
        CreateMap<TimeTrigger, TimeTriggerViewModel>()
            .IncludeBase<IAdaptivityTrigger, IAdaptivityTriggerViewModel>()
            .ReverseMap()
            .IncludeBase<IAdaptivityTriggerViewModel, IAdaptivityTrigger>();
        CreateMap<CompositeTrigger, CompositeTriggerViewModel>()
            .ForMember(ctvm => ctvm.Left, opt => opt.Ignore())
            .AfterMap((ct, ctvm, ctx) => ctvm.Left = ctx.Mapper.Map<IAdaptivityTriggerViewModel>(ct.Left))
            .ForMember(ctvm => ctvm.Right, opt => opt.Ignore())
            .AfterMap((ct, ctvm, ctx) => ctvm.Right = ctx.Mapper.Map<IAdaptivityTriggerViewModel>(ct.Right))
            .IncludeBase<IAdaptivityTrigger, IAdaptivityTriggerViewModel>()
            .ReverseMap()
            .ForMember(ct => ct.Left, opt => opt.Ignore())
            .AfterMap((ctvm, ct, ctx) => ct.Left = ctx.Mapper.Map<IAdaptivityTrigger>(ctvm.Left))
            .ForMember(ct => ct.Right, opt => opt.Ignore())
            .AfterMap((ctvm, ct, ctx) => ct.Right = ctx.Mapper.Map<IAdaptivityTrigger>(ctvm.Right))
            .IncludeBase<IAdaptivityTriggerViewModel, IAdaptivityTrigger>();
    }

    private void CreateAdaptivityActionMap()
    {
        CreateMap<IAdaptivityAction, IAdaptivityActionViewModel>()
            .EqualityComparison((entity, vm) => entity.Id == vm.Id)
            .ReverseMap()
            .EqualityComparison((vm, entity) => vm.Id == entity.Id);

        CreateMap<CommentAction, IAdaptivityActionViewModel>()
            .As<CommentActionViewModel>();
        CreateMap<CommentActionViewModel, IAdaptivityAction>()
            .As<CommentAction>();

        CreateMap<ElementReferenceAction, IAdaptivityActionViewModel>()
            .As<ElementReferenceActionViewModel>();
        CreateMap<ElementReferenceActionViewModel, IAdaptivityAction>()
            .As<ElementReferenceAction>();

        CreateMap<ContentReferenceAction, IAdaptivityActionViewModel>()
            .As<ContentReferenceActionViewModel>();
        CreateMap<ContentReferenceActionViewModel, IAdaptivityAction>()
            .As<ContentReferenceAction>();

        CreateMap<CommentAction, CommentActionViewModel>()
            .IncludeBase<IAdaptivityAction, IAdaptivityActionViewModel>()
            .ReverseMap()
            .IncludeBase<IAdaptivityActionViewModel, IAdaptivityAction>();
        CreateMap<ElementReferenceAction, ElementReferenceActionViewModel>()
            .IncludeBase<IAdaptivityAction, IAdaptivityActionViewModel>()
            .ReverseMap()
            .IncludeBase<IAdaptivityActionViewModel, IAdaptivityAction>();
        CreateMap<ContentReferenceAction, ContentReferenceActionViewModel>()
            .ForMember(crvm => crvm.Content, opt => opt.Ignore())
            .AfterMap((cr, crvm, ctx) => crvm.Content = ctx.Mapper.Map<ILearningContentViewModel>(cr.Content))
            .IncludeBase<IAdaptivityAction, IAdaptivityActionViewModel>()
            .ReverseMap()
            .ForMember(cr => cr.Content, opt => opt.Ignore())
            .AfterMap((crvm, cr, ctx) => cr.Content = ctx.Mapper.Map<ILearningContent>(crvm.Content))
            .IncludeBase<IAdaptivityActionViewModel, IAdaptivityAction>();
    }

    private void CreateAdaptivityQuestionMap()
    {
        CreateMap<IAdaptivityQuestion, IAdaptivityQuestionViewModel>()
            .ReverseMap();
        CreateMap<IMultipleChoiceQuestion, IMultipleChoiceQuestion>()
            .ReverseMap();

        CreateMap<MultipleChoiceSingleResponseQuestion, IAdaptivityQuestionViewModel>()
            .As<MultipleChoiceSingleResponseQuestionViewModel>();
        CreateMap<MultipleChoiceSingleResponseQuestionViewModel, IAdaptivityQuestion>()
            .As<MultipleChoiceSingleResponseQuestion>();

        CreateMap<MultipleChoiceMultipleResponseQuestion, IAdaptivityQuestionViewModel>()
            .As<MultipleChoiceMultipleResponseQuestionViewModel>();
        CreateMap<MultipleChoiceMultipleResponseQuestionViewModel, IAdaptivityQuestion>()
            .As<MultipleChoiceMultipleResponseQuestion>();

        CreateMap<MultipleChoiceSingleResponseQuestion, MultipleChoiceSingleResponseQuestionViewModel>()
            .IncludeBase<IAdaptivityQuestion, IAdaptivityQuestionViewModel>()
            .ForMember(x => x.CorrectChoice, opt => opt.Ignore())
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((entity, vm, context) =>
                vm.CorrectChoice = vm.Choices.Single(choicevm => choicevm.Id == entity.CorrectChoice.Id))
            .ReverseMap()
            .ForMember(x => x.CorrectChoice, opt => opt.Ignore())
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((vm, entity, context) =>
                entity.CorrectChoice = entity.Choices.Single(choicevm => choicevm.Id == vm.CorrectChoice.Id))
            .IncludeBase<IAdaptivityQuestionViewModel, IAdaptivityQuestion>();
        CreateMap<MultipleChoiceMultipleResponseQuestion, MultipleChoiceMultipleResponseQuestionViewModel>()
            .IncludeBase<IAdaptivityQuestion, IAdaptivityQuestionViewModel>()
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((entity, vm, context) => vm.CorrectChoices = vm.Choices.Where(choicevm =>
                entity.CorrectChoices.Any(choiceentity => choiceentity.Id.Equals(choicevm.Id))).ToList())
            .ReverseMap()
            .IncludeBase<IAdaptivityQuestionViewModel, IAdaptivityQuestion>()
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((vm, entity, context) => entity.CorrectChoices = entity.Choices.Where(choiceentity =>
                vm.CorrectChoices.Any(choicevm => choicevm.Id.Equals(choicevm.Id))).ToList());
    }

    private void CreateChoiceMap()
    {
        CreateMap<Choice, ChoiceViewModel>()
            .ReverseMap();
    }

    private void CreateAdaptivityRuleMap()
    {
        CreateMap<IAdaptivityRule, IAdaptivityRuleViewModel>()
            .ReverseMap();

        CreateMap<AdaptivityRule, IAdaptivityRuleViewModel>()
            .As<AdaptivityRuleViewModel>();
        CreateMap<AdaptivityRuleViewModel, IAdaptivityRule>()
            .As<AdaptivityRule>();

        CreateMap<AdaptivityRule, AdaptivityRuleViewModel>()
            .IncludeBase<IAdaptivityRule, IAdaptivityRuleViewModel>()
            .ForMember(x => x.Action, cfg => cfg.Ignore())
            .AfterMap(
                (entity, vm, context) => vm.Action = context.Mapper.Map<IAdaptivityActionViewModel>(entity.Action))
            .ForMember(x => x.Trigger, cfg => cfg.Ignore())
            .AfterMap((entity, vm, context) =>
                vm.Trigger = context.Mapper.Map<IAdaptivityTriggerViewModel>(entity.Trigger))
            .ReverseMap()
            .IncludeBase<IAdaptivityRuleViewModel, IAdaptivityRule>();
    }

    private void CreateAdaptivityTaskMap()
    {
        CreateMap<IAdaptivityTask, IAdaptivityTaskViewModel>()
            .EqualityComparison((entity, vm) => entity.Id == vm.Id)
            .ReverseMap()
            .EqualityComparison((vm, entity) => entity.Id == vm.Id);

        CreateMap<AdaptivityTask, IAdaptivityTaskViewModel>()
            .As<AdaptivityTaskViewModel>();
        CreateMap<AdaptivityTaskViewModel, IAdaptivityTask>()
            .As<AdaptivityTask>();

        CreateMap<AdaptivityTask, AdaptivityTaskViewModel>()
            .IncludeBase<IAdaptivityTask, IAdaptivityTaskViewModel>()
            .ReverseMap()
            .IncludeBase<IAdaptivityTaskViewModel, IAdaptivityTask>();
    }

    private void CreateAdaptivityContentMap()
    {
        CreateMap<IAdaptivityContent, IAdaptivityContentViewModel>()
            .ReverseMap();

        CreateMap<AdaptivityContent, IAdaptivityContentViewModel>()
            .As<AdaptivityContentViewModel>();
        CreateMap<AdaptivityContentViewModel, IAdaptivityContent>()
            .As<AdaptivityContent>();

        CreateMap<AdaptivityContent, AdaptivityContentViewModel>()
            .IncludeBase<IAdaptivityContent, IAdaptivityContentViewModel>()
            .ReverseMap()
            .IncludeBase<IAdaptivityContentViewModel, IAdaptivityContent>();
    }
}