using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.Entities;
using AutoMapper;

namespace AuthoringTool;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<ILearningWorld, ILearningWorldPe>();
        CreateMap<ILearningWorldPe, ILearningWorld>();
        CreateMap<ILearningElement, ILearningElementPe>();
        CreateMap<ILearningElementPe, ILearningElement>();
        CreateMap<ILearningSpace, ILearningSpacePe>();
        CreateMap<ILearningSpacePe, ILearningSpace>();
        CreateMap<ILearningContent, ILearningContentPe>();
        CreateMap<ILearningContentPe, ILearningContent>();
    }
    
}