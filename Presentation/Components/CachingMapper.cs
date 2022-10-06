
using AutoMapper;
using BusinessLogic.Entities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.Components;

public class CachingMapper : ICachingMapper
{
    public CachingMapper(IMapper mapper)
    {
        _mapper = mapper;
        _cache = new Dictionary<Guid, object>();
    }
    
    private readonly IMapper _mapper;
    
    private Dictionary<Guid, object> _cache;

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
            case LearningElementViewModel vM:
                key = vM.Id;
                break;
            case AuthoringToolWorkspaceViewModel vM:
                key = new Guid("00000000-0000-0000-0000-000000000000");
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

    public void Map(AuthoringToolWorkspace authoringToolWorkspaceEntity,
        IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
    {
        authoringToolWorkspaceVm = Cache(authoringToolWorkspaceVm);
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

    public void Map(LearningWorld learningWorldEntity, ILearningWorldViewModel learningWorldVm)
    {
        learningWorldVm = Cache(learningWorldVm);
        var nLearningSpaces = learningWorldEntity.LearningSpaces
            .FindAll(p => learningWorldVm.LearningSpaces.All(l => p.Id != l.Id));
        foreach (var s in nLearningSpaces)
        {
            if(_cache.ContainsKey(s.Id)) learningWorldVm.LearningSpaces.Add(Get<LearningSpaceViewModel>(s.Id));
        }
        _mapper.Map(learningWorldEntity, learningWorldVm);
        foreach (var spaceVm in learningWorldVm.LearningSpaces.Where(w =>
                     !_cache.ContainsKey(w.Id)))
        {
            Cache(spaceVm);
        }
    }

    public void Map(LearningSpace learningSpaceEntity, ILearningSpaceViewModel learningSpaceVm)
    {
        learningSpaceVm = Cache(learningSpaceVm);
        _mapper.Map(learningSpaceEntity, learningSpaceVm);
    }

    public void Map(LearningElement learningElementEntity, ILearningElementViewModel learningElementVm)
    {
        learningElementVm = Cache(learningElementVm);
        _mapper.Map(learningElementEntity, learningElementVm);
    }
}