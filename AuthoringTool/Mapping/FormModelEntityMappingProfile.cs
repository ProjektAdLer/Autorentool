using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.LinkContent;
using Presentation.Components.Forms.Models;

namespace AuthoringTool.Mapping;

public class FormModelEntityMappingProfile : Profile
{
    private FormModelEntityMappingProfile()
    {
        DisableConstructorMapping();
        CreateWorldMap();
        CreateSpaceMap();
        CreateElementMap();
        CreateContentMap();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new FormModelEntityMappingProfile());
        cfg.AddCollectionMappersOnce();
    };

    private void CreateContentMap()
    {
        CreateMap<LinkContentFormModel, LinkContent>();
    }

    private void CreateElementMap()
    {
        CreateMap<LearningElementFormModel, LearningElement>();
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