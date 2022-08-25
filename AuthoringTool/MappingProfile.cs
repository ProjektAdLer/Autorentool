using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningElement.ActivationElement;
using AuthoringTool.PresentationLogic.LearningElement.InteractionElement;
using AuthoringTool.PresentationLogic.LearningElement.TestElement;
using AuthoringTool.PresentationLogic.LearningElement.TransferElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AutoMapper;

namespace AuthoringTool;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateViewModelEntityMaps();
        CreatePersistEntityMaps();
    }

    private void CreateViewModelEntityMaps()
    {
        CreateMap<AuthoringToolWorkspaceViewModel, AuthoringToolWorkspace>().ReverseMap();
        CreateMap<LearningWorld, LearningWorldViewModel>()
            .ForMember(x => x.LearningObjects, opt => opt.Ignore())
            .ForMember(x => x.SelectedLearningObject, opt => opt.Ignore())
            .ForMember(x => x.ShowingLearningSpaceView, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<LearningSpace, LearningSpaceViewModel>()
            .ForMember(x => x.SelectedLearningObject, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<LearningElement, LearningElementViewModel>().ReverseMap();
        CreateMap<LearningContent, LearningContentViewModel>().ReverseMap();
        
        CreateMap<H5PActivationElement, H5PActivationElementViewModel>().IncludeBase<LearningElement, LearningElementViewModel>().ReverseMap();
        CreateMap<H5PInteractionElement, H5PInteractionElementViewModel>().IncludeBase<LearningElement, LearningElementViewModel>().ReverseMap();
        CreateMap<H5PTestElement, H5PTestElementViewModel>().IncludeBase<LearningElement, LearningElementViewModel>().ReverseMap();
        CreateMap<ImageTransferElement, ImageTransferElementViewModel>().IncludeBase<LearningElement, LearningElementViewModel>().ReverseMap();
        CreateMap<PdfTransferElement, PdfTransferElementViewModel>().IncludeBase<LearningElement, LearningElementViewModel>().ReverseMap();
        CreateMap<VideoActivationElement, VideoActivationElementViewModel>().IncludeBase<LearningElement, LearningElementViewModel>().ReverseMap();
        CreateMap<VideoTransferElement, VideoTransferElementViewModel>().IncludeBase<LearningElement, LearningElementViewModel>().ReverseMap();
    }

    private void CreatePersistEntityMaps()
    {
        CreateMap<LearningWorld, LearningWorldPe>().ReverseMap();
        CreateMap<LearningSpace, LearningSpacePe>().ReverseMap();
        CreateMap<LearningElement, LearningElementPe>()
            .ForMember(x => x.Content, opt => opt.MapFrom(src=>src.LearningContent));
        CreateMap<LearningElementPe, LearningElement>()
            .ForMember(x => x.LearningContent, opt => opt.MapFrom(src => src.Content));
        CreateMap<LearningContent, LearningContentPe>().ReverseMap();
        
        CreateMap<H5PActivationElement, H5PActivationElementPe>().IncludeBase<LearningElement, LearningElementPe>().ReverseMap();
        CreateMap<H5PInteractionElement, H5PInteractionElementPe>().IncludeBase<LearningElement, LearningElementPe>().ReverseMap();
        CreateMap<H5PTestElement, H5PTestElementPe>().IncludeBase<LearningElement, LearningElementPe>().ReverseMap();
        CreateMap<ImageTransferElement, ImageTransferElementPe>().IncludeBase<LearningElement, LearningElementPe>().ReverseMap();
        CreateMap<PdfTransferElement, PdfTransferElementPe>().IncludeBase<LearningElement, LearningElementPe>().ReverseMap();
        CreateMap<VideoActivationElement, VideoActivationElementPe>().IncludeBase<LearningElement, LearningElementPe>().ReverseMap();
        CreateMap<VideoTransferElement, VideoTransferElementPe>().IncludeBase<LearningElement, LearningElementPe>().ReverseMap();
        
        CreateMap<LearningElementDifficultyEnum, LearningElementDifficultyEnumPe>().ReverseMap();
    }
}