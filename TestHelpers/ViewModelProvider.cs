using System.Globalization;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningContent.Story;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Adaptivity;
using Shared.LearningOutcomes;

namespace TestHelpers;

public static class ViewModelProvider
{
    public static AuthoringToolWorkspaceViewModel GetAuthoringToolWorkspace()
    {
        return new AuthoringToolWorkspaceViewModel();
    }

    public static LearningWorldViewModel GetLearningWorld()
    {
        return new LearningWorldViewModel("LWVMn", "LWVMsn", "LWVMa", "LWVMl", "LWVMd", "LWVMg", "LWVMev", "LWVMek");
    }

    public static LearningSpaceViewModel GetLearningSpace(bool unsavedChanges = false, FloorPlanEnum? floorPlan = null,
        TopicViewModel? assignedTopic = null, double positionX = 0, double positionY = 0)
    {
        return new LearningSpaceViewModel("LSVMn", "LSVMd", Theme.CampusAschaffenburg, 4,
            GetLearningOutcomeCollection(),
            floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum)floorPlan), positionX: positionX,
            positionY: positionY) { UnsavedChanges = unsavedChanges, AssignedTopic = assignedTopic };
    }

    public static ILearningOutcomeViewModel GetLearningOutcome(bool manual = false,
        TaxonomyLevel level = TaxonomyLevel.Level1, string what = "what", string whereby = "whereby",
        string whatFor = "whatFor", string verbOfVisibility = "VerbOfVisibility",
        CultureInfo? culture = null) =>
        manual
            ? new ManualLearningOutcomeViewModel("Outcome")
            : new StructuredLearningOutcomeViewModel(level, what, whereby, whatFor, verbOfVisibility, culture ??
                new CultureInfo("de-DE"));

    public static LearningOutcomeCollectionViewModel GetLearningOutcomeCollection()
    {
        return new LearningOutcomeCollectionViewModel()
        {
            LearningOutcomes = GetLearningOutcomes(),
            UnsavedChanges = false
        };
    }

    public static List<ILearningOutcomeViewModel> GetLearningOutcomes()
    {
        return new List<ILearningOutcomeViewModel>()
        {
            new ManualLearningOutcomeViewModel("Outcome"),
            new StructuredLearningOutcomeViewModel(TaxonomyLevel.Level1, "what", "whereby", "whatFor",
                "VerbOfVisibility",
                CultureInfo.CurrentCulture)
        };
    }

    public static LearningSpaceLayoutViewModel GetLearningSpaceLayout(
        FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L)
    {
        return new LearningSpaceLayoutViewModel(floorPlan);
    }

    public static LearningElementViewModel GetLearningElement(string append = "",
        ILearningContentViewModel? content = null, ILearningSpaceViewModel? parent = null, int workload = 1,
        int points = 1)
    {
        return new LearningElementViewModel("LEVMn" + append, content!, "LEVMd" + append, "LEVMg" + append,
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, parent: parent, workload: workload,
            points: points);
    }

    public static LearningElementViewModel GetStoryElement(string append = "",
        ILearningContentViewModel? content = null, ILearningSpaceViewModel? parent = null, int workload = 1,
        int points = 1)
    {
        return new LearningElementViewModel("LEVMn" + append, content ?? GetStoryContent(), "LEVMd" + append,
            "LEVMg" + append, LearningElementDifficultyEnum.Easy, ElementModel.a_npc_defaultnpc, parent: parent,
            workload: workload, points: points);
    }

    public static LearningElementViewModel GetAdaptivityElement(string append = "",
        IAdaptivityContentViewModel? content = null, ILearningSpaceViewModel? parent = null, int workload = 1,
        int points = 1)
    {
        return new LearningElementViewModel("LEVMn" + append, content ?? GetAdaptivityContent(), "LEVMd" + append,
            "LEVMg" + append, LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, parent: parent,
            workload: workload, points: points);
    }

    public static PathWayConditionViewModel GetPathWayCondition()
    {
        return new PathWayConditionViewModel(ConditionEnum.And, false);
    }

    public static LearningPathwayViewModel GetLearningPathway(IObjectInPathWayViewModel? source = null,
        IObjectInPathWayViewModel? target = null)
    {
        return new LearningPathwayViewModel(source ?? GetPathWayCondition(), target ?? GetPathWayCondition());
    }

    public static LinkContentViewModel GetLinkContent()
    {
        return new LinkContentViewModel("LCVMn a name", "LCVMl a link");
    }

    public static FileContentViewModel GetFileContent(string? name = null, string? type = null, string? filepath = null)
    {
        return new FileContentViewModel(name ?? "FCVMn a name", type ?? "FCVMt a type", filepath ?? "FCVMf a filepath");
    }

    public static StoryContentViewModel GetStoryContent()
    {
        return new StoryContentViewModel("a story name", new List<string> { "a story", "another story" });
    }

    public static TopicViewModel GetTopic()
    {
        return new TopicViewModel("TVMn a topic");
    }

    public static LearningWorldViewModel GetLearningWorldWithSpace()
    {
        var world = GetLearningWorld();
        world.LearningSpaces.Add(GetLearningSpace());
        return world;
    }

    public static LearningWorldViewModel GetLearningWorldWithElement()
    {
        var world = GetLearningWorld();
        var element = GetLearningElement();
        world.UnplacedLearningElements.Add(element);
        return world;
    }

    public static LearningSpaceViewModel GetLearningSpaceWithElement()
    {
        var space = GetLearningSpace();
        var element = GetLearningElement();
        element.Parent = space;
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        return space;
    }

    public static LearningWorldViewModel GetLearningWorldWithSpaceWithElement()
    {
        var world = GetLearningWorld();
        var space = GetLearningSpaceWithElement();
        space.RequiredPoints = 1;
        world.LearningSpaces.Add(space);
        return world;
    }

    public static CorrectnessTriggerViewModel GetCorrectnessTrigger(AnswerResult result = AnswerResult.Correct)
    {
        return new CorrectnessTriggerViewModel(result);
    }

    public static TimeTriggerViewModel GetTimeTrigger(int time = 0, TimeFrameType timeFrameType = TimeFrameType.Until)
    {
        return new TimeTriggerViewModel(time, timeFrameType);
    }

    public static CompositeTriggerViewModel GetCompositeTrigger(ConditionEnum type = ConditionEnum.And,
        IAdaptivityTriggerViewModel? leftSide = null, IAdaptivityTriggerViewModel? rightSide = null)
    {
        leftSide ??= GetCorrectnessTrigger();
        rightSide ??= GetTimeTrigger();
        return new CompositeTriggerViewModel(type, leftSide, rightSide);
    }

    public static CommentActionViewModel GetCommentAction(string? comment = null)
    {
        return new CommentActionViewModel(comment ?? "a comment");
    }

    public static ElementReferenceActionViewModel GetElementReferenceAction(Guid? elementGuid = null,
        string comment = "")
    {
        elementGuid ??= Guid.NewGuid();
        return new ElementReferenceActionViewModel(elementGuid.Value, comment);
    }

    public static ContentReferenceActionViewModel GetContentReferenceAction(ILearningContentViewModel? content = null,
        string comment = "")
    {
        content ??= GetFileContent();
        return new ContentReferenceActionViewModel(content, comment);
    }

    public static AdaptivityRuleViewModel GetRule()
    {
        return new AdaptivityRuleViewModel(GetCorrectnessTrigger(),
            GetCommentAction());
    }

    public static ChoiceViewModel GetChoice(string? text = null)
    {
        return new ChoiceViewModel(text ?? "a choice");
    }

    public static MultipleChoiceSingleResponseQuestionViewModel GetMultipleChoiceSingleResponseQuestion()
    {
        var choiceViewModels = new List<ChoiceViewModel> { GetChoice() };
        return new MultipleChoiceSingleResponseQuestionViewModel(1, "question text",
            choiceViewModels.First(), QuestionDifficulty.Medium, choiceViewModels);
    }

    public static MultipleChoiceMultipleResponseQuestionViewModel GetMultipleChoiceMultipleResponseQuestion()
    {
        var choiceViewModels = new List<ChoiceViewModel> { GetChoice(), GetChoice() };
        return new MultipleChoiceMultipleResponseQuestionViewModel(1, "question text",
            QuestionDifficulty.Hard, choiceViewModels, choiceViewModels);
    }

    public static AdaptivityTaskViewModel GetAdaptivityTask()
    {
        return new AdaptivityTaskViewModel(
            new List<IAdaptivityQuestionViewModel>
                { GetMultipleChoiceSingleResponseQuestion(), GetMultipleChoiceMultipleResponseQuestion() },
            QuestionDifficulty.Hard,
            "taskname");
    }

    public static IAdaptivityContentViewModel GetAdaptivityContent()
    {
        var task = GetAdaptivityTask();
        var rule = GetRule();
        task.Questions.First().Rules.Add(rule);
        return new AdaptivityContentViewModel(new List<IAdaptivityTaskViewModel> { task, GetAdaptivityTask() });
    }
}