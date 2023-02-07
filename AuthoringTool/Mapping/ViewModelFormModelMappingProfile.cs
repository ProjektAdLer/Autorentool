using AutoMapper;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace AuthoringTool.Mapping;

public class ViewModelFormModelMappingProfile : Profile
{
    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ViewModelFormModelMappingProfile());
    };
    
    private ViewModelFormModelMappingProfile()
    {
        CreateWorldMap();
        CreateSpaceMap();
    }

    private void CreateSpaceMap()
    {
        CreateMap<LearningSpaceViewModel, LearningSpaceFormModel>();
    }

    private void CreateWorldMap()
    {
        CreateMap<LearningWorldViewModel, LearningWorldFormModel>();
    }
}