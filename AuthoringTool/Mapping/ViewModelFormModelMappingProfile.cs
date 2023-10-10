using AutoMapper;
using Presentation.Components.Adaptivity.Forms.Models;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace AuthoringTool.Mapping;

public class ViewModelFormModelMappingProfile : Profile
{
    private ViewModelFormModelMappingProfile()
    {
        CreateWorldMap();
        CreateSpaceMap();
        CreateAdvancedSpaceMap();
        CreateElementMap();
        CreateLinkContentMap();
        CreateAdaptivityQuestionMap();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ViewModelFormModelMappingProfile());
        cfg.AddCollectionMappersOnce();
    };

    private void CreateLinkContentMap()
    {
        CreateMap<LinkContentViewModel, LinkContentFormModel>()
            .ReverseMap();
    }

    private void CreateElementMap()
    {
        CreateMap<LearningElementViewModel, LearningElementFormModel>()
            .ReverseMap();
    }

    private void CreateSpaceMap()
    {
        CreateMap<LearningSpaceViewModel, LearningSpaceFormModel>()
            .ReverseMap();
    }

    private void CreateAdvancedSpaceMap()
    {
        CreateMap<AdvancedLearningSpaceViewModel, LearningSpaceFormModel>()
            .ReverseMap();
    }

    private void CreateWorldMap()
    {
        CreateMap<LearningWorldViewModel, LearningWorldFormModel>()
            .ReverseMap();
    }

    private void CreateAdaptivityQuestionMap()
    {
        CreateMap<IMultipleChoiceQuestionViewModel, MultipleChoiceQuestionFormModel>();
        CreateMap<MultipleChoiceMultipleResponseQuestionViewModel, MultipleChoiceQuestionFormModel>()
            .ForMember(x => x.IsSingleResponse, opt => opt.MapFrom(x => false));
        CreateMap<MultipleChoiceSingleResponseQuestionViewModel, MultipleChoiceQuestionFormModel>()
            .ForMember(x => x.IsSingleResponse, opt => opt.MapFrom(x => true));
    }
}