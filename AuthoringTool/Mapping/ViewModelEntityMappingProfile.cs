using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using PersistEntities;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Element.ActivationElement;
using Presentation.PresentationLogic.Element.InteractionElement;
using Presentation.PresentationLogic.Element.TestElement;
using Presentation.PresentationLogic.Element.TransferElement;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.Space.SpaceLayout;
using Presentation.PresentationLogic.World;

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
        CreateWorldMap();
        CreateSpaceMap();
        CreateElementMap();
        CreateContentMap();
        CreatePathwayMaps();
        CreateDerivedElementMaps();
        CreateInterfaceMaps();
        CreateSpaceLayoutMap();
    }

    private void CreateSpaceLayoutMap()
    {
        CreateMap<ISpaceLayout, SpaceLayoutViewModel>()
            .ForMember(x => x.FloorPlanViewModel, opt => opt.Ignore())
            .ForMember(x => x.UsedIndices, opt => opt.Ignore())
            .ForMember(x => x.ContainedElements, opt => opt.Ignore());
        CreateMap<ISpaceLayoutViewModel, SpaceLayout>()
            .ForMember(x => x.ContainedElements, opt => opt.Ignore());

        CreateMap<SpaceLayout, SpaceLayoutViewModel>()
            .IncludeBase<ISpaceLayout, SpaceLayoutViewModel>()
            .ReverseMap()
            .IncludeBase<ISpaceLayoutViewModel, SpaceLayout>();
    }

    private void CreateContentMap()
    {
        CreateMap<Content, ContentViewModel>().ReverseMap();
    }

    private void CreateInterfaceMaps()
    {
        //We must tell the automapper what class to use when it has to map from a class to an interface
        CreateMap<Element, IElementViewModel>()
            .As<ElementViewModel>();
        CreateMap<Space, ISpaceViewModel>()
            .As<SpaceViewModel>();
        CreateMap<Pathway, IPathWayViewModel>()
            .As<PathwayViewModel>();
        
        CreateMap<ElementViewModel, IElement>().As<Element>();
        CreateMap<ElementViewModel, Element>()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .IncludeAllDerived();

        CreateMap<Space, IObjectInPathWayViewModel>().As<SpaceViewModel>();

        CreateMap<H5PActivationElement, IElementViewModel>().As<H5PActivationElementViewModel>();
        CreateMap<H5PInteractionElement, IElementViewModel>().As<H5PInteractionElementViewModel>();
        CreateMap<H5PTestElement, IElementViewModel>().As<H5PTestElementViewModel>();
        CreateMap<ImageTransferElement, IElementViewModel>().As<ImageTransferElementViewModel>();
        CreateMap<PdfTransferElement, IElementViewModel>().As<PdfTransferElementViewModel>();
        CreateMap<VideoActivationElement, IElementViewModel>().As<VideoActivationElementViewModel>();
        CreateMap<VideoTransferElement, IElementViewModel>().As<VideoTransferElementViewModel>();
        CreateMap<TextTransferElement, IElementViewModel>().As<TextTransferElementViewModel>();

        CreateMap<H5PActivationElementViewModel, IElement>().As<H5PActivationElement>();
        CreateMap<H5PInteractionElementViewModel, IElement>().As<H5PInteractionElement>();
        CreateMap<H5PTestElementViewModel, IElement>().As<H5PTestElement>();
        CreateMap<ImageTransferElementViewModel, IElement>().As<ImageTransferElement>();
        CreateMap<PdfTransferElementViewModel, IElement>().As<PdfTransferElement>();
        CreateMap<VideoActivationElementViewModel, IElement>().As<VideoActivationElement>();
        CreateMap<VideoTransferElementViewModel, IElement>().As<VideoTransferElement>();
        CreateMap<TextTransferElementViewModel, IElement>().As<TextTransferElement>();

        CreateMap<ISelectableObjectInWorld, ISelectableObjectInWorldViewModel>()
            .ReverseMap();

        CreateMap<SpaceLayout, ISpaceLayoutViewModel>().As<SpaceLayoutViewModel>();
        CreateMap<SpaceLayoutViewModel, ISpaceLayout>().As<SpaceLayout>();
        CreateMap<ISpaceLayout, ISpaceLayoutViewModel>().As<SpaceLayoutViewModel>();
        CreateMap<ISpaceLayoutViewModel, ISpaceLayout>().As<SpaceLayout>();
    }

    private void CreateDerivedElementMaps()
    {
        //Element derived types
        CreateMap<H5PActivationElement, H5PActivationElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
        CreateMap<H5PInteractionElement, H5PInteractionElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
        CreateMap<H5PTestElement, H5PTestElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
        CreateMap<ImageTransferElement, ImageTransferElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
        CreateMap<PdfTransferElement, PdfTransferElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
        CreateMap<VideoActivationElement, VideoActivationElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
        CreateMap<VideoTransferElement, VideoTransferElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
        CreateMap<TextTransferElement, TextTransferElementViewModel>()
            .IncludeBase<Element, ElementViewModel>()
            .ReverseMap();
    }

    private void CreateElementMap()
    {
        CreateMap<Element, ElementViewModel>()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.Parent, opt => opt.Ignore());
    }

    private void CreateSpaceMap()
    {
        CreateMap<Space, SpaceViewModel>()
            .ForMember(x => x.SelectedElement, opt => opt.Ignore())
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedElements, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedElements)
                {
                    element.Parent = d;
                }

                d.SelectedElement =
                    d.ContainedElements.FirstOrDefault(x => x.Id == s.SelectedElement?.Id);
            })
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.SelectedElement, opt => opt.Ignore())
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.ContainedElements, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var element in d.ContainedElements)
                {
                    element.Parent = d;
                }

                d.SelectedElement =
                    d.ContainedElements.FirstOrDefault(x => x.Id == s.SelectedElement?.Id);
            });
    }

    private void CreatePathwayMaps()
    {
        CreateMap<PathwayViewModel, Pathway>()
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

    private void CreateWorldMap()
    {
        CreateMap<World, WorldViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.SelectedObject, opt => opt.Ignore())
            .ForMember(x => x.OnHoveredObjectInPathWay, opt => opt.Ignore())
            .ForMember(x => x.ShowingSpaceView, opt => opt.Ignore())
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedObject =
                    d.SelectableWorldObjects.FirstOrDefault(z => z.Id == s.SelectedObject?.Id);
            })
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.PathWays)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject?.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var pathWayObject in d.ObjectsInPathWays)
                {
                    pathWayObject.InBoundObjects = d.PathWays.Where(x => x.TargetObject.Id == pathWayObject.Id)
                        .Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.PathWays.Where(x => x.SourceObject.Id == pathWayObject.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            })
            .ReverseMap()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .ForMember(x => x.SelectableWorldObjects, opt => opt.Ignore())
            .ForMember(x => x.SelectedObject, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedObject =
                    d.SelectableWorldObjects.FirstOrDefault(z => z.Id == s.SelectedObject?.Id);
            })
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
                foreach (var pathWayObject in d.ObjectsInPathWays)
                {
                    pathWayObject.InBoundObjects = d.Pathways.Where(x => x.TargetObject.Id == pathWayObject.Id)
                        .Select(x => x.SourceObject).ToList();
                    pathWayObject.OutBoundObjects = d.Pathways.Where(x => x.SourceObject.Id == pathWayObject.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
    }

    private void CreateWorkspaceMap()
    {
        CreateMap<AuthoringToolWorkspace, AuthoringToolWorkspaceViewModel>()
            .ForMember(x => x.EditDialogInitialValues, opt => opt.Ignore())
            .ForMember(x => x.SelectedWorld, opt => opt.Ignore())
            .ForMember(x => x.WorldNames, opt => opt.Ignore())
            .ForMember(x => x.WorldShortNames, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedWorld = d.Worlds.FirstOrDefault(x => x.Id == s.SelectedWorld?.Id);
            })
            .ReverseMap()
            .ForMember(x => x.SelectedWorld, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedWorld = d.Worlds.FirstOrDefault(x => x.Id == s.SelectedWorld?.Id);
            });
    }
}