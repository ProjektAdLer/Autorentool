using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using PersistEntities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningElement.ActivationElement;
using Presentation.PresentationLogic.LearningElement.InteractionElement;
using Presentation.PresentationLogic.LearningElement.TestElement;
using Presentation.PresentationLogic.LearningElement.TransferElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
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
            .ForMember(x=>x.EditDialogInitialValues, opt=>opt.Ignore())
            .ForMember(x=>x.SelectedLearningWorld, opt=>opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Id == s.SelectedLearningWorld?.Id);
            }).ReverseMap()
            .ForMember(x=>x.SelectedLearningWorld, opt=>opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Id == s.SelectedLearningWorld?.Id);
            });
        CreateMap<LearningWorld, LearningWorldViewModel>()
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ForMember(x => x.SelectedLearningSpace, opt => opt.Ignore())
            .ForMember(x => x.OnHoveredLearningSpace, opt => opt.Ignore())
            .ForMember(x => x.ShowingLearningSpaceView, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningSpace = d.LearningSpaces.FirstOrDefault(z => z.Id == s.SelectedLearningSpace?.Id);
            })
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.LearningPathWays)
                {
                    pathWay.SourceSpace = d.LearningSpaces.First(x => x.Id == pathWay.SourceSpace?.Id);
                    pathWay.TargetSpace = d.LearningSpaces.First(x => x.Id == pathWay.TargetSpace?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var learningSpace in d.LearningSpaces)
                {
                    learningSpace.InBoundSpaces = d.LearningPathWays.Where(x => x.TargetSpace.Id == learningSpace.Id).Select(x => x.SourceSpace).ToList();
                    learningSpace.OutBoundSpaces = d.LearningPathWays.Where(x => x.SourceSpace.Id == learningSpace.Id).Select(x => x.TargetSpace).ToList();
                }
            })
            .ReverseMap()
            .ForMember(x => x.SelectedLearningSpace, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningSpace = d.LearningSpaces.FirstOrDefault(z => z.Id == s.SelectedLearningSpace?.Id);
            })
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.LearningPathways)
                {
                    pathWay.SourceSpace = d.LearningSpaces.First(x => x.Id == pathWay.SourceSpace?.Id);
                    pathWay.TargetSpace = d.LearningSpaces.First(x => x.Id == pathWay.TargetSpace?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var learningSpace in d.LearningSpaces)
                {
                    learningSpace.InBoundSpaces = d.LearningPathways.Where(x => x.TargetSpace.Id == learningSpace.Id).Select(x => x.SourceSpace).ToList();
                    learningSpace.OutBoundSpaces = d.LearningPathways.Where(x => x.SourceSpace.Id == learningSpace.Id).Select(x => x.TargetSpace).ToList();
                }
            });

        CreateMap<LearningPathway, LearningPathwayViewModel>()
            .ReverseMap();
            
        CreateMap<LearningSpace, LearningSpaceViewModel>()
            .ForMember(x => x.SelectedLearningElement, opt => opt.Ignore())
            .ForMember(x => x.InBoundSpaces, opt => opt.Ignore())
            .ForMember(x => x.OutBoundSpaces, opt => opt.Ignore())
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }

                d.SelectedLearningElement = d.LearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
            })
            .ReverseMap()
            .ForMember(x => x.SelectedLearningElement, opt => opt.Ignore())
            .ForMember(x => x.InBoundSpaces, opt => opt.Ignore())
            .ForMember(x => x.OutBoundSpaces, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }

                d.SelectedLearningElement = d.LearningElements.FirstOrDefault(x => x.Id == s.SelectedLearningElement?.Id);
            });

        CreateMap<LearningElement, LearningElementViewModel>()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .EqualityComparison((x, y) => x.Id == y.Id)
            .ReverseMap()
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
        CreateMap<LearningElement, ILearningElementViewModel>()
            .As<LearningElementViewModel>();
        CreateMap<LearningSpace, ILearningSpaceViewModel>()
            .As<LearningSpaceViewModel>();
        CreateMap<LearningPathway, ILearningPathWayViewModel>()
            .As<LearningPathwayViewModel>();
    }

    /// <summary>
    /// Configures mappings between Entity and PersistEntity classes
    /// </summary>
    private void CreatePersistEntityMaps()
    {
        CreateMap<LearningWorld, LearningWorldPe>()
            .AfterMap((s, d) =>
            {
                foreach (var pathWay in d.LearningPathWays)
                {
                    pathWay.SourceSpace = d.LearningSpaces.First(x => x.Id == pathWay.SourceSpace?.Id);
                    pathWay.TargetSpace = d.LearningSpaces.First(x => x.Id == pathWay.TargetSpace?.Id);
                }
            })
            .AfterMap((s, d) =>
            {
                foreach (var learningSpace in d.LearningSpaces)
                {
                    learningSpace.InBoundSpaces = d.LearningPathWays.Where(x => x.TargetSpace.Id == learningSpace.Id)
                        .Select(x => x.SourceSpace).ToList();
                    learningSpace.OutBoundSpaces = d.LearningPathWays.Where(x => x.SourceSpace.Id == learningSpace.Id)
                        .Select(x => x.TargetSpace).ToList();
                }
            })
            .ReverseMap();
        CreateMap<LearningSpace, LearningSpacePe>()
            .ForMember(x => x.InBoundSpaces, opt => opt.Ignore())
            .ForMember(x => x.OutBoundSpaces, opt => opt.Ignore())
            .ReverseMap()
            .AfterMap((_, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }
            });
        CreateMap<LearningPathway, LearningPathwayPe>()
            .ReverseMap()
            .AfterMap((s, d) =>
            {
                s.SourceSpace.Id = new Guid();
                s.TargetSpace.Id = new Guid();
            });
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
    }
}