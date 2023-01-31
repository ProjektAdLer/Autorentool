using AutoMapper;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
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
                MapInternal(s, d);
                break;
            case (LearningWorld s, ILearningWorldViewModel d):
                MapInternal(s, d);
                break;
            case (LearningSpace s, ILearningSpaceViewModel d):
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
        if (viewModel == null){
            throw new ArgumentException("ViewModel is null");
        }
        Guid key = default;
        switch (viewModel)
        {
            case LearningWorldViewModel vM:
                key = vM.Id;
                break;
            case LearningSpaceViewModel vM:
                key = vM.Id;
                break;
            case PathWayConditionViewModel vM:
                key = vM.Id;
                break;
            case LearningElementViewModel vM:
                key = vM.Id;
                break;
        }
        if (!_cache.ContainsKey(key))
        {
            _cache[key] = viewModel;
        }

        return (T)_cache[key];
    }
    
    private T Get<T>(Guid id)
    {
        if (_cache.ContainsKey(id))
        {
            return (T)_cache[id];
        }
        throw new ApplicationException("No cached object found. Check if the object is cached before calling this method.");
    }

    private void MapInternal(AuthoringToolWorkspace authoringToolWorkspaceEntity,
        IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
    {
        var nLearningWorlds = authoringToolWorkspaceEntity.LearningWorlds
            .FindAll(p=>authoringToolWorkspaceVm.LearningWorlds.All(l => p.Id != l.Id));
        foreach (var w in nLearningWorlds)
        {
            if(_cache.ContainsKey(w.Id)) authoringToolWorkspaceVm.LearningWorlds.Add(Get<LearningWorldViewModel>(w.Id));
        }
        _mapper.Map(authoringToolWorkspaceEntity, authoringToolWorkspaceVm);
        foreach (var worldVm in authoringToolWorkspaceVm.LearningWorlds.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(worldVm);
        }
    }

    private void MapInternal(LearningWorld learningWorldEntity, ILearningWorldViewModel learningWorldVm)
    {
        learningWorldVm = Cache(learningWorldVm);
        var newLearningSpacesInEntity = learningWorldEntity.LearningSpaces
            .FindAll(p => learningWorldVm.LearningSpaces.All(l => p.Id != l.Id));
        foreach (var s in newLearningSpacesInEntity)
        {
            if(_cache.ContainsKey(s.Id)) learningWorldVm.LearningSpaces.Add(Get<LearningSpaceViewModel>(s.Id));
        }
        var newPathWayConditionInEntity = learningWorldEntity.PathWayConditions
            .FindAll(p => learningWorldVm.PathWayConditions.All(l => p.Id != l.Id));
        foreach (var s in newPathWayConditionInEntity)
        {
            if(_cache.ContainsKey(s.Id)) learningWorldVm.PathWayConditions.Add(Get<PathWayConditionViewModel>(s.Id));
        }
        _mapper.Map(learningWorldEntity, learningWorldVm);
        foreach (var conditionVm in learningWorldVm.PathWayConditions.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(conditionVm);
        }
        foreach (var spaceVm in learningWorldVm.LearningSpaces.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(spaceVm);
        }
    }

    private void MapInternal(LearningSpace learningSpaceEntity, ILearningSpaceViewModel learningSpaceVm)
    {
        learningSpaceVm = Cache(learningSpaceVm);
        //var newLearningElementsInEntityO = learningSpaceEntity.LearningSpaceLayout.LearningElements.Where((p, i) => learningSpaceVm.LearningSpaceLayoutViewModel.LearningElements.All(l => p?.Id != l?.Id));
        //TODO: Check if this works - AW
        var newLearningElementsInEntity = learningSpaceEntity.LearningSpaceLayout.LearningElements
            .Select((s,i)=>new {i,s})
            .Where(x => x.s != null)
            .Where((p, x) => learningSpaceVm.ContainedLearningElements.All(l => p.s!.Id != l.Id)).ToList();
        if (newLearningElementsInEntity.Any())
        {
            var max = newLearningElementsInEntity.Max(x => x.i);
            if (max >= learningSpaceVm.LearningSpaceLayout.LearningElements.Length)
            {
                var newLearningElementsVm = new ILearningElementViewModel?[max + 1];
                foreach (var (v, i) in learningSpaceVm.LearningSpaceLayout.LearningElements.Select((v,i)=>(v,i)))
                {
                    newLearningElementsVm[i] = v;
                }
                learningSpaceVm.LearningSpaceLayout.LearningElements = newLearningElementsVm;
            }
        }
        foreach (var e in newLearningElementsInEntity)
        {
            if (_cache.ContainsKey(e.s!.Id)) learningSpaceVm.LearningSpaceLayout.PutElement(e.i, Get<LearningElementViewModel>(e.s!.Id));
        }
        _mapper.Map(learningSpaceEntity, learningSpaceVm);
        foreach (var elementVm in learningSpaceVm.ContainedLearningElements.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(elementVm);
        }
    }

    private void OnRemovedCommandsFromStacks(object sender, RemoveCommandsFromStacksEventArgs removeCommandsFromStacksEventArgs)
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
                LearningSpace e => e.Id,
                PathWayCondition e => e.Id,
                LearningElement e => e.Id,
                _ => throw new ArgumentException("Object is neither a World, Space, Condition or an Element")
            };
        });
    }
}