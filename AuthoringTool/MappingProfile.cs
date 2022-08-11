using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using AutoMapper;

namespace AuthoringTool;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<LearningWorld, LearningWorldPe>();
        CreateMap<LearningWorldPe, LearningWorld>();
        CreateMap<LearningElement, LearningElementPe>();
        CreateMap<LearningElementPe, LearningElement>();
        CreateMap<LearningSpace, LearningSpacePe>();
        CreateMap<LearningSpacePe, LearningSpace>();
        CreateMap<LearningContent, LearningContentPe>();
        CreateMap<LearningContentPe, LearningContent>();
        CreateMap<H5PActivationElement, H5PActivationElementPe>();
        CreateMap<H5PActivationElementPe, H5PActivationElement>();
        CreateMap<H5PInteractionElement, H5PInteractionElementPe>();
        CreateMap<H5PInteractionElementPe, H5PInteractionElement>();
        CreateMap<H5PTestElement, H5PTestElementPe>();
        CreateMap<H5PTestElementPe, H5PTestElement>();
        CreateMap<ImageTransferElement, ImageTransferElementPe>();
        CreateMap<ImageTransferElementPe, ImageTransferElement>();
        CreateMap<PdfTransferElement, PdfTransferElementPe>();
        CreateMap<PdfTransferElementPe, PdfTransferElement>();
        CreateMap<VideoActivationElement, VideoActivationElementPe>();
        CreateMap<VideoActivationElementPe, VideoActivationElement>();
        CreateMap<VideoTransferElement, VideoTransferElementPe>();
        CreateMap<VideoTransferElementPe, VideoTransferElement>();
        CreateMap<LearningElementDifficultyEnum, LearningElementDifficultyEnumPe>();
        CreateMap<LearningElementDifficultyEnumPe, LearningElementDifficultyEnum>();
    }
    
}