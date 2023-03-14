using AuthoringTool;
using AuthoringTool.Mapping;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
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
    case (LearningWorld s, ILearningWorldViewModel d):
        Map(s, d);
        break;
    case (LearningSpace s, ILearningSpaceViewModel d):
        Map(s, d);
        break;
    case (LearningElement s, ILearningElementViewModel d):
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
        var entity = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var mockViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var commandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateTestableCachingMapper(mapper, commandStateManager, logger);

        systemUnderTest.Map(entity, mockViewModel);

        mapper.Received(1).Map(entity, mockViewModel);
    }
    
    [Test]
    public void MapAuthoringToolWorkspaceEntityToViewModel_MapsLearningWorldToViewModel()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var workspaceViewModel = new AuthoringToolWorkspaceViewModel();
        var worldEntity = new LearningWorld("n", "s", "a", "l", "d", "g");
        workspace.LearningWorlds.Add(worldEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(workspace, workspaceViewModel);

        Assert.That(workspaceViewModel.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceViewModel.LearningWorlds[0].Id, Is.EqualTo(worldEntity.Id));
            Assert.That(workspaceViewModel.LearningWorlds[0].Name, Is.EqualTo(worldEntity.Name));
            Assert.That(workspaceViewModel.LearningWorlds[0].Shortname, Is.EqualTo(worldEntity.Shortname));
            Assert.That(workspaceViewModel.LearningWorlds[0].Authors, Is.EqualTo(worldEntity.Authors));
            Assert.That(workspaceViewModel.LearningWorlds[0].Language, Is.EqualTo(worldEntity.Language));
            Assert.That(workspaceViewModel.LearningWorlds[0].Description, Is.EqualTo(worldEntity.Description));
            Assert.That(workspaceViewModel.LearningWorlds[0].Goals, Is.EqualTo(worldEntity.Goals));
        });
    }
    
    [Test]
    public void MapAuthoringToolWorkspaceEntityToViewModel_MapsWorldToTheSameViewModelAfterFirstCall()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var workspaceViewModel = new AuthoringToolWorkspaceViewModel();
        var worldEntity = new LearningWorld("n", "s", "a", "l", "d", "g");
        workspace.LearningWorlds.Add(worldEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(workspace, workspaceViewModel);

        Assert.That(workspaceViewModel.LearningWorlds, Has.Count.EqualTo(1));
        Assert.That(workspaceViewModel.LearningWorlds[0].Id, Is.EqualTo(worldEntity.Id));
        
        var worldViewModel = workspaceViewModel.LearningWorlds[0];
        workspaceViewModel.LearningWorlds.Clear();
        Assert.That(workspaceViewModel.LearningWorlds, Has.Count.EqualTo(0));
        
        systemUnderTest.Map(workspace, workspaceViewModel);
        
        Assert.That(workspaceViewModel.LearningWorlds, Has.Count.EqualTo(1));
        Assert.That(workspaceViewModel.LearningWorlds[0], Is.EqualTo(worldViewModel));
    }
    
    [Test]
    public void MapLearningWorldEntityToViewModel_MapperReceivedCallWithCorrectParameters()
    {
        var entity = new LearningWorld("n","s","a","l","d","g");
       var viewModel = new LearningWorldViewModel("x","x","x","x","x","x");
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(entity, viewModel);

        mapper.Received(1).Map(entity, (ILearningWorldViewModel)viewModel);
    }
    
    [Test]
    public void MapLearningWorldEntityToViewModel_MapsLearningSpaceConditionAndPathWayToViewModel()
    {
        var world = new LearningWorld("n","s","a","l","d","g");
        var worldViewModel = new LearningWorldViewModel("x","x","x","x","x","x");
        var spaceEntity = new LearningSpace("n", "s", "a", "d", "g", 5);
        var conditionEntity = new PathWayCondition(ConditionEnum.And, 2, 1);
        var pathWayEntity = new LearningPathway(spaceEntity,conditionEntity);
        world.LearningSpaces.Add(spaceEntity);
        world.PathWayConditions.Add(conditionEntity);
        world.LearningPathways.Add(pathWayEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(world, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningPathWays, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(worldViewModel.LearningSpaces.First().Id, Is.EqualTo(spaceEntity.Id));
            Assert.That(worldViewModel.LearningSpaces.First().Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(worldViewModel.LearningSpaces.First().Shortname, Is.EqualTo(spaceEntity.Shortname));
            Assert.That(worldViewModel.LearningSpaces.First().Authors, Is.EqualTo(spaceEntity.Authors));
            Assert.That(worldViewModel.LearningSpaces.First().Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(worldViewModel.LearningSpaces.First().Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(worldViewModel.LearningSpaces.First().RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
        });
        Assert.Multiple(() =>
        {
            Assert.That(worldViewModel.PathWayConditions.First().Id, Is.EqualTo(conditionEntity.Id));    
            Assert.That(worldViewModel.PathWayConditions.First().Condition, Is.EqualTo(conditionEntity.Condition));
        });
        Assert.Multiple(() =>
        {
            Assert.That(worldViewModel.LearningPathWays.First().Id, Is.EqualTo(pathWayEntity.Id));
            Assert.That(worldViewModel.LearningPathWays.First().SourceObject.Id, Is.EqualTo(spaceEntity.Id));
            Assert.That(worldViewModel.LearningPathWays.First().TargetObject.Id, Is.EqualTo(conditionEntity.Id));
        });
    }
    
    [Test]
    public void MapLearningWorldEntityToViewModel_MapsSpaceConditionAndPathWayToTheSameViewModelAfterFirstCall()
    {
        var world = new LearningWorld("n","s","a","l","d","g");
        var worldViewModel = new LearningWorldViewModel("x","x","x","x","x","x");
        var spaceEntity = new LearningSpace("n", "s", "a", "d", "g", 5);
        var conditionEntity = new PathWayCondition(ConditionEnum.And, 2, 1);
        var pathWayEntity = new LearningPathway(spaceEntity,conditionEntity);
        world.LearningSpaces.Add(spaceEntity);
        world.PathWayConditions.Add(conditionEntity);
        world.LearningPathways.Add(pathWayEntity);
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(world, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningPathWays, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningSpaces.First().Id, Is.EqualTo(spaceEntity.Id));
        Assert.That(worldViewModel.PathWayConditions.First().Id, Is.EqualTo(conditionEntity.Id));
        Assert.That(worldViewModel.LearningPathWays.First().Id, Is.EqualTo(pathWayEntity.Id));
        
        var spaceViewModel = worldViewModel.LearningSpaces.First();
        var conditionViewModel = worldViewModel.PathWayConditions.First();
        var pathWayViewModel = worldViewModel.LearningPathWays.First();
        worldViewModel.LearningSpaces.Clear();
        worldViewModel.PathWayConditions.Clear();
        worldViewModel.LearningPathWays.Clear();
        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.LearningPathWays, Has.Count.EqualTo(0));
        
        systemUnderTest.Map(world, worldViewModel);
        
        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningPathWays, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningSpaces.First(), Is.EqualTo(spaceViewModel));
        Assert.That(worldViewModel.PathWayConditions.First(), Is.EqualTo(conditionViewModel));
    }

    [Test]
    public void
        MapLearningSpaceEntityToViewModel_ChangedLayoutWithElementWithIndexBiggerThanTheOldLearningElementsArray_MapsCorrectly()
    {
        var spaceEntity = new LearningSpace("n", "s", "a", "d", "g", 5, new LearningSpaceLayout(new ILearningElement?[4], FloorPlanEnum.Rectangle2X2));
        var elementEntity = new LearningElement("n", "s", new FileContent("n", "t", "f"), "a", "d", "g", LearningElementDifficultyEnum.Easy, spaceEntity);
        spaceEntity.LearningSpaceLayout.LearningElements[3] = elementEntity;
        
        var systemUnderTest = CreateTestableCachingMapper();
        
        var spaceViewModel = new LearningSpaceViewModel("", "", "", "", "", 0, new LearningSpaceLayoutViewModel());
        
        systemUnderTest.Map<LearningSpace, LearningSpaceViewModel>(spaceEntity, spaceViewModel);
        
        Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements, Has.Length.EqualTo(4));
        Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[3], Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[3]!.Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(spaceViewModel.Shortname, Is.EqualTo(spaceEntity.Shortname));
            Assert.That(spaceViewModel.Authors, Is.EqualTo(spaceEntity.Authors));
            Assert.That(spaceViewModel.Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(spaceViewModel.Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(spaceViewModel.RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
        });

        spaceEntity.LearningSpaceLayout = new LearningSpaceLayout(new ILearningElement[6], FloorPlanEnum.Rectangle2X3);
        spaceEntity.LearningSpaceLayout.LearningElements[5] = elementEntity;

        systemUnderTest.Map<LearningSpace, LearningSpaceViewModel>(spaceEntity, spaceViewModel);
        
        Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements, Has.Length.EqualTo(6));
        Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[5], Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[5]!.Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(spaceViewModel.Shortname, Is.EqualTo(spaceEntity.Shortname));
            Assert.That(spaceViewModel.Authors, Is.EqualTo(spaceEntity.Authors));
            Assert.That(spaceViewModel.Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(spaceViewModel.Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(spaceViewModel.RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
        });
    }

    [Test]
    public void MapLearningSpaceEntityToViewModel_MapperReceivedCallWithCorrectParameters()
    {
        var entity = new LearningSpace("n", "s", "a", "d", "g", 5);
        var viewModel = new LearningSpaceViewModel("x","x","x","x","x",5);
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(entity, viewModel);

        mapper.Received(1).Map(entity, (ILearningSpaceViewModel)viewModel);
    }
    
    [Test]
    public void MapLearningSpaceEntityToViewModel_MapsLearningElementToViewModel()
    {
        var space = new LearningSpace("n", "s", "a", "d", "g", 5, new LearningSpaceLayout(new ILearningElement[6], FloorPlanEnum.Rectangle2X3));
        var spaceViewModel = new LearningSpaceViewModel("x","x","x","x","x",5);
        var elementEntity = new LearningElement("n", "s", null!,"a", "d", "g", LearningElementDifficultyEnum.Easy);
        space.LearningSpaceLayout.LearningElements[0] = elementEntity;
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(space, spaceViewModel);

        Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.ContainedLearningElements.First().Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Name, Is.EqualTo(elementEntity.Name));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Shortname, Is.EqualTo(elementEntity.Shortname));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Authors, Is.EqualTo(elementEntity.Authors));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Description, Is.EqualTo(elementEntity.Description));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Goals, Is.EqualTo(elementEntity.Goals));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Difficulty, Is.EqualTo(elementEntity.Difficulty));
        });
    }
    
    [Test]
    public void MapLearningSpaceEntityToViewModel_MapsElementToTheSameViewModelAfterFirstCall()
    {
        var space = new LearningSpace("n", "s", "a", "d", "g", 5,
            new LearningSpaceLayout(new ILearningElement[6], FloorPlanEnum.Rectangle2X3));
        var spaceViewModel = new LearningSpaceViewModel("x", "x", "x", "x", "x", 5,
            new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var elementEntity =
            new LearningElement("n", "s", null!, "a", "d", "g", LearningElementDifficultyEnum.Easy);
        space.LearningSpaceLayout.LearningElements[0] = elementEntity;
        
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(space, spaceViewModel);

        Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(spaceViewModel.ContainedLearningElements.First().Id, Is.EqualTo(elementEntity.Id));
        
        var elementViewModel = spaceViewModel.ContainedLearningElements.First();
        spaceViewModel.LearningSpaceLayout.ClearAllElements();
        Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(0));
        
        space.ContainedLearningElements.First().Name = "newName";
        systemUnderTest.Map(space, spaceViewModel);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.ContainedLearningElements.First().Name, Is.EqualTo("newName"));
            Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Id, Is.EqualTo(elementViewModel.Id));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Name, Is.EqualTo(elementViewModel.Name));
            Assert.That(spaceViewModel.ContainedLearningElements.First(), Is.EqualTo(elementViewModel));
            Assert.That(spaceViewModel.ContainedLearningElements.First(), Is.EqualTo(systemUnderTest.ReadOnlyCache[elementEntity.Id]));
            Assert.That(elementViewModel, Is.EqualTo(systemUnderTest.ReadOnlyCache[elementEntity.Id]));
        });
    }
    
    [Test]
    public void OnRemovedCommandsFromStacksInvoked_UnusedViewModelsAreRemoved()
    {
        var worldEntity = new LearningWorld("n","s","a","l","d","g");
        var workspace = new AuthoringToolWorkspace(worldEntity, new List<LearningWorld>(){worldEntity});
        var workspaceViewModel = new AuthoringToolWorkspaceViewModel();
        var spaceEntity = new LearningSpace("n", "s", "a", "d", "g", 5, new LearningSpaceLayout(new ILearningElement?[6], FloorPlanEnum.Rectangle2X3));
        var elementEntity = new LearningElement("n", "s", null!,"a", "d", "g", LearningElementDifficultyEnum.Easy);
        var secondElementEntity = new LearningElement("n2", "s2", null!,"a2", "d2", "g2", LearningElementDifficultyEnum.Easy);
        
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateTestableCachingMapper(commandStateManager: mockCommandStateManager);

        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
        systemUnderTest.Map(workspace, workspaceViewModel);
        
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(1));
        
        worldEntity.LearningSpaces.Add(spaceEntity);
        var worldViewModel = workspaceViewModel.LearningWorlds.First();
        systemUnderTest.Map(worldEntity, worldViewModel);
        
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(2));
        
        spaceEntity.LearningSpaceLayout.LearningElements[0] = elementEntity;
        var spaceViewModel = worldViewModel.LearningSpaces.First();
        systemUnderTest.Map(spaceEntity, spaceViewModel);
        
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(3));
        
        spaceEntity.LearningSpaceLayout.LearningElements[1] = secondElementEntity;
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
        var elementEntity = new LearningElement("n", "s", null!,"a", "d", "g", LearningElementDifficultyEnum.Easy);
        var elementViewModel = new LearningElementViewModel("x","x",null!,"x","x","x",LearningElementDifficultyEnum.Easy);
        
        var systemUnderTest = CreateTestableCachingMapper();

        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
        systemUnderTest.Map(elementEntity, elementViewModel);
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void MapSomethingOtherThanWorkspaceOrWorldOrSpace_CallsMapper(){
        var elementEntity = new LearningElement("n", "s", null!,"a", "d", "g", LearningElementDifficultyEnum.Easy);
        var elementViewModel = new LearningElementViewModel("x","x",null!,"x","x","x",LearningElementDifficultyEnum.Easy);
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