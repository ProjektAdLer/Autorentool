using AuthoringTool;
using AuthoringTool.Mapping;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.Space.SpaceLayout;
using Presentation.PresentationLogic.World;
using Shared;

namespace AuthoringToolTest.Mapping;

[TestFixture]
public class CachingMapperUt
{
    /*
switch (entity, viewModel)
{
    case (AuthoringToolWorkspace s, IAuthoringToolWorkspaceViewModel d):
        Map(s, d);
        break;
    case (World s, IWorldViewModel d):
        Map(s, d);
        break;
    case (Space s, ISpaceViewModel d):
        Map(s, d);
        break;
    case (Element s, IElementViewModel d):
        Map(s, d);
        break;
    default:
        _mapper.Map(entity, viewModel);
        break;
}
*/

    
    [Test]
    public void MapAuthoringToolWorkspaceEntityToViewModel_MapperReceivedCallWithCorrectParameters()
    {
        var entity = new AuthoringToolWorkspace(null, new List<World>());
        var mockViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var commandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateTestableCachingMapper(mapper, commandStateManager, logger);

        systemUnderTest.Map(entity, mockViewModel);

        mapper.Received(1).Map(entity, mockViewModel);
    }
    
    [Test]
    public void MapAuthoringToolWorkspaceEntityToViewModel_MapsWorldToViewModel()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var workspaceViewModel = new AuthoringToolWorkspaceViewModel();
        var worldEntity = new World("n", "s", "a", "l", "d", "g");
        workspace.Worlds.Add(worldEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(workspace, workspaceViewModel);

        Assert.That(workspaceViewModel.Worlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceViewModel.Worlds[0].Id, Is.EqualTo(worldEntity.Id));
            Assert.That(workspaceViewModel.Worlds[0].Name, Is.EqualTo(worldEntity.Name));
            Assert.That(workspaceViewModel.Worlds[0].Shortname, Is.EqualTo(worldEntity.Shortname));
            Assert.That(workspaceViewModel.Worlds[0].Authors, Is.EqualTo(worldEntity.Authors));
            Assert.That(workspaceViewModel.Worlds[0].Language, Is.EqualTo(worldEntity.Language));
            Assert.That(workspaceViewModel.Worlds[0].Description, Is.EqualTo(worldEntity.Description));
            Assert.That(workspaceViewModel.Worlds[0].Goals, Is.EqualTo(worldEntity.Goals));
        });
    }
    
    [Test]
    public void MapAuthoringToolWorkspaceEntityToViewModel_MapsWorldToTheSameViewModelAfterFirstCall()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var workspaceViewModel = new AuthoringToolWorkspaceViewModel();
        var worldEntity = new World("n", "s", "a", "l", "d", "g");
        workspace.Worlds.Add(worldEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(workspace, workspaceViewModel);

        Assert.That(workspaceViewModel.Worlds, Has.Count.EqualTo(1));
        Assert.That(workspaceViewModel.Worlds[0].Id, Is.EqualTo(worldEntity.Id));
        
        var worldViewModel = workspaceViewModel.Worlds[0];
        workspaceViewModel.Worlds.Clear();
        Assert.That(workspaceViewModel.Worlds, Has.Count.EqualTo(0));
        
        systemUnderTest.Map(workspace, workspaceViewModel);
        
        Assert.That(workspaceViewModel.Worlds, Has.Count.EqualTo(1));
        Assert.That(workspaceViewModel.Worlds[0], Is.EqualTo(worldViewModel));
    }
    
    [Test]
    public void MapWorldEntityToViewModel_MapperReceivedCallWithCorrectParameters()
    {
        var entity = new World("n","s","a","l","d","g");
       var viewModel = new WorldViewModel("x","x","x","x","x","x");
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(entity, viewModel);

        mapper.Received(1).Map(entity, (IWorldViewModel)viewModel);
    }
    
    [Test]
    public void MapWorldEntityToViewModel_MapsSpaceConditionAndPathWayToViewModel()
    {
        var world = new World("n","s","a","l","d","g");
        var worldViewModel = new WorldViewModel("x","x","x","x","x","x");
        var spaceEntity = new Space("n", "s", "a", "d", "g", 5);
        var conditionEntity = new PathWayCondition(ConditionEnum.And, 2, 1);
        var pathWayEntity = new Pathway(spaceEntity,conditionEntity);
        world.Spaces.Add(spaceEntity);
        world.PathWayConditions.Add(conditionEntity);
        world.Pathways.Add(pathWayEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(world, worldViewModel);

        Assert.That(worldViewModel.Spaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWays, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(worldViewModel.Spaces.First().Id, Is.EqualTo(spaceEntity.Id));
            Assert.That(worldViewModel.Spaces.First().Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(worldViewModel.Spaces.First().Shortname, Is.EqualTo(spaceEntity.Shortname));
            Assert.That(worldViewModel.Spaces.First().Authors, Is.EqualTo(spaceEntity.Authors));
            Assert.That(worldViewModel.Spaces.First().Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(worldViewModel.Spaces.First().Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(worldViewModel.Spaces.First().RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
        });
        Assert.Multiple(() =>
        {
            Assert.That(worldViewModel.PathWayConditions.First().Id, Is.EqualTo(conditionEntity.Id));    
            Assert.That(worldViewModel.PathWayConditions.First().Condition, Is.EqualTo(conditionEntity.Condition));
        });
        Assert.Multiple(() =>
        {
            Assert.That(worldViewModel.PathWays.First().Id, Is.EqualTo(pathWayEntity.Id));
            Assert.That(worldViewModel.PathWays.First().SourceObject.Id, Is.EqualTo(spaceEntity.Id));
            Assert.That(worldViewModel.PathWays.First().TargetObject.Id, Is.EqualTo(conditionEntity.Id));
        });
    }
    
    [Test]
    public void MapWorldEntityToViewModel_MapsSpaceConditionAndPathWayToTheSameViewModelAfterFirstCall()
    {
        var world = new World("n","s","a","l","d","g");
        var worldViewModel = new WorldViewModel("x","x","x","x","x","x");
        var spaceEntity = new Space("n", "s", "a", "d", "g", 5);
        var conditionEntity = new PathWayCondition(ConditionEnum.And, 2, 1);
        var pathWayEntity = new Pathway(spaceEntity,conditionEntity);
        world.Spaces.Add(spaceEntity);
        world.PathWayConditions.Add(conditionEntity);
        world.Pathways.Add(pathWayEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(world, worldViewModel);

        Assert.That(worldViewModel.Spaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWays, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.Spaces.First().Id, Is.EqualTo(spaceEntity.Id));
        Assert.That(worldViewModel.PathWayConditions.First().Id, Is.EqualTo(conditionEntity.Id));
        Assert.That(worldViewModel.PathWays.First().Id, Is.EqualTo(pathWayEntity.Id));
        
        var spaceViewModel = worldViewModel.Spaces.First();
        var conditionViewModel = worldViewModel.PathWayConditions.First();
        var pathWayViewModel = worldViewModel.PathWays.First();
        worldViewModel.Spaces.Clear();
        worldViewModel.PathWayConditions.Clear();
        worldViewModel.PathWays.Clear();
        Assert.That(worldViewModel.Spaces, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.PathWays, Has.Count.EqualTo(0));
        
        systemUnderTest.Map(world, worldViewModel);
        
        Assert.That(worldViewModel.Spaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWays, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.Spaces.First(), Is.EqualTo(spaceViewModel));
        Assert.That(worldViewModel.PathWayConditions.First(), Is.EqualTo(conditionViewModel));
    }

    [Test]
    public void
        MapSpaceEntityToViewModel_ChangedLayoutWithElementWithIndexBiggerThanTheOldElementsArray_MapsCorrectly()
    {
        var spaceEntity = new Space("n", "s", "a", "d", "g", 5, new SpaceLayout(new IElement?[4], FloorPlanEnum.Rectangle2X2));
        var elementEntity = new Element("n", "s", new Content("n", "t", "f"), "u", "a", "d", "g", ElementDifficultyEnum.Easy, spaceEntity);
        spaceEntity.SpaceLayout.Elements[3] = elementEntity;
        
        var systemUnderTest = CreateTestableCachingMapper();
        
        var spaceViewModel = new SpaceViewModel("", "", "", "", "", 0, new SpaceLayoutViewModel());
        
        systemUnderTest.Map<Space, SpaceViewModel>(spaceEntity, spaceViewModel);
        
        Assert.That(spaceViewModel.SpaceLayout.Elements, Has.Length.EqualTo(4));
        Assert.That(spaceViewModel.SpaceLayout.Elements[3], Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.SpaceLayout.Elements[3]!.Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(spaceViewModel.Shortname, Is.EqualTo(spaceEntity.Shortname));
            Assert.That(spaceViewModel.Authors, Is.EqualTo(spaceEntity.Authors));
            Assert.That(spaceViewModel.Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(spaceViewModel.Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(spaceViewModel.RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
        });

        spaceEntity.SpaceLayout = new SpaceLayout(new IElement[6], FloorPlanEnum.Rectangle2X3);
        spaceEntity.SpaceLayout.Elements[5] = elementEntity;

        systemUnderTest.Map<Space, SpaceViewModel>(spaceEntity, spaceViewModel);
        
        Assert.That(spaceViewModel.SpaceLayout.Elements, Has.Length.EqualTo(6));
        Assert.That(spaceViewModel.SpaceLayout.Elements[5], Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.SpaceLayout.Elements[5]!.Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(spaceViewModel.Shortname, Is.EqualTo(spaceEntity.Shortname));
            Assert.That(spaceViewModel.Authors, Is.EqualTo(spaceEntity.Authors));
            Assert.That(spaceViewModel.Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(spaceViewModel.Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(spaceViewModel.RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
        });
    }

    [Test]
    public void MapSpaceEntityToViewModel_MapperReceivedCallWithCorrectParameters()
    {
        var entity = new Space("n", "s", "a", "d", "g", 5);
        var viewModel = new SpaceViewModel("x","x","x","x","x",5);
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(entity, viewModel);

        mapper.Received(1).Map(entity, (ISpaceViewModel)viewModel);
    }
    
    [Test]
    public void MapSpaceEntityToViewModel_MapsElementToViewModel()
    {
        var space = new Space("n", "s", "a", "d", "g", 5, new SpaceLayout(new IElement[6], FloorPlanEnum.Rectangle2X3));
        var spaceViewModel = new SpaceViewModel("x","x","x","x","x",5);
        var elementEntity = new Element("n", "s", null!,"u","a", "d", "g", ElementDifficultyEnum.Easy);
        space.SpaceLayout.Elements[0] = elementEntity;
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(space, spaceViewModel);

        Assert.That(spaceViewModel.ContainedElements.Count(), Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.ContainedElements.First().Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.ContainedElements.First().Name, Is.EqualTo(elementEntity.Name));
            Assert.That(spaceViewModel.ContainedElements.First().Shortname, Is.EqualTo(elementEntity.Shortname));
            Assert.That(spaceViewModel.ContainedElements.First().Url, Is.EqualTo(elementEntity.Url));
            Assert.That(spaceViewModel.ContainedElements.First().Authors, Is.EqualTo(elementEntity.Authors));
            Assert.That(spaceViewModel.ContainedElements.First().Description, Is.EqualTo(elementEntity.Description));
            Assert.That(spaceViewModel.ContainedElements.First().Goals, Is.EqualTo(elementEntity.Goals));
            Assert.That(spaceViewModel.ContainedElements.First().Difficulty, Is.EqualTo(elementEntity.Difficulty));
        });
    }
    
    [Test]
    public void MapSpaceEntityToViewModel_MapsElementToTheSameViewModelAfterFirstCall()
    {
        var space = new Space("n", "s", "a", "d", "g", 5,
            new SpaceLayout(new IElement[6], FloorPlanEnum.Rectangle2X3));
        var spaceViewModel = new SpaceViewModel("x", "x", "x", "x", "x", 5,
            new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var elementEntity =
            new Element("n", "s", null!, "u", "a", "d", "g", ElementDifficultyEnum.Easy);
        space.SpaceLayout.Elements[0] = elementEntity;
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(space, spaceViewModel);

        Assert.That(spaceViewModel.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(spaceViewModel.ContainedElements.First().Id, Is.EqualTo(elementEntity.Id));
        
        var elementViewModel = spaceViewModel.ContainedElements.First();
        spaceViewModel.SpaceLayout.ClearAllElements();
        Assert.That(spaceViewModel.ContainedElements.Count(), Is.EqualTo(0));
        
        space.ContainedElements.First().Name = "newName";
        systemUnderTest.Map(space, spaceViewModel);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.ContainedElements.First().Name, Is.EqualTo("newName"));
            Assert.That(spaceViewModel.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(spaceViewModel.ContainedElements.First().Id, Is.EqualTo(elementViewModel.Id));
            Assert.That(spaceViewModel.ContainedElements.First().Name, Is.EqualTo(elementViewModel.Name));
            Assert.That(spaceViewModel.ContainedElements.First(), Is.EqualTo(elementViewModel));
            Assert.That(spaceViewModel.ContainedElements.First(), Is.EqualTo(systemUnderTest.ReadOnlyCache[elementEntity.Id]));
            Assert.That(elementViewModel, Is.EqualTo(systemUnderTest.ReadOnlyCache[elementEntity.Id]));
        });
    }
    
    [Test]
    public void OnRemovedCommandsFromStacksInvoked_UnusedViewModelsAreRemoved()
    {
        var worldEntity = new World("n","s","a","l","d","g");
        var workspace = new AuthoringToolWorkspace(worldEntity, new List<World>(){worldEntity});
        var workspaceViewModel = new AuthoringToolWorkspaceViewModel();
        var spaceEntity = new Space("n", "s", "a", "d", "g", 5, new SpaceLayout(new IElement?[6], FloorPlanEnum.Rectangle2X3));
        var elementEntity = new Element("n", "s", null!,"u","a", "d", "g", ElementDifficultyEnum.Easy);
        var secondElementEntity = new Element("n2", "s2", null!,"u2","a2", "d2", "g2", ElementDifficultyEnum.Easy);
        
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateTestableCachingMapper(commandStateManager: mockCommandStateManager);

        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
        systemUnderTest.Map(workspace, workspaceViewModel);
        
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(1));
        
        worldEntity.Spaces.Add(spaceEntity);
        var worldViewModel = workspaceViewModel.Worlds.First();
        systemUnderTest.Map(worldEntity, worldViewModel);
        
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(2));
        
        spaceEntity.SpaceLayout.Elements[0] = elementEntity;
        var spaceViewModel = worldViewModel.Spaces.First();
        systemUnderTest.Map(spaceEntity, spaceViewModel);
        
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(3));
        
        spaceEntity.SpaceLayout.Elements[1] = secondElementEntity;
        systemUnderTest.Map(spaceEntity, spaceViewModel);
        
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(4));
        
        var objectList = new List<object> {worldEntity, spaceEntity, elementEntity};
        mockCommandStateManager.RemovedCommandsFromStacks +=
            Raise.Event<CommandStateManager.RemovedCommandsFromStacksHandler>(mockCommandStateManager, new RemoveCommandsFromStacksEventArgs(objectList));

        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(3));
    }

    [Test]
    public void MapSomethingOtherThanWorkspaceOrWorldOrSpace_DoesNotCache()
    {
        var elementEntity = new Element("n", "s", null!,"u","a", "d", "g", ElementDifficultyEnum.Easy);
        var elementViewModel = new ElementViewModel("x","x",null!,"x","x","x","x",ElementDifficultyEnum.Easy);
        
        var systemUnderTest = CreateTestableCachingMapper();

        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
        systemUnderTest.Map(elementEntity, elementViewModel);
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void MapSomethingOtherThanWorkspaceOrWorldOrSpace_CallsMapper(){
        var elementEntity = new Element("n", "s", null!,"u","a", "d", "g", ElementDifficultyEnum.Easy);
        var elementViewModel = new ElementViewModel("x","x",null!,"x","x","x","x",ElementDifficultyEnum.Easy);
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(elementEntity, elementViewModel);
        mapper.Received(1).Map(elementEntity, elementViewModel);
    }

    private static CachingMapper CreateTestableCachingMapper(
        IMapper? mapper = null, ICommandStateManager? commandStateManager = null, ILogger<CachingMapper>? logger = null)
    {
        var config = new MapperConfiguration(cfg=>
        {
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappers();
        });
        mapper ??= config.CreateMapper();
        commandStateManager ??= Substitute.For<ICommandStateManager>();
        logger ??= Substitute.For<ILogger<CachingMapper>>();

        return new CachingMapper(mapper, commandStateManager, logger);
    }
}