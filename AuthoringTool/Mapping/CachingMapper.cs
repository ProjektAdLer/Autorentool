﻿using AutoMapper;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;

namespace AuthoringTool.Mapping;

public class CachingMapper : ICachingMapper
{
    public CachingMapper(IMapper mapper, ICommandStateManager commandStateManager, ILogger<CachingMapper> logger)
    {
        _mapper = mapper;
        commandStateManager.RemovedCommandsFromStacks += OnRemovedCommandsFromStacks;
        _logger = logger;
        _cache = new Dictionary<Guid, object>();
    }

    private readonly IMapper _mapper;
    private readonly ILogger<CachingMapper> _logger;

    private readonly Dictionary<Guid, object> _cache;
    internal IReadOnlyDictionary<Guid, object> ReadOnlyCache => _cache;

    public void Map<TSource, TDestination>(TSource entity, TDestination viewModel)
    {
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
        var unusedKeys = _cache.Keys.Except(usedKeys).ToList();
        foreach (var key in unusedKeys)
        {
            _cache.Remove(key);
        }
    }

    private T Cache<T>(T viewModel)
    {
        var key = CacheIfNotCached(viewModel);
        return (T) _cache[key];
    }

    private Guid CacheIfNotCached<T>(T viewModel)
    {
        var key = GetKeyFromViewModel(viewModel);
        if (!_cache.ContainsKey(key))
        {
            _cache[key] = viewModel!; // viewModel is checked for null in GetKeyFromViewModel
        }

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
        if (_cache.ContainsKey(id))
        {
            return (T) _cache[id];
        }

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

    private void MapInternal(LearningWorld learningWorldEntity, ILearningWorldViewModel learningWorldVm)
    {
        learningWorldVm = Cache(learningWorldVm);

        var newTopicsInEntity = learningWorldEntity.Topics
            .FindAll(p => learningWorldVm.Topics.All(l => p.Id != l.Id));
        foreach (var s in newTopicsInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.Topics.Add(Get<TopicViewModel>(s.Id));
        }

        var newLearningSpacesInEntity = learningWorldEntity.LearningSpaces
            .FindAll(p => learningWorldVm.LearningSpaces.All(l => p.Id != l.Id));
        foreach (var s in newLearningSpacesInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.LearningSpaces.Add(Get<LearningSpaceViewModel>(s.Id));
        }

        var newPathWayConditionsInEntity = learningWorldEntity.PathWayConditions
            .FindAll(p => learningWorldVm.PathWayConditions.All(l => p.Id != l.Id));
        foreach (var s in newPathWayConditionsInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.PathWayConditions.Add(Get<PathWayConditionViewModel>(s.Id));
        }

        var newLearningElementsInEntity = learningWorldEntity.UnplacedLearningElements.ToList()
            .FindAll(p => learningWorldVm.UnplacedLearningElements.All(l => p.Id != l.Id));
        foreach (var s in newLearningElementsInEntity.Where(s => _cache.ContainsKey(s.Id)))
        {
            learningWorldVm.UnplacedLearningElements.Add(Get<LearningElementViewModel>(s.Id));
        }

        _mapper.Map(learningWorldEntity, learningWorldVm);

        CacheLearningWorldContent(learningWorldVm);

        var newLearningSpacesInViewModel = from space in newLearningSpacesInEntity
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
            .Where(kvP =>
                learningSpaceVm.ContainedLearningElements.All(l => kvP.Value.Id != l.Id));
        //for all elements in entity that are not in view model, check cache and insert to view model
        foreach (var e in newLearningElementsInEntity.Where(e => _cache.ContainsKey(e.Value!.Id)))
        {
            learningSpaceVm.LearningSpaceLayout.PutElement(e.Key, Get<LearningElementViewModel>(e.Value!.Id));
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