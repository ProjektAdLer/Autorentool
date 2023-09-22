using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.LinkContent;
using Presentation.Components.Adaptivity.Forms.Models;
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
        CreateAdaptivityQuestionMap();
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

    private void CreateAdaptivityQuestionMap()
    {
        CreateMap<MultipleChoiceQuestionFormModel, IMultipleChoiceQuestion>()
            .As<MultipleChoiceMultipleResponseQuestion>();
        CreateMap<MultipleChoiceQuestionFormModel, MultipleChoiceMultipleResponseQuestion>();
        CreateMap<MultipleChoiceQuestionFormModel, MultipleChoiceSingleResponseQuestion>();
    }
}