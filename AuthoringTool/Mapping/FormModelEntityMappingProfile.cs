using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.Entities.LearningContent.Story;
using Presentation.Components.Adaptivity.Dialogues;
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
        CreateMap<FileContentFormModel, FileContent>();
        CreateMap<AdaptivityContentFormModel, AdaptivityContent>()
            .ReverseMap();
        CreateMap<StoryContentFormModel, StoryContent>();

        CreateMap<ILearningContentFormModel, ILearningContent>()
            .IncludeAllDerived();

        CreateMap<LinkContentFormModel, ILearningContent>()
            .As<LinkContent>();
        CreateMap<FileContentFormModel, ILearningContent>()
            .As<FileContent>();
        CreateMap<AdaptivityContentFormModel, ILearningContent>()
            .As<AdaptivityContent>();
        CreateMap<StoryContentFormModel, ILearningContent>()
            .As<StoryContent>();
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
            .ForMember(x => x.Difficulty, opt => opt.Ignore())
            .ForMember(x => x.Rules, opt => opt.Ignore())
            .ConstructUsing((formModel, context) => formModel.IsSingleResponse
                ? context.Mapper.Map<MultipleChoiceSingleResponseQuestion>(formModel)
                : context.Mapper.Map<MultipleChoiceMultipleResponseQuestion>(formModel));
        CreateMap<MultipleChoiceQuestionFormModel, MultipleChoiceMultipleResponseQuestion>()
            .ForMember(x => x.Difficulty, opt => opt.Ignore())
            .ForMember(x => x.Rules, opt => opt.Ignore());
        CreateMap<MultipleChoiceQuestionFormModel, MultipleChoiceSingleResponseQuestion>()
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .ForMember(x => x.CorrectChoice, opt => opt.MapFrom(x => x.CorrectChoices.First()))
            .ForMember(x => x.Difficulty, opt => opt.Ignore())
            .ForMember(x => x.Rules, opt => opt.Ignore());

        CreateMap<ChoiceFormModel, Choice>();
    }
}