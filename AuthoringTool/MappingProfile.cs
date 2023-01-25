using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using PersistEntities;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningElement.ActivationElement;
using Presentation.PresentationLogic.LearningElement.InteractionElement;
using Presentation.PresentationLogic.LearningElement.TestElement;
using Presentation.PresentationLogic.LearningElement.TransferElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace AuthoringTool;

public class MappingProfile : Profile
{
    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new MappingProfile());
        cfg.AddCollectionMappers();
    };
    
    private MappingProfile()
    {
        DisableConstructorMapping();
        CreateViewModelEntityMaps();
        CreatePersistEntityMaps();
    }

    /// <summary>
    /// Configures mappings between ViewModels and Entity classes.
    /// </summary>
    private void CreateViewModelEntityMaps()
    {
        CreateMap<AuthoringToolWorkspace, AuthoringToolWorkspaceViewModel>()
            .ForMember(x => x.EditDialogInitialValues, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningWorld, opt => opt.Ignore())
            .ForMember(x => x.WorldNames, opt => opt.Ignore())
            .ForMember(x => x.WorldShortNames, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Id == s.SelectedLearningWorld?.Id);
            }).ReverseMap()
            .ForMember(x => x.SelectedLearningWorld, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Id == s.SelectedLearningWorld?.Id);
            });
        CreateMap<LearningWorld, LearningWorldViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.SelectedLearningObject, opt => opt.Ignore())
            .ForMember(x => x.OnHoveredObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.ShowingLearningSpaceView, opt => opt.Ignore())
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningObject = d.SelectableWorldObjects.FirstOrDefault(z => z.Id == s.SelectedLearningObject?.Id);
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
                    pathWayObject.InBoundObjects = d.LearningPathWays.Where(x => x.TargetObject.Id == pathWayObject.Id).Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.LearningPathWays.Where(x => x.SourceObject.Id == pathWayObject.Id).Select(x => x.TargetObject).ToList();
                }
            })
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningObject, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningObject = d.SelectableWorldObjects.FirstOrDefault(z => z.Id == s.SelectedLearningObject?.Id);
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
                    pathWayObject.InBoundObjects = d.LearningPathways.Where(x => x.TargetObject.Id == pathWayObject.Id).Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.LearningPathways.Where(x => x.SourceObject.Id == pathWayObject.Id).Select(x => x.TargetObject).ToList();
                }
            });

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

        CreateMap<IObjectInPathWay, IObjectInPathWayViewModel>()
            .IncludeBase<ISelectableObjectInWorld, ISelectableObjectInWorldViewModel>()
            .ReverseMap();
        CreateMap<ISelectableObjectInWorld, ISelectableObjectInWorldViewModel>()
            .ReverseMap();

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

                d.SelectedLearningElement = d.ContainedLearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
            })
            .ReverseMap()
            .ForMember(x => x.SelectedLearningElement, opt => opt.Ignore())
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }

                d.SelectedLearningElement = d.ContainedLearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
            });

        CreateMap<ILearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .ForMember(x => x.FloorPlanViewModel, opt => opt.Ignore())
            .ForMember(x => x.UsedIndices, opt => opt.Ignore())
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        CreateMap<ILearningSpaceLayoutViewModel, LearningSpaceLayout>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());

        CreateMap<LearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .IncludeBase<ILearningSpaceLayout, LearningSpaceLayoutViewModel>()
            .ReverseMap()
            .IncludeBase<ILearningSpaceLayoutViewModel, LearningSpaceLayout>();

        CreateMap<LearningElement, LearningElementViewModel>()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore());
        CreateMap<LearningContent, LearningContentViewModel>().ReverseMap();

        //Element derived types
        CreateMap<H5PActivationElement, H5PActivationElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();
        CreateMap<H5PInteractionElement, H5PInteractionElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();
        CreateMap<H5PTestElement, H5PTestElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();
        CreateMap<ImageTransferElement, ImageTransferElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();
        CreateMap<PdfTransferElement, PdfTransferElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();
        CreateMap<VideoActivationElement, VideoActivationElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();
        CreateMap<VideoTransferElement, VideoTransferElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();
        CreateMap<TextTransferElement, TextTransferElementViewModel>()
            .IncludeBase<LearningElement, LearningElementViewModel>()
            .ReverseMap();

        //We must tell the automapper what class to use when it has to map from a class to an interface
        CreateMap<LearningElement, ILearningElementViewModel>().As<LearningElementViewModel>();
        CreateMap<LearningElementViewModel, ILearningElement>().As<LearningElement>();
        CreateMap<LearningSpace, ILearningSpaceViewModel>()
            .As<LearningSpaceViewModel>();
        CreateMap<LearningPathway, ILearningPathWayViewModel>()
            .As<LearningPathwayViewModel>();
        
        
        CreateMap<PathWayCondition, IObjectInPathWayViewModel>().As<PathWayConditionViewModel>();
        CreateMap<LearningSpace, IObjectInPathWayViewModel>().As<LearningSpaceViewModel>();
        
        CreateMap<LearningSpaceLayout, ILearningSpaceLayoutViewModel>().As<LearningSpaceLayoutViewModel>();
        CreateMap<LearningSpaceLayoutViewModel, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, ILearningSpaceLayoutViewModel>().As<LearningSpaceLayoutViewModel>();
        CreateMap<ILearningSpaceLayoutViewModel, ILearningSpaceLayout>().As<LearningSpaceLayout>();

        
        CreateMap<H5PActivationElement, ILearningElementViewModel>().As<H5PActivationElementViewModel>();
        CreateMap<H5PInteractionElement, ILearningElementViewModel>().As<H5PInteractionElementViewModel>();
        CreateMap<H5PTestElement, ILearningElementViewModel>().As<H5PTestElementViewModel>();
        CreateMap<ImageTransferElement, ILearningElementViewModel>().As<ImageTransferElementViewModel>();
        CreateMap<PdfTransferElement, ILearningElementViewModel>().As<PdfTransferElementViewModel>();
        CreateMap<VideoActivationElement, ILearningElementViewModel>().As<VideoActivationElementViewModel>();
        CreateMap<VideoTransferElement, ILearningElementViewModel>().As<VideoTransferElementViewModel>();
        CreateMap<TextTransferElement, ILearningElementViewModel>().As<TextTransferElementViewModel>();

        CreateMap<H5PActivationElementViewModel, ILearningElement>().As<H5PActivationElement>();
        CreateMap<H5PInteractionElementViewModel, ILearningElement>().As<H5PInteractionElement>();
        CreateMap<H5PTestElementViewModel, ILearningElement>().As<H5PTestElement>();
        CreateMap<ImageTransferElementViewModel, ILearningElement>().As<ImageTransferElement>();
        CreateMap<PdfTransferElementViewModel, ILearningElement>().As<PdfTransferElement>();
        CreateMap<VideoActivationElementViewModel, ILearningElement>().As<VideoActivationElement>();
        CreateMap<VideoTransferElementViewModel, ILearningElement>().As<VideoTransferElement>();
        CreateMap<TextTransferElementViewModel, ILearningElement>().As<TextTransferElement>();
    }

    /// <summary>
    /// Configures mappings between Entity and PersistEntity classes
    /// </summary>
    private void CreatePersistEntityMaps()
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
                    objectInPathWayPe.InBoundObjects = d.LearningPathways.Where(x => x.TargetObject.Id == objectInPathWayPe.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWayPe.OutBoundObjects = d.LearningPathways.Where(x => x.SourceObject.Id == objectInPathWayPe.Id)
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
                    objectInPathWay.InBoundObjects = d.LearningPathways.Where(x => x.TargetObject.Id == objectInPathWay.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWay.OutBoundObjects = d.LearningPathways.Where(x => x.SourceObject.Id == objectInPathWay.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
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
        CreateMap<LearningSpaceLayout, LearningSpaceLayoutPe>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        
        CreateMap<LearningPathway, LearningPathwayPe>()
            .ReverseMap();
        CreateMap<PathWayCondition, PathWayConditionPe>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore());
        CreateMap<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap();

        CreateMap<LearningElement, LearningElementPe>();
        CreateMap<LearningElementPe, LearningElement>()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<LearningContent, LearningContentPe>()
            .ReverseMap();
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

        CreateMap<LearningElementDifficultyEnum, LearningElementDifficultyEnumPe>()
            .ReverseMap();
        
        CreateMap<ConditionEnum, ConditionEnumPe>()
            .ReverseMap();
        
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
        
        CreateMap<LearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<LearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<ILearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, LearningSpaceLayoutPe>().ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        CreateMap<ILearningSpaceLayoutPe, LearningSpaceLayout>().ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
    }
}