using AutoMapper;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
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
            case (World s, IWorldViewModel d):
                MapInternal(s, d);
                break;
            case (Space s, ISpaceViewModel d):
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
            case WorldViewModel vM:
                key = vM.Id;
                break;
            case SpaceViewModel vM:
                key = vM.Id;
                break;
            case PathWayConditionViewModel vM:
                key = vM.Id;
                break;
            case ElementViewModel vM:
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
        var nWorlds = authoringToolWorkspaceEntity.Worlds
            .FindAll(p=>authoringToolWorkspaceVm.Worlds.All(l => p.Id != l.Id));
        foreach (var w in nWorlds)
        {
            if(_cache.ContainsKey(w.Id)) authoringToolWorkspaceVm.Worlds.Add(Get<WorldViewModel>(w.Id));
        }
        _mapper.Map(authoringToolWorkspaceEntity, authoringToolWorkspaceVm);
        foreach (var worldVm in authoringToolWorkspaceVm.Worlds.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(worldVm);
        }
    }

    private void MapInternal(World worldEntity, IWorldViewModel worldVm)
    {
        worldVm = Cache(worldVm);
        var newSpacesInEntity = worldEntity.Spaces
            .FindAll(p => worldVm.Spaces.All(l => p.Id != l.Id));
        foreach (var s in newSpacesInEntity)
        {
            if(_cache.ContainsKey(s.Id)) worldVm.Spaces.Add(Get<SpaceViewModel>(s.Id));
        }
        var newPathWayConditionInEntity = worldEntity.PathWayConditions
            .FindAll(p => worldVm.PathWayConditions.All(l => p.Id != l.Id));
        foreach (var s in newPathWayConditionInEntity)
        {
            if(_cache.ContainsKey(s.Id)) worldVm.PathWayConditions.Add(Get<PathWayConditionViewModel>(s.Id));
        }
        _mapper.Map(worldEntity, worldVm);
        foreach (var conditionVm in worldVm.PathWayConditions.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(conditionVm);
        }
        foreach (var spaceVm in worldVm.Spaces.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(spaceVm);
        }
    }

    private void MapInternal(Space spaceEntity, ISpaceViewModel spaceVm)
    {
        spaceVm = Cache(spaceVm);
        //var newElementsInEntityO = spaceEntity.SpaceLayout.Elements.Where((p, i) => spaceVm.SpaceLayoutViewModel.Elements.All(l => p?.Id != l?.Id));
        //TODO: Check if this works - AW
        var newElementsInEntity = spaceEntity.SpaceLayout.Elements
            .Select((s,i)=>new {i,s})
            .Where(x => x.s != null)
            .Where((p, x) => spaceVm.ContainedElements.All(l => p.s!.Id != l.Id)).ToList();
        if (newElementsInEntity.Any())
        {
            var max = newElementsInEntity.Max(x => x.i);
            if (max >= spaceVm.SpaceLayout.Elements.Length)
            {
                var newElementsVm = new IElementViewModel?[max + 1];
                foreach (var (v, i) in spaceVm.SpaceLayout.Elements.Select((v,i)=>(v,i)))
                {
                    newElementsVm[i] = v;
                }
                spaceVm.SpaceLayout.Elements = newElementsVm;
            }
        }
        foreach (var e in newElementsInEntity)
        {
            if (_cache.ContainsKey(e.s!.Id)) spaceVm.SpaceLayout.PutElement(e.i, Get<ElementViewModel>(e.s!.Id));
        }
        _mapper.Map(spaceEntity, spaceVm);
        foreach (var elementVm in spaceVm.ContainedElements.Where(w =>
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
                World e => e.Id,
                Space e => e.Id,
                PathWayCondition e => e.Id,
                Element e => e.Id,
                _ => throw new ArgumentException("Object is neither a World, Space, Condition or an Element")
            };
        });
    }
}