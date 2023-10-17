using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using PersistEntities;
using PersistEntities.LearningContent;
using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Question;
using PersistEntities.LearningContent.Trigger;

namespace AuthoringTool.Mapping;

/// <summary>
/// Configures mappings between Entity and PersistEntity classes
/// </summary>
public class EntityPersistEntityMappingProfile : Profile
{
    private EntityPersistEntityMappingProfile()
    {
        DisableConstructorMapping();
        CreateLearningWorldMap();
        CreateLearningSpaceMap();
        CreateLearningElementMap();
        CreateLearningContentMap();
        CreatePathwayMaps();
        CreateInterfaceMaps();
        CreateLearningSpaceLayoutMap();
        CreateTopicMap();
        CreateAdaptivityMap();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new EntityPersistEntityMappingProfile());
        cfg.AddCollectionMappersOnce();
    };

    private void CreateTopicMap()
    {
        CreateMap<Topic, TopicPe>()
            .EqualityComparison((entity, pe) => entity.Id == pe.Id)
            .ReverseMap();
    }

    private void CreateLearningSpaceLayoutMap()
    {
        CreateMap<ILearningSpaceLayout, LearningSpaceLayoutPe>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.Capacity, opt => opt.Ignore());
        CreateMap<ILearningSpaceLayoutPe, LearningSpaceLayout>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore());
        CreateMap<LearningSpaceLayout, LearningSpaceLayoutPe>()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.LearningElements, opt => opt.Ignore())
            .ForMember(x => x.Capacity, opt => opt.Ignore())
            .AfterMap(MapSpaceLayoutElements)
            .ReverseMap()
            .ForMember(x => x.ContainedLearningElements, opt => opt.Ignore())
            .ForMember(x => x.LearningElements, opt => opt.Ignore())
            .AfterMap(MapSpaceLayoutElements);
    }

    private void MapSpaceLayoutElements(LearningSpaceLayout source, LearningSpaceLayoutPe destination,
        ResolutionContext ctx)
    {
        var sourceNewElements = source.LearningElements
            .Where(x => !SameIdAtSameIndex(destination, x))
            .Select(tup =>
                new KeyValuePair<int, ILearningElementPe>(tup.Key,
                    ctx.Mapper.Map<LearningElementPe>(tup.Value)));
        foreach (var (key, _) in destination.LearningElements
                     .Where(x => !SameIdAtSameIndex(source, x)))
        {
            destination.LearningElements.Remove(key);
        }

        foreach (var (key, value) in destination.LearningElements)
        {
            var sourceElement = source.LearningElements
                .First(x => x.Key == key && x.Value.Id == value.Id);
            ctx.Mapper.Map(sourceElement.Value, value);
        }

        destination.LearningElements = sourceNewElements
            .Union(destination.LearningElements)
            .ToDictionary(tup => tup.Key, tup => tup.Value);
    }

    private void MapSpaceLayoutElements(LearningSpaceLayoutPe source, LearningSpaceLayout destination,
        ResolutionContext ctx)
    {
        var sourceNewElements = source.LearningElements
            .Where(x => !SameIdAtSameIndex(destination, x))
            .Select(tup =>
                new KeyValuePair<int, ILearningElement>(tup.Key,
                    ctx.Mapper.Map<LearningElement>(tup.Value)));
        foreach (var (key, _) in destination.LearningElements
                     .Where(x => !SameIdAtSameIndex(source, x)))
        {
            destination.LearningElements.Remove(key);
        }

        foreach (var (key, value) in destination.LearningElements)
        {
            var sourceElement = source.LearningElements
                .First(x => x.Key == key && x.Value.Id == value.Id);
            ctx.Mapper.Map(sourceElement.Value, value);
        }

        destination.LearningElements = sourceNewElements
            .Union(destination.LearningElements)
            .ToDictionary(tup => tup.Key, tup => tup.Value);
    }

    private static bool
        SameIdAtSameIndex(ILearningSpaceLayoutPe destination, KeyValuePair<int, ILearningElement> kvp) =>
        destination.LearningElements.Any(y => y.Key == kvp.Key && y.Value.Id == kvp.Value.Id);

    private static bool
        SameIdAtSameIndex(ILearningSpaceLayout destination, KeyValuePair<int, ILearningElementPe> kvp) =>
        destination.LearningElements.Any(y => y.Key == kvp.Key && y.Value.Id == kvp.Value.Id);

    private void CreatePathwayMaps()
    {
        CreateMap<LearningPathway, LearningPathwayPe>()
            .ReverseMap();
        CreateMap<PathWayCondition, PathWayConditionPe>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore());
    }

    private void CreateLearningContentMap()
    {
        CreateMap<FileContent, FileContentPe>()
            .ReverseMap();
        CreateMap<LinkContent, LinkContentPe>()
            .ReverseMap();
        CreateMap<FileContent, ILearningContentPe>()
            .As<FileContentPe>();
        CreateMap<LinkContent, ILearningContentPe>()
            .As<LinkContentPe>();
        CreateMap<FileContentPe, ILearningContent>()
            .As<FileContent>();
        CreateMap<LinkContentPe, ILearningContent>()
            .As<LinkContent>();
        CreateMap<ILearningContent, ILearningContentPe>()
            .IncludeAllDerived()
            .ReverseMap()
            .IncludeAllDerived();
    }

    private void CreateInterfaceMaps()
    {
        CreateMap<PathWayCondition, IObjectInPathWayPe>().As<PathWayConditionPe>();
        CreateMap<LearningSpace, IObjectInPathWayPe>().As<LearningSpacePe>();

        CreateMap<LearningElement, ILearningElementPe>().As<LearningElementPe>();
        CreateMap<LearningElementPe, ILearningElement>().As<LearningElement>();

        CreateMap<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap();

        CreateMap<LearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<LearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
        CreateMap<ILearningSpaceLayout, ILearningSpaceLayoutPe>().As<LearningSpaceLayoutPe>();
        CreateMap<ILearningSpaceLayoutPe, ILearningSpaceLayout>().As<LearningSpaceLayout>();
    }

    private void CreateLearningElementMap()
    {
        CreateMap<LearningElement, LearningElementPe>()
            .ReverseMap()
            .ForMember(x => x.Parent, opt => opt.Ignore())
            .ForMember(x => x.Id, opt => opt.Ignore());
    }

    private void CreateLearningSpaceMap()
    {
        CreateMap<LearningSpace, LearningSpacePe>()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .IncludeBase<IObjectInPathWay, IObjectInPathWayPe>()
            .ReverseMap()
            .ForMember(x => x.InBoundObjects, opt => opt.Ignore())
            .ForMember(x => x.OutBoundObjects, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var element in d.ContainedLearningElements)
                {
                    element.Parent = d;
                }
            });
    }

    private void CreateLearningWorldMap()
    {
        CreateMap<LearningWorld, LearningWorldPe>()
            .ForMember(x => x.ObjectsInPathWaysPe, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var pathWay in d.LearningPathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.SourceObject.Id);
                    pathWay.TargetObject = d.ObjectsInPathWaysPe.First(x => x.Id == pathWay.TargetObject.Id);
                }
            })
            .AfterMap((_, d) =>
            {
                foreach (var objectInPathWayPe in d.ObjectsInPathWaysPe)
                {
                    objectInPathWayPe.InBoundObjects = d.LearningPathways
                        .Where(x => x.TargetObject.Id == objectInPathWayPe.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWayPe.OutBoundObjects = d.LearningPathways
                        .Where(x => x.SourceObject.Id == objectInPathWayPe.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            })
            .ReverseMap()
            .ForMember(x => x.ObjectsInPathWays, opt => opt.Ignore())
            .AfterMap((_, d) =>
            {
                foreach (var pathWay in d.LearningPathways)
                {
                    pathWay.SourceObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.SourceObject.Id);
                    pathWay.TargetObject = d.ObjectsInPathWays.First(x => x.Id == pathWay.TargetObject.Id);
                }
            })
            .AfterMap((_, d) =>
            {
                foreach (var objectInPathWay in d.ObjectsInPathWays)
                {
                    objectInPathWay.InBoundObjects = d.LearningPathways
                        .Where(x => x.TargetObject.Id == objectInPathWay.Id)
                        .Select(x => x.SourceObject).ToList();
                    objectInPathWay.OutBoundObjects = d.LearningPathways
                        .Where(x => x.SourceObject.Id == objectInPathWay.Id)
                        .Select(x => x.TargetObject).ToList();
                }
            });
    }

    private void CreateAdaptivityMap()
    {
        CreateAdaptivityTriggerMap();
        CreateAdaptivityActionMap();
        CreateAdaptivityQuestionMap();
        CreateChoiceMap();
        CreateAdaptivityRuleMap();
        CreateAdaptivityTaskMap();
        CreateAdaptivityContentMap();
    }

    private void CreateAdaptivityTriggerMap()
    {
        CreateMap<IAdaptivityTrigger, IAdaptivityTriggerPe>()
            .ReverseMap();

        CreateMap<CorrectnessTrigger, IAdaptivityTriggerPe>()
            .As<CorrectnessTriggerPe>();
        CreateMap<CorrectnessTriggerPe, IAdaptivityTrigger>()
            .As<CorrectnessTrigger>();

        CreateMap<TimeTrigger, IAdaptivityTriggerPe>()
            .As<TimeTriggerPe>();
        CreateMap<TimeTriggerPe, IAdaptivityTrigger>()
            .As<TimeTrigger>();

        CreateMap<CompositeTrigger, IAdaptivityTriggerPe>()
            .As<CompositeTriggerPe>();
        CreateMap<CompositeTriggerPe, IAdaptivityTrigger>()
            .As<CompositeTrigger>();

        CreateMap<CorrectnessTrigger, CorrectnessTriggerPe>()
            .IncludeBase<IAdaptivityTrigger, IAdaptivityTriggerPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityTriggerPe, IAdaptivityTrigger>();
        CreateMap<TimeTrigger, TimeTriggerPe>()
            .IncludeBase<IAdaptivityTrigger, IAdaptivityTriggerPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityTriggerPe, IAdaptivityTrigger>();
        CreateMap<CompositeTrigger, CompositeTriggerPe>()
            .IncludeBase<IAdaptivityTrigger, IAdaptivityTriggerPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityTriggerPe, IAdaptivityTrigger>();
    }

    private void CreateAdaptivityActionMap()
    {
        CreateMap<IAdaptivityAction, IAdaptivityActionPe>()
            .EqualityComparison((entity, pe) => entity.Id == pe.Id)
            .ReverseMap()
            .EqualityComparison((pe, entity) => pe.Id == entity.Id);

        CreateMap<CommentAction, IAdaptivityActionPe>()
            .As<CommentActionPe>();
        CreateMap<CommentActionPe, IAdaptivityAction>()
            .As<CommentAction>();

        CreateMap<ElementReferenceAction, IAdaptivityActionPe>()
            .As<ElementReferenceActionPe>();
        CreateMap<ElementReferenceActionPe, IAdaptivityAction>()
            .As<ElementReferenceAction>();

        CreateMap<ContentReferenceAction, IAdaptivityActionPe>()
            .As<ContentReferenceActionPe>();
        CreateMap<ContentReferenceActionPe, IAdaptivityAction>()
            .As<ContentReferenceAction>();

        CreateMap<CommentAction, CommentActionPe>()
            .IncludeBase<IAdaptivityAction, IAdaptivityActionPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityActionPe, IAdaptivityAction>();
        CreateMap<ElementReferenceAction, ElementReferenceActionPe>()
            .IncludeBase<IAdaptivityAction, IAdaptivityActionPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityActionPe, IAdaptivityAction>();
        CreateMap<ContentReferenceAction, ContentReferenceActionPe>()
            .IncludeBase<IAdaptivityAction, IAdaptivityActionPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityActionPe, IAdaptivityAction>();
    }

    private void CreateAdaptivityQuestionMap()
    {
        CreateMap<IAdaptivityQuestion, IAdaptivityQuestionPe>()
            .ReverseMap();
        CreateMap<IMultipleChoiceQuestion, IMultipleChoiceQuestion>()
            .ReverseMap();

        CreateMap<MultipleChoiceSingleResponseQuestion, IAdaptivityQuestionPe>()
            .As<MultipleChoiceSingleResponseQuestionPe>();
        CreateMap<MultipleChoiceSingleResponseQuestionPe, IAdaptivityQuestion>()
            .As<MultipleChoiceSingleResponseQuestion>();

        CreateMap<MultipleChoiceMultipleResponseQuestion, IAdaptivityQuestionPe>()
            .As<MultipleChoiceMultipleResponseQuestionPe>();
        CreateMap<MultipleChoiceMultipleResponseQuestionPe, IAdaptivityQuestion>()
            .As<MultipleChoiceMultipleResponseQuestion>();

        CreateMap<MultipleChoiceSingleResponseQuestion, MultipleChoiceSingleResponseQuestionPe>()
            .IncludeBase<IAdaptivityQuestion, IAdaptivityQuestionPe>()
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .ForMember(x => x.CorrectChoice, opt => opt.Ignore())
            .AfterMap((x, y, _) => y.CorrectChoice = y.Choices.Single(choice => choice.Id == x.CorrectChoice.Id))
            .ReverseMap()
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .ForMember(x => x.CorrectChoice, opt => opt.Ignore())
            .AfterMap((x, y, _) => y.CorrectChoice = y.Choices.Single(choice => choice.Id == x.CorrectChoice.Id))
            .IncludeBase<IAdaptivityQuestionPe, IAdaptivityQuestion>();
        CreateMap<MultipleChoiceMultipleResponseQuestion, MultipleChoiceMultipleResponseQuestionPe>()
            .IncludeBase<IAdaptivityQuestion, IAdaptivityQuestionPe>()
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((x, y, _) =>
                y.CorrectChoices = y.Choices.Where(choice => x.CorrectChoices.Any(c => c.Id == choice.Id)).ToList())
            .ReverseMap()
            .ForMember(x => x.CorrectChoices, opt => opt.Ignore())
            .AfterMap((x, y, _) =>
                y.CorrectChoices = y.Choices.Where(choice => x.CorrectChoices.Any(c => c.Id == choice.Id)).ToList())
            .IncludeBase<IAdaptivityQuestionPe, IAdaptivityQuestion>();
    }

    private void CreateChoiceMap()
    {
        CreateMap<Choice, ChoicePe>()
            .ReverseMap();
    }

    private void CreateAdaptivityRuleMap()
    {
        CreateMap<IAdaptivityRule, IAdaptivityRulePe>()
            .ReverseMap();

        CreateMap<AdaptivityRule, IAdaptivityRulePe>()
            .As<AdaptivityRulePe>();
        CreateMap<AdaptivityRulePe, IAdaptivityRule>()
            .As<AdaptivityRule>();

        CreateMap<AdaptivityRule, AdaptivityRulePe>()
            .IncludeBase<IAdaptivityRule, IAdaptivityRulePe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityRulePe, IAdaptivityRule>();
    }

    private void CreateAdaptivityTaskMap()
    {
        CreateMap<IAdaptivityTask, IAdaptivityTaskPe>()
            .EqualityComparison((entity, pe) => entity.Id == pe.Id)
            .ReverseMap()
            .EqualityComparison((pe, entity) => entity.Id == pe.Id);

        CreateMap<AdaptivityTask, IAdaptivityTaskPe>()
            .As<AdaptivityTaskPe>();
        CreateMap<AdaptivityTaskPe, IAdaptivityTask>()
            .As<AdaptivityTask>();

        CreateMap<AdaptivityTask, AdaptivityTaskPe>()
            .IncludeBase<IAdaptivityTask, IAdaptivityTaskPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityTaskPe, IAdaptivityTask>();
    }

    private void CreateAdaptivityContentMap()
    {
        CreateMap<IAdaptivityContent, IAdaptivityContentPe>()
            .ReverseMap();

        CreateMap<AdaptivityContent, IAdaptivityContentPe>()
            .As<AdaptivityContentPe>();
        CreateMap<AdaptivityContentPe, IAdaptivityContent>()
            .As<AdaptivityContent>();

        CreateMap<AdaptivityContent, AdaptivityContentPe>()
            .IncludeBase<IAdaptivityContent, IAdaptivityContentPe>()
            .ReverseMap()
            .IncludeBase<IAdaptivityContentPe, IAdaptivityContent>();
    }
}