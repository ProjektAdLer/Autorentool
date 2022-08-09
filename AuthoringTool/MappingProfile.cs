using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.Entities;
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
    }
    
}