using AuthoringTool.Mapping;
using AutoMapper;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;
using TestHelpers;

namespace AuthoringToolTest.Mapping;

[TestFixture]
public class CachingMapperUt
{
    [Test]
    public void MapAuthoringToolWorkspaceEntityToViewModel_MapperReceivedCallWithCorrectParameters()
    {
        var entity = new AuthoringToolWorkspace(new List<ILearningWorld>());
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
        var workspace = EntityProvider.GetAuthoringToolWorkspace();
        var workspaceViewModel = ViewModelProvider.GetAuthoringToolWorkspace();
        var worldEntity = EntityProvider.GetLearningWorld();
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
        var workspace = EntityProvider.GetAuthoringToolWorkspace();
        var workspaceViewModel = ViewModelProvider.GetAuthoringToolWorkspace();
        var worldEntity = EntityProvider.GetLearningWorld();
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
        var entity = EntityProvider.GetLearningWorld();
        var viewModel = ViewModelProvider.GetLearningWorld();
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(entity, viewModel);

        mapper.Received(1).Map(entity, (ILearningWorldViewModel) viewModel);
    }

    [Test]
    public void MapLearningWorldEntityToViewModel_MapsLearningSpaceConditionAndPathWayToViewModel()
    {
        var world = EntityProvider.GetLearningWorld();
        var worldViewModel = ViewModelProvider.GetLearningWorld();
        var spaceEntity = EntityProvider.GetLearningSpace();
        var conditionEntity = EntityProvider.GetPathWayCondition();
        var pathWayEntity = EntityProvider.GetLearningPathway(source: spaceEntity, target: conditionEntity);
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
            Assert.That(worldViewModel.LearningSpaces.First().Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(worldViewModel.LearningSpaces.First().Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(worldViewModel.LearningSpaces.First().RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
            Assert.That(worldViewModel.LearningSpaces.First().Theme, Is.EqualTo(spaceEntity.Theme));
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
        var world = EntityProvider.GetLearningWorld();
        var worldViewModel = ViewModelProvider.GetLearningWorld();
        var spaceEntity = EntityProvider.GetLearningSpace();
        var conditionEntity = EntityProvider.GetPathWayCondition();
        var pathWayEntity = EntityProvider.GetLearningPathway(source: spaceEntity, target: conditionEntity);
        var topicEntity = EntityProvider.GetTopic();
        world.LearningSpaces.Add(spaceEntity);
        world.PathWayConditions.Add(conditionEntity);
        world.LearningPathways.Add(pathWayEntity);
        world.Topics.Add(topicEntity);
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(world, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningPathWays, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningSpaces.First().Id, Is.EqualTo(spaceEntity.Id));
        Assert.That(worldViewModel.PathWayConditions.First().Id, Is.EqualTo(conditionEntity.Id));
        Assert.That(worldViewModel.LearningPathWays.First().Id, Is.EqualTo(pathWayEntity.Id));
        Assert.That(worldViewModel.Topics.First().Id, Is.EqualTo(topicEntity.Id));

        var spaceViewModel = worldViewModel.LearningSpaces.First();
        var conditionViewModel = worldViewModel.PathWayConditions.First();
        var topicViewModel = worldViewModel.Topics.First();
        worldViewModel.LearningSpaces.Clear();
        worldViewModel.PathWayConditions.Clear();
        worldViewModel.LearningPathWays.Clear();
        worldViewModel.Topics.Clear();
        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.LearningPathWays, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.Topics, Has.Count.EqualTo(0));

        systemUnderTest.Map(world, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningPathWays, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningSpaces.First(), Is.EqualTo(spaceViewModel));
        Assert.That(worldViewModel.PathWayConditions.First(), Is.EqualTo(conditionViewModel));
        Assert.That(worldViewModel.Topics.First(), Is.EqualTo(topicViewModel));
    }

    [Test]
    public void MapLearningWorldEntityToViewModel_NewSpaceWithTopic_UsingCachedTopicVm()
    {
        var world = EntityProvider.GetLearningWorld();
        var worldViewModel = ViewModelProvider.GetLearningWorld();
        var spaceEntity = EntityProvider.GetLearningSpace();
        var topicEntity = EntityProvider.GetTopic();
        spaceEntity.AssignedTopic = topicEntity;
        world.Topics.Add(topicEntity);

        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(world, worldViewModel);

        world.LearningSpaces.Add(spaceEntity);

        systemUnderTest.Map(world, worldViewModel);

        Assert.That(worldViewModel.Topics, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.LearningSpaces.First().AssignedTopic, Is.EqualTo(worldViewModel.Topics.First()));
    }

    [Test]
    public void MapLearningWorldEntityToViewModel_MoveElementFromSpaceToUnplaced()
    {
        var worldViewModel = ViewModelProvider.GetLearningWorld();
        var spaceViewModel = ViewModelProvider.GetLearningSpace();
        var elementViewModel = ViewModelProvider.GetLearningElement();
        spaceViewModel.LearningSpaceLayout.LearningElements[0] = elementViewModel;
        elementViewModel.Parent = spaceViewModel;
        worldViewModel.LearningSpaces.Add(spaceViewModel);

        var systemUnderTest = CreateTestableCachingMapper();

        var worldEntity = EntityProvider.GetLearningWorld();

        systemUnderTest.Map(worldViewModel, worldEntity);

        var spaceEntity = worldEntity.LearningSpaces.First();
        var elementEntity = spaceEntity.LearningSpaceLayout.LearningElements.First().Value;

        systemUnderTest.Map(worldEntity, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces.First().LearningSpaceLayout.LearningElements, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.UnplacedLearningElements, Has.Count.EqualTo(0));
        Assert.That(elementViewModel.Id, Is.EqualTo(elementEntity.Id));

        spaceEntity.LearningSpaceLayout.LearningElements.Remove(0);
        elementEntity.Parent = null;
        worldEntity.UnplacedLearningElements.Add(elementEntity);

        systemUnderTest.Map(worldEntity, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces.First().LearningSpaceLayout.LearningElements, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.UnplacedLearningElements, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.UnplacedLearningElements.First(), Is.EqualTo(elementViewModel));
    }

    [Test]
    public void MapLearningWorldEntityToViewModel_MoveElementFromUnplacedToSpace()
    {
        var worldViewModel = ViewModelProvider.GetLearningWorld();
        var spaceViewModel = ViewModelProvider.GetLearningSpace();
        var elementViewModel = ViewModelProvider.GetLearningElement();
        worldViewModel.UnplacedLearningElements.Add(elementViewModel);
        worldViewModel.LearningSpaces.Add(spaceViewModel);

        var systemUnderTest = CreateTestableCachingMapper();

        var worldEntity = EntityProvider.GetLearningWorld();

        systemUnderTest.Map(worldViewModel, worldEntity);

        var spaceEntity = worldEntity.LearningSpaces.First();
        var elementEntity = worldEntity.UnplacedLearningElements.First();

        systemUnderTest.Map(worldEntity, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces.First().LearningSpaceLayout.LearningElements, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.UnplacedLearningElements, Has.Count.EqualTo(1));
        Assert.That(elementViewModel.Id, Is.EqualTo(elementEntity.Id));

        worldEntity.UnplacedLearningElements.Remove(elementEntity);
        spaceEntity.LearningSpaceLayout.LearningElements[0] = elementEntity;
        elementEntity.Parent = spaceEntity;

        systemUnderTest.Map(worldEntity, worldViewModel);

        Assert.That(worldViewModel.LearningSpaces.First().LearningSpaceLayout.LearningElements, Has.Count.EqualTo(1));
        Assert.That(worldViewModel.UnplacedLearningElements, Has.Count.EqualTo(0));
        Assert.That(worldViewModel.LearningSpaces.First().LearningSpaceLayout.LearningElements.First().Value,
            Is.EqualTo(elementViewModel));
    }

    [Test]
    public void
        MapLearningSpaceEntityToViewModel_ChangedLayoutWithElementWithIndexBiggerThanTheOldLearningElementsArray_MapsCorrectly()
    {
        var spaceEntity = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L);
        var elementEntity = EntityProvider.GetLearningElement();
        spaceEntity.LearningSpaceLayout.LearningElements[5] = elementEntity;

        var systemUnderTest = CreateTestableCachingMapper();

        var spaceViewModel = ViewModelProvider.GetLearningSpace();
        systemUnderTest.Map<LearningSpace, LearningSpaceViewModel>(spaceEntity, spaceViewModel);

        Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements, Has.Count.EqualTo(1));
        Assert.That(spaceViewModel.LearningSpaceLayout.Capacity, Is.EqualTo(6));
        Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[5], Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[5]!.Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(spaceViewModel.Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(spaceViewModel.Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(spaceViewModel.RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
            Assert.That(spaceViewModel.Theme, Is.EqualTo(spaceEntity.Theme));
        });

        spaceEntity.LearningSpaceLayout =
            EntityProvider.GetLearningSpaceLayout(FloorPlanEnum.R_20X30_8L);
        spaceEntity.LearningSpaceLayout.LearningElements[7] = elementEntity;

        systemUnderTest.Map<LearningSpace, LearningSpaceViewModel>(spaceEntity, spaceViewModel);

        Assert.That(spaceViewModel.LearningSpaceLayout.Capacity, Is.EqualTo(8));
        Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[7], Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.LearningSpaceLayout.LearningElements[7]!.Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.Name, Is.EqualTo(spaceEntity.Name));
            Assert.That(spaceViewModel.Description, Is.EqualTo(spaceEntity.Description));
            Assert.That(spaceViewModel.Goals, Is.EqualTo(spaceEntity.Goals));
            Assert.That(spaceViewModel.RequiredPoints, Is.EqualTo(spaceEntity.RequiredPoints));
        });
    }

    [Test]
    public void MapLearningSpaceEntityToViewModel_MapperReceivedCallWithCorrectParameters()
    {
        var entity = EntityProvider.GetLearningSpace();
        var viewModel = ViewModelProvider.GetLearningSpace();
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(entity, viewModel);

        mapper.Received(1).Map(entity, (ILearningSpaceViewModel) viewModel);
    }

    [Test]
    public void MapLearningSpaceEntityToViewModel_MapsLearningElementToViewModel()
    {
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X30_8L);
        var spaceViewModel = ViewModelProvider.GetLearningSpace();
        var topicEntity = EntityProvider.GetTopic();
        var elementEntity = EntityProvider.GetLearningElement();
        space.AssignedTopic = topicEntity;
        space.LearningSpaceLayout.LearningElements[0] = elementEntity;

        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(space, spaceViewModel);

        Assert.That(spaceViewModel.AssignedTopic?.Id, Is.EqualTo(topicEntity.Id));
        Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(spaceViewModel.ContainedLearningElements.First().Id, Is.EqualTo(elementEntity.Id));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Name, Is.EqualTo(elementEntity.Name));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Description,
                Is.EqualTo(elementEntity.Description));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Goals, Is.EqualTo(elementEntity.Goals));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Difficulty,
                Is.EqualTo(elementEntity.Difficulty));
        });
    }

    [Test]
    public void MapLearningSpaceEntityToViewModel_MapsElementToTheSameViewModelAfterFirstCall()
    {
        //create space entity containing one element
        var space = EntityProvider.GetLearningSpace();
        var topicViewModel = ViewModelProvider.GetTopic();
        var topicEntity = EntityProvider.GetTopic();
        var elementEntity = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.LearningElements[0] = elementEntity;
        space.AssignedTopic = topicEntity;
        //create empty view model
        var spaceViewModel = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreateTestableCachingMapper();

        systemUnderTest.Map(topicViewModel, topicEntity);
        systemUnderTest.Map(topicEntity, topicViewModel);
        //map entity into empty view model
        systemUnderTest.Map(space, spaceViewModel);

        Assert.Multiple(() =>
        {
            //view model now contains one element, the one from the entity
            Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(spaceViewModel.AssignedTopic?.Id, Is.EqualTo(topicEntity.Id));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Id, Is.EqualTo(elementEntity.Id));
        });

        //get this element and clear the layout of the view model
        var elementViewModel = spaceViewModel.ContainedLearningElements.First();
        spaceViewModel.LearningSpaceLayout.ClearAllElements();
        spaceViewModel.AssignedTopic = null;
        Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(0));

        //change name of the element in the entity and map entity into viewmodel again
        space.ContainedLearningElements.First().Name = "newName";
        systemUnderTest.Map(space, spaceViewModel);
        Assert.Multiple(() =>
        {
            //element name should be the new one, but at the same time equal to the name of the old element view model
            //(they should be the same exact object)
            Assert.That(spaceViewModel.ContainedLearningElements.First().Name, Is.EqualTo("newName"));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Name, Is.EqualTo(elementViewModel.Name));
            Assert.That(spaceViewModel.ContainedLearningElements.First().Id, Is.EqualTo(elementViewModel.Id));
            Assert.That(spaceViewModel.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(spaceViewModel.ContainedLearningElements.First(), Is.EqualTo(elementViewModel));
            Assert.That(spaceViewModel.ContainedLearningElements.First(),
                Is.EqualTo(systemUnderTest.ReadOnlyCache[elementEntity.Id]));
            Assert.That(spaceViewModel.AssignedTopic, Is.EqualTo(topicViewModel));
            Assert.That(elementViewModel, Is.EqualTo(systemUnderTest.ReadOnlyCache[elementEntity.Id]));
        });
    }

    [Test]
    public void OnRemovedCommandsFromStacksInvoked_UnusedViewModelsAreRemoved()
    {
        var worldEntity = EntityProvider.GetLearningWorld();
        var workspace = EntityProvider.GetAuthoringToolWorkspace(worlds: new List<ILearningWorld> {worldEntity});
        var workspaceViewModel = ViewModelProvider.GetAuthoringToolWorkspace();
        var spaceEntity = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X30_8L);
        var elementEntity = EntityProvider.GetLearningElement();
        var secondElementEntity = EntityProvider.GetLearningElement(append: "2");
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
            Raise.Event<CommandStateManager.RemovedCommandsFromStacksHandler>(mockCommandStateManager,
                new RemoveCommandsFromStacksEventArgs(objectList));

        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(3));
    }

    [Test]
    public void MapSomethingOtherThanWorkspaceOrWorldOrSpace_DoesNotCache()
    {
        var elementEntity = EntityProvider.GetLearningElement();
        var elementViewModel = ViewModelProvider.GetLearningElement();
        var systemUnderTest = CreateTestableCachingMapper();

        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
        systemUnderTest.Map(elementEntity, elementViewModel);
        Assert.That(systemUnderTest.ReadOnlyCache, Has.Count.EqualTo(0));
    }

    [Test]
    public void MapSomethingOtherThanWorkspaceOrWorldOrSpace_CallsMapper()
    {
        var elementEntity = EntityProvider.GetLearningElement();
        var elementViewModel = ViewModelProvider.GetLearningElement();
        var mapper = Substitute.For<IMapper>();
        var systemUnderTest = CreateTestableCachingMapper(mapper);

        systemUnderTest.Map(elementEntity, elementViewModel);
        mapper.Received(1).Map(elementEntity, elementViewModel);
    }

    private static CachingMapper CreateTestableCachingMapper(
        IMapper? mapper = null, ICommandStateManager? commandStateManager = null, ILogger<CachingMapper>? logger = null)
    {
        var config = new MapperConfiguration(cfg =>
        {
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappersOnce();
        });
        mapper ??= config.CreateMapper();
        commandStateManager ??= Substitute.For<ICommandStateManager>();
        logger ??= Substitute.For<ILogger<CachingMapper>>();

        return new CachingMapper(mapper, commandStateManager, logger);
    }
}