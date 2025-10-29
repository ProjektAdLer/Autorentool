using AutoMapper;
using AutoMapper.Internal;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.Components.Adaptivity.Forms.Models;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningContent.Story;
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
        CreateElementMap();
        CreateContentMap();
        CreateAdaptivityQuestionMap();
        CreateAdaptivityQuestionMap();
        CreateQuestionChoiceMap();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ViewModelFormModelMappingProfile());
        cfg.AddCollectionMappersOnce();
        cfg.Internal().MethodMappingEnabled = false;
    };

    private void CreateContentMap()
    {
        CreateMap<LinkContentViewModel, LinkContentFormModel>()
            .ReverseMap();
        CreateMap<AdaptivityContentViewModel, AdaptivityContentFormModel>()
            .ReverseMap();
        CreateMap<FileContentViewModel, FileContentFormModel>()
            .ReverseMap();
        CreateMap<StoryContentViewModel, StoryContentFormModel>()
            .ForMember(x => x.Name, opt => opt.MapFrom(_ => "StoryContent"))
            .ReverseMap();

        CreateMap<ILearningContentViewModel, ILearningContentFormModel>()
            .IncludeAllDerived()
            .ReverseMap()
            .IncludeAllDerived();

        CreateMap<LinkContentViewModel, ILearningContentFormModel>()
            .As<LinkContentFormModel>();
        CreateMap<FileContentViewModel, ILearningContentFormModel>()
            .As<FileContentFormModel>();
        CreateMap<AdaptivityContentViewModel, ILearningContentFormModel>()
            .As<AdaptivityContentFormModel>();
        CreateMap<StoryContentViewModel, ILearningContentFormModel>()
            .As<StoryContentFormModel>();

        CreateMap<LinkContentFormModel, ILearningContentViewModel>()
            .As<LinkContentViewModel>();
        CreateMap<FileContentFormModel, ILearningContentViewModel>()
            .As<FileContentViewModel>();
        CreateMap<AdaptivityContentFormModel, ILearningContentViewModel>()
            .As<AdaptivityContentViewModel>();
        CreateMap<StoryContentFormModel, ILearningContentViewModel>()
            .As<StoryContentViewModel>();
    }

    private void CreateElementMap()
    {
        CreateMap<LearningElementViewModel, LearningElementFormModel>()
            .ForMember(x => x.LearningContent, opt => opt.Ignore())
            .AfterMap(MapContent)
            .ReverseMap();
        return;

        void MapContent(LearningElementViewModel source, LearningElementFormModel dest, ResolutionContext ctx)
        {
            dest.LearningContent =
                ctx.Mapper.Map<ILearningContentFormModel>(source.LearningContent);
        }
    }

    private void CreateSpaceMap()
    {
        CreateMap<LearningSpaceViewModel, LearningSpaceFormModel>()
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
            .ForMember(x => x.IsSingleResponse, opt => opt.MapFrom(x => false))
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((vm, fm, _) =>
                fm.CorrectChoices = fm.Choices
                    .Where(choiceFm => vm.CorrectChoices.Any(choiceVm => choiceVm.Id.Equals(choiceFm.Id))).ToList())
            .ReverseMap();
        CreateMap<MultipleChoiceSingleResponseQuestionViewModel, MultipleChoiceQuestionFormModel>()
            .ForMember(x => x.IsSingleResponse, opt => opt.MapFrom(x => true))
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((vm, fm, _) =>
                fm.CorrectChoices = fm.Choices.Where(choiceFm => choiceFm.Id == vm.CorrectChoice.Id).ToList())
            .ReverseMap();
    }

    private void CreateQuestionChoiceMap()
    {
        CreateMap<ChoiceViewModel, ChoiceFormModel>()
            .ReverseMap();
    }
}