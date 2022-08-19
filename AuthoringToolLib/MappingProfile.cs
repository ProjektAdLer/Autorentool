using AuthoringToolLib.Entities;
using AuthoringToolLib.PresentationLogic.LearningElement;
using AutoMapper;
using Generator.PersistEntities;

namespace AuthoringToolLib;

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
        CreateMap<H5PActivationElement, H5PActivationElementPe>().IncludeBase<LearningElement, LearningElementPe>();
        CreateMap<H5PActivationElementPe, H5PActivationElement>().IncludeBase<LearningElementPe,LearningElement>();
        CreateMap<H5PInteractionElement, H5PInteractionElementPe>().IncludeBase<LearningElement, LearningElementPe>();
        CreateMap<H5PInteractionElementPe, H5PInteractionElement>().IncludeBase<LearningElementPe,LearningElement>();
        CreateMap<H5PTestElement, H5PTestElementPe>().IncludeBase<LearningElement, LearningElementPe>();
        CreateMap<H5PTestElementPe, H5PTestElement>().IncludeBase<LearningElementPe,LearningElement>();
        CreateMap<ImageTransferElement, ImageTransferElementPe>().IncludeBase<LearningElement, LearningElementPe>();
        CreateMap<ImageTransferElementPe, ImageTransferElement>().IncludeBase<LearningElementPe,LearningElement>();
        CreateMap<PdfTransferElement, PdfTransferElementPe>().IncludeBase<LearningElement, LearningElementPe>();
        CreateMap<PdfTransferElementPe, PdfTransferElement>().IncludeBase<LearningElementPe,LearningElement>();
        CreateMap<VideoActivationElement, VideoActivationElementPe>().IncludeBase<LearningElement, LearningElementPe>();
        CreateMap<VideoActivationElementPe, VideoActivationElement>().IncludeBase<LearningElementPe,LearningElement>();
        CreateMap<VideoTransferElement, VideoTransferElementPe>().IncludeBase<LearningElement, LearningElementPe>();
        CreateMap<VideoTransferElementPe, VideoTransferElement>().IncludeBase<LearningElementPe,LearningElement>();
        CreateMap<LearningElementDifficultyEnum, LearningElementDifficultyEnumPe>();
        CreateMap<LearningElementDifficultyEnumPe, LearningElementDifficultyEnum>();
    }
    
}