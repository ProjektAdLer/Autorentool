using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
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

namespace AuthoringTool.Mapping;

/// <summary>
/// Configures mappings between ViewModels and Entity classes.
/// </summary>
public class ViewModelEntityMappingProfile : Profile
{
    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ViewModelEntityMappingProfile());
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
        CreateDerivedElementMaps();
        CreateInterfaceMaps();
        CreateLearningSpaceLayoutMap();
    }

    private void CreateLearningSpaceLayoutMap()
    {
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

        CreateMap<ISelectableObjectInWorld, ISelectableObjectInWorldViewModel>()
            .ReverseMap();

        CreateMap<LearningSpaceLayout, ILearningSpaceLayoutViewModel>().As<LearningSpaceLayoutViewModel>();
        CreateMap<LearningSpaceLayoutViewModel, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, ILearningSpaceLayoutViewModel>().As<LearningSpaceLayoutViewModel>();
        CreateMap<ILearningSpaceLayoutViewModel, ILearningSpaceLayout>().As<LearningSpaceLayout>();
    }

    private void CreateDerivedElementMaps()
    {
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