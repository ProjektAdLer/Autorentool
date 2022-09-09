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
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Name == s.SelectedLearningWorld.Name);
            }).ReverseMap()
            .ForMember(x=>x.SelectedLearningWorld, opt=>opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.SelectedLearningWorld = d.LearningWorlds.FirstOrDefault(x => x.Name == s.SelectedLearningWorld.Name);
            });
        CreateMap<LearningWorld, LearningWorldViewModel>()
            .ForMember(x => x.LearningObjects, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningObject, opt => opt.Ignore())
            .ForMember(x => x.ShowingLearningSpaceView, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }
            })
            .ReverseMap()
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }
            });

        CreateMap<LearningSpace, LearningSpaceViewModel>()
            .ForMember(x => x.SelectedLearningObject, opt => opt.Ignore())
            .EqualityComparison((x, y) => x.Id == y.Id)
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }
            })
            .ReverseMap()
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }
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

        //We must tell the automapper what class to use when it has to map from a class to an interface
        CreateMap<LearningElement, ILearningElementViewModel>()
            .As<LearningElementViewModel>();
        CreateMap<LearningSpace, ILearningSpaceViewModel>()
            .As<LearningSpaceViewModel>();

        //Same deal here as above
        CreateMap<ILearningElementParent, ILearningElementViewModelParent>()
            .ReverseMap();
        CreateMap<LearningWorld, ILearningElementViewModelParent>()
            .IncludeBase<ILearningElementParent, ILearningElementViewModelParent>()
            .As<LearningWorldViewModel>();
        CreateMap<LearningSpace, ILearningElementViewModelParent>()
            .IncludeBase<ILearningElementParent, ILearningElementViewModelParent>()
            .As<LearningSpaceViewModel>();
        CreateMap<LearningWorldViewModel, ILearningElementParent>()
            .IncludeBase<ILearningElementViewModelParent, ILearningElementParent>()
            .As<LearningWorld>();
        CreateMap<LearningSpaceViewModel, ILearningElementParent>()
            .IncludeBase<ILearningElementViewModelParent, ILearningElementParent>()
            .As<LearningSpace>();
    }

    /// <summary>
    /// Configures mappings between Entity and PersistEntity classes
    /// </summary>
    private void CreatePersistEntityMaps()
    {
        CreateMap<LearningWorld, LearningWorldPe>()
            .ReverseMap()
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }
            });
        CreateMap<LearningSpace, LearningSpacePe>()
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                foreach (var element in d.LearningElements)
                {
                    element.Parent = d;
                }
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

        CreateMap<LearningElementDifficultyEnum, LearningElementDifficultyEnumPe>()
            .ReverseMap();
    }
}