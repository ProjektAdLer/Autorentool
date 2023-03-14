using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Presentation.Components.Forms.Models;

namespace AuthoringTool.Mapping;

public class FormModelEntityMappingProfile : Profile
{
    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new FormModelEntityMappingProfile());
    };

    private FormModelEntityMappingProfile()
    {
        DisableConstructorMapping();
        CreateWorldMap();
        CreateSpaceMap();
        CreateContentMap();
    }

    private void CreateContentMap()
    {
        CreateMap<LinkContentFormModel, LinkContent>();
    }

    private void CreateSpaceMap()
    {
        CreateMap<LearningSpaceFormModel, LearningSpace>();
    }

    private void CreateWorldMap()
    {
        CreateMap<LearningWorldFormModel, LearningWorld>();
    }
}