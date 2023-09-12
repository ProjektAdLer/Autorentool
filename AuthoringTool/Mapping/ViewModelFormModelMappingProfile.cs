using AutoMapper;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace AuthoringTool.Mapping;

public class ViewModelFormModelMappingProfile : Profile
{
    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ViewModelFormModelMappingProfile());
        cfg.AddCollectionMappersOnce();
    };

    private ViewModelFormModelMappingProfile()
    {
        CreateWorldMap();
        CreateSpaceMap();
        CreateAdvancedSpaceMap();
        CreateElementMap();
        CreateLinkContentMap();
    }

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
}