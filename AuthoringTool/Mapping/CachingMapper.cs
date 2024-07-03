using AutoMapper;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;

namespace AuthoringTool.Mapping;

public class CachingMapper : ICachingMapper
{
    private readonly Dictionary<Guid, object> _cache;
    private readonly ILogger<CachingMapper> _logger;

    private readonly IMapper _mapper;

    public CachingMapper(IMapper mapper, ICommandStateManager commandStateManager, ILogger<CachingMapper> logger)
    {
        _mapper = mapper;
        commandStateManager.RemovedCommandsFromStacks += OnRemovedCommandsFromStacks;
        _logger = logger;
        _cache = new Dictionary<Guid, object>();
    }

    internal IReadOnlyDictionary<Guid, object> ReadOnlyCache => _cache;

    public void Map<TSource, TDestination>(TSource entity, TDestination viewModel)
    {
        _logger.LogTrace("Logging {TSource} to {TDestination}", typeof(TSource).Name, typeof(TDestination).Name);
        switch (entity, viewModel)
        {
            case (AuthoringToolWorkspace s, IAuthoringToolWorkspaceViewModel d):
                CacheAuthoringToolWorkspaceContent(d);
                MapInternal(s, d);
                break;
            case (LearningWorld s, ILearningWorldViewModel d):
                CacheLearningWorldContent(d);
                MapInternal(s, d);
                break;
            case (Topic s, TopicViewModel d):
                MapInternal(s, d);
                break;
            case (LearningSpace s, ILearningSpaceViewModel d):
                CacheLearningSpaceContent(d);
                MapInternal(s, d);
                break;
            default:
                _mapper.Map(entity, viewModel);
                break;
        }
    }

    private void RemoveUnusedKeys(IEnumerable<Guid> usedKeys)
    {
        _logger.LogTrace("Cleaning unused keys from cache");
        var unusedKeys = _cache.Keys.Except(usedKeys).ToList();
        foreach (var key in unusedKeys)
        {
            _cache.Remove(key);
        }
    }

    private T Cache<T>(T viewModel) where T : notnull
    {
        var key = CacheIfNotCached(viewModel);
        return (T)_cache[key];
    }

    private Guid CacheIfNotCached<T>(T viewModel) where T : notnull
    {
        var key = GetKeyFromViewModel(viewModel);
        if (_cache.TryAdd(key, viewModel)) // viewModel is checked for null in GetKeyFromViewModel
            _logger.LogTrace("Cached {ViewModel} with key {Key}", viewModel.GetType().Name, key);

        return key;
    }

    private Guid GetKeyFromViewModel(object? viewModel)
    {
        return viewModel switch
        {
            LearningWorldViewModel vM => vM.Id,
            LearningSpaceViewModel vM => vM.Id,
            PathWayConditionViewModel vM => vM.Id,
            LearningElementViewModel vM => vM.Id,
            TopicViewModel vM => vM.Id,
            null => throw new ArgumentException("ViewModel is null"),
            _ => throw new NotImplementedException(
                $"ViewModel type '{viewModel.GetType()}' is not implemented in CachingMapper.")
        };
    }

    private T Get<T>(Guid id)
    {
        if (_cache.TryGetValue(id, out var value))
            return (T)value;

        throw new ApplicationException(
            "No cached object found. Check if the object is cached before calling this method.");
    }

    private void MapInternal(AuthoringToolWorkspace authoringToolWorkspaceEntity,
        IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
    {
        var newLearningWorldsInEntity = authoringToolWorkspaceEntity.LearningWorlds
            .FindAll(p => authoringToolWorkspaceVm.LearningWorlds.All(l => p.Id != l.Id));
        foreach (var w in newLearningWorldsInEntity.Where(w => _cache.ContainsKey(w.Id)))
        {
            authoringToolWorkspaceVm.LearningWorlds.Add(Get<LearningWorldViewModel>(w.Id));
        }

        _mapper.Map(authoringToolWorkspaceEntity, authoringToolWorkspaceVm);

        CacheAuthoringToolWorkspaceContent(authoringToolWorkspaceVm);
    }

    /// <summary>
    /// Maps the data from the learning world entity to the learning world view model,
    /// including caching and updating of new topics, learning spaces, pathway conditions,
    /// and unplaced learning elements.
    /// </summary>
    /// <param name="learningWorldEntity">The source learning world entity.</param>
    /// <param name="learningWorldVm">The target learning world view model.</param>
    private void MapInternal(LearningWorld learningWorldEntity, ILearningWorldViewModel learningWorldVm)
    {
        learningWorldVm = Cache(learningWorldVm);

        AddNewTopicsToLearningWorldVm(learningWorldEntity, learningWorldVm);
        AddNewLearningSpacesToLearningWorldVm(learningWorldEntity, learningWorldVm);
        AddNewPathWayConditionsToLearningWorldVm(learningWorldEntity, learningWorldVm);
        AddNewUnplacedLearningElementsToLearningWorldVm(learningWorldEntity, learningWorldVm);
        UpdateLearningSpacesInLearningWorldVm(learningWorldEntity, learningWorldVm);

        _mapper.Map(learningWorldEntity, learningWorldVm);

        CacheLearningWorldContent(learningWorldVm);

        AssignTopicsToNewLearningSpacesInViewModel(learningWorldEntity, learningWorldVm);
    }

    /// <summary>
    /// Adds new topics from the learning world entity to the learning world view model,
    /// ensuring that only topics not already present in the view model are added and
    /// checking the cache for existing topic view models.
    /// </summary>
    /// <param name="learningWorldEntity">The source learning world entity containing topics.</param>
    /// <param name="learningWorldVm">The target learning world view model to update with new topics.</param>
    private void AddNewTopicsToLearningWorldVm(LearningWorld learningWorldEntity,
        ILearningWorldViewModel learningWorldVm)
    {
        var newTopicsInEntity = learningWorldEntity.Topics
            .FindAll(p => learningWorldVm.Topics.All(l => p.Id != l.Id));
        foreach (var s in newTopicsInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.Topics.Add(Get<TopicViewModel>(s.Id));
        }
    }

    /// <summary>
    /// Adds new learning spaces from the learning world entity to the learning world view model,
    /// ensuring that only learning spaces not already present in the view model are added and
    /// checking the cache for existing learning space view models.
    /// </summary>
    /// <param name="learningWorldEntity">The source learning world entity containing learning spaces.</param>
    /// <param name="learningWorldVm">The target learning world view model to update with new learning spaces.</param>
    private void AddNewLearningSpacesToLearningWorldVm(LearningWorld learningWorldEntity,
        ILearningWorldViewModel learningWorldVm)
    {
        var newLearningSpacesInEntity = learningWorldEntity.LearningSpaces
            .FindAll(p => learningWorldVm.LearningSpaces.All(l => p.Id != l.Id));
        foreach (var s in newLearningSpacesInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.LearningSpaces.Add(Get<LearningSpaceViewModel>(s.Id));
        }
    }

    /// <summary>
    /// Adds new pathway conditions from the learning world entity to the learning world view model,
    /// ensuring that only pathway conditions not already present in the view model are added and
    /// checking the cache for existing pathway condition view models.
    /// </summary>
    /// <param name="learningWorldEntity">The source learning world entity containing pathway conditions.</param>
    /// <param name="learningWorldVm">The target learning world view model to update with new pathway conditions.</param>
    private void AddNewPathWayConditionsToLearningWorldVm(LearningWorld learningWorldEntity,
        ILearningWorldViewModel learningWorldVm)
    {
        var newPathWayConditionsInEntity = learningWorldEntity.PathWayConditions
            .FindAll(p => learningWorldVm.PathWayConditions.All(l => p.Id != l.Id));
        foreach (var s in newPathWayConditionsInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.PathWayConditions.Add(Get<PathWayConditionViewModel>(s.Id));
        }
    }

    /// <summary>
    /// Adds new unplaced learning elements from the learning world entity to the learning world view model,
    /// ensuring that only unplaced learning elements not already present in the view model are added and
    /// checking the cache for existing learning element view models.
    /// </summary>
    /// <param name="learningWorldEntity">The source learning world entity containing unplaced learning elements.</param>
    /// <param name="learningWorldVm">The target learning world view model to update with new unplaced learning elements.</param>
    private void AddNewUnplacedLearningElementsToLearningWorldVm(LearningWorld learningWorldEntity,
        ILearningWorldViewModel learningWorldVm)
    {
        var newUnplacedLearningElementsInEntity = learningWorldEntity.UnplacedLearningElements.ToList()
            .FindAll(p => learningWorldVm.UnplacedLearningElements.All(l => p.Id != l.Id));
        foreach (var s in newUnplacedLearningElementsInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.UnplacedLearningElements.Add(Get<LearningElementViewModel>(s.Id));
        }
    }

    /// <summary>
    /// Updates the learning spaces in the learning world view model based on the learning world entity,
    /// adding new learning elements and story elements from the entity to the corresponding learning spaces
    /// in the view model if they are not already present and checking the cache for existing view models.
    /// </summary>
    /// <param name="learningWorldEntity">The source learning world entity containing learning spaces.</param>
    /// <param name="learningWorldVm">The target learning world view model to update with new elements in learning spaces.</param>
    private void UpdateLearningSpacesInLearningWorldVm(LearningWorld learningWorldEntity,
        ILearningWorldViewModel learningWorldVm)
    {
        foreach (var learningSpaceEntity in learningWorldEntity.LearningSpaces)
        {
            var newLearningElementsInSpaceEntity = learningSpaceEntity.LearningSpaceLayout.LearningElements
                .Where(kvP =>
                    learningWorldVm.LearningSpaces.First(s => s.Id == learningSpaceEntity.Id).ContainedLearningElements
                        .All(l => kvP.Value.Id != l.Id));
            var newStoryElementsInSpaceEntity = learningSpaceEntity.LearningSpaceLayout.StoryElements
                .Where(kvP =>
                    learningWorldVm.LearningSpaces.First(s => s.Id == learningSpaceEntity.Id).LearningSpaceLayout
                        .StoryElements.All(l => kvP.Value.Id != l.Value.Id));
            //for all elements in entity that are not in view model, check cache and insert to view model
            foreach (var e in newLearningElementsInSpaceEntity.Where(e => _cache.ContainsKey(e.Value.Id)))
            {
                learningWorldVm.LearningSpaces.First(s => s.Id == learningSpaceEntity.Id).LearningSpaceLayout
                    .PutElement(e.Key, Get<LearningElementViewModel>(e.Value.Id));
            }

            foreach (var e in newStoryElementsInSpaceEntity.Where(e => _cache.ContainsKey(e.Value.Id)))
            {
                learningWorldVm.LearningSpaces.First(s => s.Id == learningSpaceEntity.Id).LearningSpaceLayout
                    .PutStoryElement(e.Key, Get<LearningElementViewModel>(e.Value.Id));
            }
        }
    }

    /// <summary>
    /// Assigns topics to newly added learning spaces in the learning world view model,
    /// ensuring that the assigned topics are retrieved from the cache if they exist.
    /// </summary>
    /// <param name="learningWorldEntity">The source learning world entity containing learning spaces.</param>
    /// <param name="learningWorldVm">The target learning world view model to update with assigned topics in learning spaces.</param>
    private void AssignTopicsToNewLearningSpacesInViewModel(LearningWorld learningWorldEntity,
        ILearningWorldViewModel learningWorldVm)
    {
        var newLearningSpacesInViewModel = from space in learningWorldEntity.LearningSpaces
            from spaceVm in learningWorldVm.LearningSpaces
            where space.Id == spaceVm.Id
            select spaceVm;

        foreach (var space in newLearningSpacesInViewModel)
        {
            if (space.AssignedTopic != null && _cache.ContainsKey(space.AssignedTopic.Id))
            {
                space.AssignedTopic = Get<TopicViewModel>(space.AssignedTopic.Id);
            }
        }
    }

    /// <summary>
    /// Maps from space entity to space view model.
    /// </summary>
    /// <param name="learningSpaceEntity">The entity from which we should map.</param>
    /// <param name="learningSpaceVm">The view model we should map into.</param>
    private void MapInternal(LearningSpace learningSpaceEntity, ILearningSpaceViewModel learningSpaceVm)
    {
        learningSpaceVm = Cache(learningSpaceVm);
        //get all elements in entity that are not in view model
        var newLearningElementsInEntity = learningSpaceEntity.LearningSpaceLayout.LearningElements
            .Where(kvP => !SameIdAtSameIndex(learningSpaceVm.LearningSpaceLayout, kvP));
        //for all elements in entity that are not in view model, check cache and insert to view model
        foreach (var e in newLearningElementsInEntity.Where(e => _cache.ContainsKey(e.Value.Id)))
        {
            learningSpaceVm.LearningSpaceLayout.PutElement(e.Key, Get<LearningElementViewModel>(e.Value.Id));
        }

        //call automapper
        _mapper.Map(learningSpaceEntity, learningSpaceVm);

        CacheLearningSpaceContent(learningSpaceVm);

        if (learningSpaceVm.AssignedTopic != null && _cache.ContainsKey(learningSpaceVm.AssignedTopic.Id))
            learningSpaceVm.AssignedTopic = Get<TopicViewModel>(learningSpaceVm.AssignedTopic.Id);
    }

    private void MapInternal(Topic topic, TopicViewModel topicVm)
    {
        topicVm = Cache(topicVm);
        _mapper.Map(topic, topicVm);
    }

    private static bool SameIdAtSameIndex(ILearningSpaceLayoutViewModel destination,
        KeyValuePair<int, ILearningElement> kvp) =>
        destination.LearningElements.Any(y => y.Key == kvp.Key && y.Value.Id == kvp.Value.Id);

    private void CacheAuthoringToolWorkspaceContent(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
    {
        foreach (var learningWorldVm in authoringToolWorkspaceVm.LearningWorlds)
        {
            CacheIfNotCached(learningWorldVm);
            CacheLearningWorldContent(learningWorldVm);
        }
    }

    private void CacheLearningWorldContent(ILearningWorldViewModel learningWorldVm)
    {
        foreach (var topicVm in learningWorldVm.Topics)
        {
            CacheIfNotCached(topicVm);
        }

        foreach (var spaceVm in learningWorldVm.LearningSpaces)
        {
            CacheIfNotCached(spaceVm);
            CacheLearningSpaceContent(spaceVm);
        }

        foreach (var conditionVm in learningWorldVm.PathWayConditions)
        {
            CacheIfNotCached(conditionVm);
        }

        foreach (var elementVm in learningWorldVm.UnplacedLearningElements)
        {
            CacheIfNotCached(elementVm);
        }
    }

    private void CacheLearningSpaceContent(ILearningSpaceViewModel learningSpaceVm)
    {
        foreach (var elementVm in learningSpaceVm.ContainedLearningElements)
        {
            CacheIfNotCached(elementVm);
        }
    }

    private void OnRemovedCommandsFromStacks(object sender,
        RemoveCommandsFromStacksEventArgs removeCommandsFromStacksEventArgs)
    {
        RemoveUnusedKeys(GetKeysFromObjects(removeCommandsFromStacksEventArgs.ObjectsInStacks));
    }

    private static IEnumerable<Guid> GetKeysFromObjects(IEnumerable<object> objects)
    {
        return objects.Select(o =>
        {
            return o switch
            {
                LearningWorld e => e.Id,
                Topic e => e.Id,
                LearningSpace e => e.Id,
                PathWayCondition e => e.Id,
                LearningElement e => e.Id,
                _ => throw new ArgumentException("Object is neither a World, Topic, Space, Condition or an Element")
            };
        });
    }
}