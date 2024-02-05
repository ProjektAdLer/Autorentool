using System.Globalization;
using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Adaptivity.Action;
using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Commands.Adaptivity.Rule;
using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.LearningOutcomes;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.Entities.LearningOutcome;
using ElectronWrapper;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Adaptivity;
using Shared.Command;
using Shared.Configuration;
using Shared.LearningOutcomes;

namespace Presentation.PresentationLogic.API;

public class PresentationLogic : IPresentationLogic
{
    private const string WorldFileFormatDescriptor = "AdLer World File";
    private const string SpaceFileFormatDescriptor = "AdLer Space File";
    private const string ElementFileFormatDescriptor = "AdLer Element File";
    private readonly IElectronDialogManager? _dialogManager;

    public PresentationLogic(
        IApplicationConfiguration configuration,
        IBusinessLogic businessLogic,
        IMapper mapper,
        ICachingMapper cMapper,
        ISelectedViewModelsProvider selectedViewModelsProvider,
        IServiceProvider serviceProvider,
        ILogger<PresentationLogic> logger,
        IHybridSupportWrapper hybridSupportWrapper,
        IShellWrapper shellWrapper,
        IQuestionCommandFactory questionCommandFactory,
        ITaskCommandFactory taskCommandFactory,
        IConditionCommandFactory conditionCommandFactory,
        IElementCommandFactory elementCommandFactory,
        ILayoutCommandFactory layoutCommandFactory,
        IPathwayCommandFactory pathwayCommandFactory,
        ISpaceCommandFactory spaceCommandFactory,
        ITopicCommandFactory topicCommandFactory,
        ILearningOutcomeCommandFactory learningOutcomeCommandFactory,
        IWorldCommandFactory worldCommandFactory,
        IBatchCommandFactory batchCommandFactory,
        IAdaptivityRuleCommandFactory adaptivityRuleCommandFactory,
        IAdaptivityActionCommandFactory adaptivityActionCommandFactory,
        IFileSystem fileSystem
    )
    {
        Logger = logger;
        Configuration = configuration;
        BusinessLogic = businessLogic;
        Mapper = mapper;
        CMapper = cMapper;
        SelectedViewModelsProvider = selectedViewModelsProvider;
        HybridSupportWrapper = hybridSupportWrapper;
        ShellWrapper = shellWrapper;
        QuestionCommandFactory = questionCommandFactory;
        TaskCommandFactory = taskCommandFactory;
        ConditionCommandFactory = conditionCommandFactory;
        ElementCommandFactory = elementCommandFactory;
        LayoutCommandFactory = layoutCommandFactory;
        PathwayCommandFactory = pathwayCommandFactory;
        SpaceCommandFactory = spaceCommandFactory;
        TopicCommandFactory = topicCommandFactory;
        LearningOutcomeCommandFactory = learningOutcomeCommandFactory;
        WorldCommandFactory = worldCommandFactory;
        BatchCommandFactory = batchCommandFactory;
        AdaptivityRuleCommandFactory = adaptivityRuleCommandFactory;
        AdaptivityActionCommandFactory = adaptivityActionCommandFactory;
        FileSystem = fileSystem;
        _dialogManager = serviceProvider.GetService(typeof(IElectronDialogManager)) as IElectronDialogManager;
    }

    private static string WorldFileEnding => FileEndings.WorldFileEnding;
    private static string SpaceFileEnding => FileEndings.SpaceFileEnding;
    private static string ElementFileEnding => FileEndings.ElementFileEnding;

    internal ILogger<PresentationLogic> Logger { get; }
    internal IMapper Mapper { get; }
    internal ICachingMapper CMapper { get; }
    internal ISelectedViewModelsProvider SelectedViewModelsProvider { get; }
    internal IHybridSupportWrapper HybridSupportWrapper { get; }
    internal IShellWrapper ShellWrapper { get; }
    public IQuestionCommandFactory QuestionCommandFactory { get; }
    public ITaskCommandFactory TaskCommandFactory { get; }
    public IConditionCommandFactory ConditionCommandFactory { get; }
    public IElementCommandFactory ElementCommandFactory { get; }
    public ILayoutCommandFactory LayoutCommandFactory { get; }
    public IPathwayCommandFactory PathwayCommandFactory { get; }
    public ISpaceCommandFactory SpaceCommandFactory { get; }
    public ITopicCommandFactory TopicCommandFactory { get; }
    public ILearningOutcomeCommandFactory LearningOutcomeCommandFactory { get; }
    public IWorldCommandFactory WorldCommandFactory { get; }
    public IBatchCommandFactory BatchCommandFactory { get; }
    public IAdaptivityRuleCommandFactory AdaptivityRuleCommandFactory { get; }
    public IAdaptivityActionCommandFactory AdaptivityActionCommandFactory { get; }
    internal IFileSystem FileSystem { get; }

    public IApplicationConfiguration Configuration { get; }
    public IBusinessLogic BusinessLogic { get; }
    public bool RunningElectron => HybridSupportWrapper.IsElectronActive;

    /// <inheritdoc cref="IPresentationLogic.CanUndo"/>
    public bool CanUndo => BusinessLogic.CanUndo;

    /// <inheritdoc cref="IPresentationLogic.CanRedo"/>
    public bool CanRedo => BusinessLogic.CanRedo;

    public event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute
    {
        add => BusinessLogic.OnCommandUndoRedoOrExecute += value;
        remove => BusinessLogic.OnCommandUndoRedoOrExecute -= value;
    }

    /// <inheritdoc cref="IPresentationLogic.UndoCommand"/>
    public void UndoCommand()
    {
        BusinessLogic.UndoCommand();
    }

    /// <inheritdoc cref="IPresentationLogic.RedoCommand"/>
    public void RedoCommand()
    {
        BusinessLogic.RedoCommand();
    }

    /// <inheritdoc cref="IPresentationLogic.AddLearningWorld"/>
    public void AddLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        ILearningWorldViewModel learningWorldVm)
    {
        var authoringToolWorkspaceEntity =
            Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command = WorldCommandFactory.GetCreateCommand(authoringToolWorkspaceEntity, worldEntity,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateLearningWorld"/>
    public void CreateLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string name,
        string shortname, string authors, string language, string description, string goals, string evaluationLink,
        string enrolmentKey)
    {
        var authoringToolWorkspaceEntity =
            Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);

        var command = WorldCommandFactory.GetCreateCommand(authoringToolWorkspaceEntity, name, shortname, authors,
            language,
            description, goals, evaluationLink, enrolmentKey,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningWorld(authoringToolWorkspaceVm.LearningWorlds.Last(), command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditLearningWorld"/>
    public void EditLearningWorld(ILearningWorldViewModel learningWorldVm, string name,
        string shortname, string authors, string language, string description, string goals, string evaluationLink,
        string enrolmentKey)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command = WorldCommandFactory.GetEditCommand(worldEntity, name, shortname, authors, language, description,
            goals, evaluationLink, enrolmentKey,
            world => CMapper.Map(world, learningWorldVm));
        //quit early if there are no changes
        if (!command.AnyChanges())
        {
            Logger.LogInformation("No changes in edit learning world command, quitting before executing command");
            return;
        }

        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningWorld"/>
    public void DeleteLearningWorld(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        ILearningWorldViewModel worldVm)
    {
        var authoringToolWorkspaceEntity =
            Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(worldVm);

        var command = WorldCommandFactory.GetDeleteCommand(authoringToolWorkspaceEntity, worldEntity,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningWorld(null, command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningWorld"/>
    public void SaveLearningWorld(ILearningWorldViewModel learningWorldViewModel)
    {
        var filepath = learningWorldViewModel.SavePath;
        Logger.LogTrace("Saving world to {Path}", filepath);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldViewModel);
        var command = WorldCommandFactory.GetSaveCommand(BusinessLogic, worldEntity, filepath,
            world => CMapper.Map(world, learningWorldViewModel));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningWorldAsync"/>
    public async Task LoadLearningWorldAsync(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm)
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning World", WorldFileEnding, WorldFileFormatDescriptor);
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = WorldCommandFactory.GetLoadCommand(workspaceEntity, filepath, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningWorld(authoringToolWorkspaceVm.LearningWorlds.LastOrDefault(), command);
    }

    /// <inheritdoc cref="IPresentationLogic.AddLearningSpace"/>
    public void AddLearningSpace(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);

        var command = SpaceCommandFactory.GetCreateCommand(worldEntity, spaceEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningObjectInPathWay(learningSpaceVm, command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateLearningSpace"/>
    public void CreateLearningSpace(ILearningWorldViewModel learningWorldVm, string name, string description,
        LearningOutcomeCollectionViewModel learningOutcomeCollectionVm, int requiredPoints, Theme theme,
        double positionX,
        double positionY,
        ITopicViewModel? topicVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var topicEntity = Mapper.Map<BusinessLogic.Entities.Topic>(topicVm);
        var learningOutcomeCollection = Mapper.Map<LearningOutcomeCollection>(learningOutcomeCollectionVm);

        var command = SpaceCommandFactory.GetCreateCommand(worldEntity, name, description, learningOutcomeCollection,
            requiredPoints, theme,
            positionX, positionY, topicEntity, world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        var space = learningWorldVm.ObjectsInPathWays.Single(spvm => spvm.Id == command.NewSpace.Id);

        SelectedViewModelsProvider.SetLearningObjectInPathWay(space, command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditLearningSpace"/>
    public void EditLearningSpace(ILearningSpaceViewModel learningSpaceVm, string name,
        string description, int requiredPoints, Theme theme,
        ITopicViewModel? topicVm)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);
        var topicEntity = Mapper.Map<BusinessLogic.Entities.Topic>(topicVm);

        var command = SpaceCommandFactory.GetEditCommand(spaceEntity, name, description, requiredPoints, theme,
            topicEntity,
            space => CMapper.Map(space, learningSpaceVm));
        if (!command.AnyChanges())
        {
            Logger.LogInformation("No changes in edit learning space command, quitting before executing command");
            return;
        }

        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.ChangeLearningSpaceLayout"/>
    public void ChangeLearningSpaceLayout(ILearningSpaceViewModel learningSpaceVm,
        ILearningWorldViewModel learningWorldVm, FloorPlanEnum floorPlanName)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = worldEntity.LearningSpaces.First(s => s.Id == learningSpaceVm.Id);

        learningSpaceVm.AssignedTopic = null;
        var command = LayoutCommandFactory.GetChangeCommand(spaceEntity, worldEntity, floorPlanName,
            world => { CMapper.Map(world, learningWorldVm); });
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DragObjectInPathWay"/>
    public void DragObjectInPathWay(IObjectInPathWayViewModel objectInPathWayVm, double oldPositionX,
        double oldPositionY)
    {
        var objectInPathWayEntity = Mapper.Map<IObjectInPathWay>(objectInPathWayVm);

        var command = PathwayCommandFactory.GetDragCommand(objectInPathWayEntity, oldPositionX, oldPositionY,
            objectInPathWayEntity.PositionX, objectInPathWayEntity.PositionY,
            space => CMapper.Map(space, objectInPathWayVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningSpace"/>
    public void DeleteLearningSpace(ILearningWorldViewModel learningWorldVm, ILearningSpaceViewModel learningSpaceVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);

        var command = SpaceCommandFactory.GetDeleteCommand(worldEntity, spaceEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningObjectInPathWay(null, command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningSpaceAsync"/>
    public async Task SaveLearningSpaceAsync(LearningSpaceViewModel learningSpaceViewModel)
    {
        ElectronCheck();
        var filepath = await GetSaveFilepathAsync("Save Learning Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceViewModel);
        var command = SpaceCommandFactory.GetSaveCommand(BusinessLogic, spaceEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningSpaceAsync"/>
    public async Task LoadLearningSpaceAsync(ILearningWorldViewModel learningWorldVm)
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning Space", SpaceFileEnding, SpaceFileFormatDescriptor);
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var command = SpaceCommandFactory.GetLoadCommand(worldEntity, filepath, BusinessLogic,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningObjectInPathWay(learningWorldVm.ObjectsInPathWays.LastOrDefault(),
            command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreatePathWayCondition"/>
    public void CreatePathWayCondition(ILearningWorldViewModel learningWorldVm, ConditionEnum condition,
        double positionX, double positionY)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command = ConditionCommandFactory.GetCreateCommand(worldEntity, condition, positionX, positionY,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreatePathWayConditionBetweenObjects"/>
    public void CreatePathWayConditionBetweenObjects(ILearningWorldViewModel learningWorldVm, ConditionEnum condition,
        IObjectInPathWayViewModel sourceObject, ILearningSpaceViewModel targetObject)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var sourceObjectEntity = Mapper.Map<IObjectInPathWay>(sourceObject);
        var targetObjectEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(targetObject);

        var command = ConditionCommandFactory.GetCreateCommand(worldEntity, condition, sourceObjectEntity,
            targetObjectEntity, world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditPathWayCondition"/>
    public void EditPathWayCondition(PathWayConditionViewModel pathWayConditionVm, ConditionEnum newCondition)
    {
        var pathWayConditionEntity = Mapper.Map<PathWayCondition>(pathWayConditionVm);

        var command = ConditionCommandFactory.GetEditCommand(pathWayConditionEntity, newCondition,
            condition => CMapper.Map(condition, pathWayConditionVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeletePathWayCondition"/>
    public void DeletePathWayCondition(ILearningWorldViewModel learningWorldVm,
        PathWayConditionViewModel pathWayConditionVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var pathWayConditionEntity = Mapper.Map<PathWayCondition>(pathWayConditionVm);

        var command = ConditionCommandFactory.GetDeleteCommand(worldEntity, pathWayConditionEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningObjectInPathWay(null, command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateTopic"/>
    public void CreateTopic(ILearningWorldViewModel learningWorldVm, string name)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command =
            TopicCommandFactory.GetCreateCommand(learningWorldEntity, name,
                world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditTopic"/>
    public void EditTopic(ITopicViewModel topicVm, string newName)
    {
        var topicEntity = Mapper.Map<BusinessLogic.Entities.Topic>(topicVm);

        var command = TopicCommandFactory.GetEditCommand(topicEntity, newName, topic => CMapper.Map(topic, topicVm));

        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteTopic"/>
    public void DeleteTopic(ILearningWorldViewModel learningWorldVm, ITopicViewModel topicVm)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var topicEntity = Mapper.Map<BusinessLogic.Entities.Topic>(topicVm);

        var listOfCommands =
            learningWorldEntity.LearningSpaces
                .Where(x => x.AssignedTopic?.Id == topicEntity.Id)
                .Select(spaceEntity => new { spaceEntity, spaceVm = Mapper.Map<LearningSpaceViewModel>(spaceEntity) })
                .Select(t => SpaceCommandFactory.GetEditCommand(t.spaceEntity, t.spaceEntity.Name,
                    t.spaceEntity.Description, t.spaceEntity.RequiredPoints,
                    t.spaceEntity.Theme, null,
                    space => CMapper.Map(space, t.spaceVm)))
                .Cast<IUndoCommand>()
                .ToList();

        var deleteTopic =
            TopicCommandFactory.GetDeleteCommand(learningWorldEntity, topicEntity,
                world => CMapper.Map(world, learningWorldVm));

        listOfCommands.Add(deleteTopic);

        var batchCommand = BatchCommandFactory.GetBatchCommand(listOfCommands);

        BusinessLogic.ExecuteCommand(batchCommand);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateLearningPathWay"/>
    public void CreateLearningPathWay(ILearningWorldViewModel learningWorldVm, IObjectInPathWayViewModel sourceObjectVm,
        IObjectInPathWayViewModel targetObjectVm)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var sourceObjectEntity = Mapper.Map<IObjectInPathWay>(sourceObjectVm);
        var targetObjectEntity = Mapper.Map<IObjectInPathWay>(targetObjectVm);

        var command = PathwayCommandFactory.GetCreateCommand(learningWorldEntity, sourceObjectEntity,
            targetObjectEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningPathWay"/>
    public void DeleteLearningPathWay(ILearningWorldViewModel learningWorldVm,
        ILearningPathWayViewModel learningPathWayVm)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var learningPathWayEntity = Mapper.Map<BusinessLogic.Entities.LearningPathway>(learningPathWayVm);

        var command = PathwayCommandFactory.GetDeleteCommand(learningWorldEntity, learningPathWayEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningObjectInPathWay(null, command);
    }

    /// <inheritdoc cref="IPresentationLogic.AddStructuredLearningOutcome"/>
    public void AddStructuredLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        TaxonomyLevel taxonomyLevel, string what, string verbOfVisibility, string whereby,
        string whatFor, CultureInfo language)
    {
        var learningOutcomeCollectionEntity = Mapper.Map<LearningOutcomeCollection>(learningOutcomes);

        var command = LearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(learningOutcomeCollectionEntity,
            taxonomyLevel, what, verbOfVisibility, whereby, whatFor, language,
            collection => Mapper.Map(collection, learningOutcomes));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.AddManualLearningOutcome"/>
    public void AddManualLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        string manualLearningOutcomeText)
    {
        var learningOutcomeCollectionEntity = Mapper.Map<LearningOutcomeCollection>(learningOutcomes);

        var command = LearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(learningOutcomeCollectionEntity,
            manualLearningOutcomeText,
            collection => Mapper.Map(collection, learningOutcomes));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditStructuredLearningOutcome"/>
    public void EditStructuredLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        StructuredLearningOutcomeViewModel learningOutcome,
        TaxonomyLevel taxonomyLevel, string what, string verbOfVisibility, string whereby,
        string whatFor, CultureInfo language)
    {
        var learningOutcomeCollectionEntity = Mapper.Map<LearningOutcomeCollection>(learningOutcomes);
        var learningOutcomeEntity = Mapper.Map<StructuredLearningOutcome>(learningOutcome);

        var listOfCommands = new List<IUndoCommand>
        {
            LearningOutcomeCommandFactory.GetDeleteLearningOutcomeCommand(learningOutcomeCollectionEntity,
                learningOutcomeEntity,
                collection => Mapper.Map(collection, learningOutcomes)),
            LearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(learningOutcomeCollectionEntity, taxonomyLevel,
                what, verbOfVisibility, whereby, whatFor, language,
                collection => Mapper.Map(collection, learningOutcomes))
        };

        var batchCommand = BatchCommandFactory.GetBatchCommand(listOfCommands);

        BusinessLogic.ExecuteCommand(batchCommand);
    }

    /// <inheritdoc cref="IPresentationLogic.EditManualLearningOutcome"/>
    public void EditManualLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        ManualLearningOutcomeViewModel learningOutcome, string manualLearningOutcomeText)
    {
        var index = learningOutcomes.LearningOutcomes.IndexOf(learningOutcome);
        var learningOutcomeCollectionEntity = Mapper.Map<LearningOutcomeCollection>(learningOutcomes);
        var learningOutcomeEntity = Mapper.Map<ManualLearningOutcome>(learningOutcome);

        var listOfCommands = new List<IUndoCommand>
        {
            LearningOutcomeCommandFactory.GetDeleteLearningOutcomeCommand(learningOutcomeCollectionEntity,
                learningOutcomeEntity,
                collection => Mapper.Map(collection, learningOutcomes)),
            LearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(learningOutcomeCollectionEntity,
                manualLearningOutcomeText,
                collection => Mapper.Map(collection, learningOutcomes), index)
        };

        var batchCommand = BatchCommandFactory.GetBatchCommand(listOfCommands);

        BusinessLogic.ExecuteCommand(batchCommand);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningOutcome"/>
    public void DeleteLearningOutcome(LearningOutcomeCollectionViewModel learningOutcomes,
        ILearningOutcomeViewModel learningOutcome)
    {
        var learningOutcomeCollectionEntity = Mapper.Map<LearningOutcomeCollection>(learningOutcomes);
        var learningOutcomeEntity = Mapper.Map<ILearningOutcome>(learningOutcome);

        var command = LearningOutcomeCommandFactory.GetDeleteLearningOutcomeCommand(learningOutcomeCollectionEntity,
            learningOutcomeEntity,
            collection => Mapper.Map(collection, learningOutcomes));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.AddLearningElement"/>
    public void AddLearningElement(ILearningSpaceViewModel parentSpaceVm, int slotIndex,
        ILearningElementViewModel learningElementVm)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = ElementCommandFactory.GetCreateInSlotCommand(parentSpaceEntity, slotIndex, elementEntity,
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningElement(learningElementVm, command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateUnplacedLearningElement"/>
    public void CreateUnplacedLearningElement(ILearningWorldViewModel learningWorldVm, string name,
        ILearningContentViewModel learningContentVm, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        double positionX = 0D,
        double positionY = 0D)
    {
        var learningWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var contentEntity = Mapper.Map<ILearningContent>(learningContentVm);

        //copy list of elements before
        var elementsInWorldBefore = learningWorldVm.UnplacedLearningElements.ToList();

        var command = ElementCommandFactory.GetCreateUnplacedCommand(learningWorldEntity, name, contentEntity,
            description, goals,
            difficulty, elementModel, workload, points, positionX, positionY,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        //as there should only be one new element, we can take new list - old list = new element
        var elementsInWorldAfter = learningWorldVm.UnplacedLearningElements;
        var newElement = elementsInWorldAfter.Except(elementsInWorldBefore).First();
        SelectedViewModelsProvider.SetLearningElement(newElement, command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateLearningElementInSlot"/>
    public void CreateLearningElementInSlot(ILearningSpaceViewModel parentSpaceVm, int slotIndex, string name,
        ILearningContentViewModel learningContentVm, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        double positionX = 0, double positionY = 0)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var contentEntity = Mapper.Map<ILearningContent>(learningContentVm);

        //copy list of elements before
        var elementsInSpaceBefore = parentSpaceVm.ContainedLearningElements.ToList();

        var command = ElementCommandFactory.GetCreateInSlotCommand(parentSpaceEntity, slotIndex, name, contentEntity,
            description,
            goals, difficulty, elementModel, workload, points, positionX, positionY,
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);

        //as there should only be one new element, we can take new list - old list = new element
        var elementsInSpaceAfter = parentSpaceVm.ContainedLearningElements;
        var newElement = elementsInSpaceAfter.Except(elementsInSpaceBefore).First();
        SelectedViewModelsProvider.SetLearningElement(newElement, command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditLearningElement"/>
    public void EditLearningElement(ILearningSpaceViewModel? parentSpaceVm, ILearningElementViewModel learningElementVm,
        string name, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, ILearningContentViewModel learningContentViewModel)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);
        var contentEntity = Mapper.Map<ILearningContent>(learningContentViewModel);

        var parentSpaceEntity = parentSpaceVm != null
            ? Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm)
            : null;
        var command = ElementCommandFactory.GetEditCommand(elementEntity, parentSpaceEntity, name, description,
            goals, difficulty, elementModel, workload, points, contentEntity,
            element => CMapper.Map(element, learningElementVm));
        if (!command.AnyChanges())
        {
            Logger.LogInformation("No changes in edit learning element command, quitting before executing command");
            return;
        }

        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DragLearningElementFromUnplaced"/>
    public void DragLearningElementFromUnplaced(ILearningWorldViewModel learningWorldVm,
        ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm, int newSlotIndex)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = LayoutCommandFactory.GetPlaceFromUnplacedCommand(worldEntity, spaceEntity, elementEntity,
            newSlotIndex,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        if (SelectedViewModelsProvider.ActiveSlotInSpace == newSlotIndex)
        {
            SelectedViewModelsProvider.SetActiveSlotInSpace(-1, command);
        }
    }

    /// <inheritdoc cref="IPresentationLogic.DragLearningElementToUnplaced"/>
    public void DragLearningElementToUnplaced(ILearningWorldViewModel learningWorldVm,
        ILearningSpaceViewModel learningSpaceVm, ILearningElementViewModel learningElementVm)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = LayoutCommandFactory.GetRemoveCommand(worldEntity, spaceEntity, elementEntity,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.SwitchLearningElementSlot"/>
    public void SwitchLearningElementSlot(ILearningSpaceViewModel learningSpaceVm,
        ILearningElementViewModel learningElementVm, int newSlotIndex)
    {
        var spaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = LayoutCommandFactory.GetPlaceFromLayoutCommand(spaceEntity, elementEntity, newSlotIndex,
            space => CMapper.Map(space, learningSpaceVm));
        BusinessLogic.ExecuteCommand(command);

        if (SelectedViewModelsProvider.ActiveSlotInSpace == newSlotIndex)
        {
            SelectedViewModelsProvider.SetActiveSlotInSpace(-1, command);
        }
    }

    /// <inheritdoc cref="IPresentationLogic.DragLearningElement"/>
    public void DragLearningElement(ILearningElementViewModel learningElementVm, double oldPositionX,
        double oldPositionY)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);

        var command = ElementCommandFactory.GetDragCommand(elementEntity, oldPositionX, oldPositionY,
            elementEntity.PositionX, elementEntity.PositionY,
            space => CMapper.Map(space, learningElementVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningElementInSpace"/>
    public void DeleteLearningElementInSpace(ILearningSpaceViewModel parentSpaceVm,
        ILearningElementViewModel learningElementVm)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);

        var command = ElementCommandFactory.GetDeleteInSpaceCommand(elementEntity, parentSpaceEntity,
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningElement(null, command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteLearningElementInWorld"/>
    public void DeleteLearningElementInWorld(ILearningWorldViewModel learningWorldVm,
        ILearningElementViewModel learningElementVm)
    {
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementVm);
        var parentWorldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);

        var command = ElementCommandFactory.GetDeleteInWorldCommand(elementEntity, parentWorldEntity,
            parent => CMapper.Map(parent, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningElement(null, command);
    }

    /// <inheritdoc cref="IPresentationLogic.SaveLearningElementAsync"/>
    public async Task SaveLearningElementAsync(LearningElementViewModel learningElementViewModel)
    {
        ElectronCheck();
        var filepath =
            await GetSaveFilepathAsync("Save Learning Element", ElementFileEnding, ElementFileFormatDescriptor);
        var elementEntity = Mapper.Map<BusinessLogic.Entities.LearningElement>(learningElementViewModel);
        var command = ElementCommandFactory.GetSaveCommand(BusinessLogic, elementEntity, filepath);
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningElementAsync"/>
    public async Task LoadLearningElementAsync(ILearningSpaceViewModel parentSpaceVm, int slotIndex)
    {
        ElectronCheck();
        var filepath =
            await GetLoadFilepathAsync("Load Learning Element", ElementFileEnding, ElementFileFormatDescriptor);
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var command = ElementCommandFactory.GetLoadCommand(parentSpaceEntity, slotIndex, filepath, BusinessLogic,
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningElement(parentSpaceVm.ContainedLearningElements.Last(), command);
    }


    /// <inheritdoc cref="IPresentationLogic.ShowLearningElementContentAsync"/>
    public async Task ShowLearningElementContentAsync(LearningElementViewModel learningElementVm)
    {
        ElectronCheck();
        try
        {
            await ShowLearningContentAsync(learningElementVm.LearningContent);
        }
        catch (ArgumentOutOfRangeException)
        {
            Logger.LogError(
                "LearningElementViewModel.LearningContent is not of type FileContentViewModel or LinkContentViewModel");
            throw new ArgumentOutOfRangeException(nameof(learningElementVm),
                "LearningElementViewModel.LearningContent is not of type FileContentViewModel or LinkContentViewModel");
        }
    }

    /// <inheritdoc cref="IPresentationLogic.ShowLearningContentAsync"/>
    public async Task ShowLearningContentAsync(ILearningContentViewModel content)
    {
        ElectronCheck();
        var error =
            await (content switch
            {
                FileContentViewModel fileContentVm => ShellWrapper.OpenPathAsync(fileContentVm.Filepath),
                LinkContentViewModel linkContentVm => ShellWrapper.OpenExternalAsync(linkContentVm.Link),
                _ => throw new ArgumentOutOfRangeException(nameof(content),
                    "LearningContent is not of type FileContentViewModel or LinkContentViewModel")
            });

        if (error != "")
        {
            Logger.LogError("Could not open file in OS viewer: {Error}", error);
            throw new IOException("Could not open file in OS viewer" + error);
        }
    }

    /// <inheritdoc cref="IPresentationLogic.GetAllContent"/>
    public IEnumerable<ILearningContentViewModel> GetAllContent() =>
        BusinessLogic.GetAllContent().Select(Mapper.Map<ILearningContentViewModel>);

    /// <inheritdoc cref="IPresentationLogic.RemoveContent"/>
    public void RemoveContent(ILearningContentViewModel content) =>
        BusinessLogic.RemoveContent(Mapper.Map<ILearningContent>(content));

    /// <inheritdoc cref="IPresentationLogic.SaveLink"/>
    public void SaveLink(LinkContentViewModel linkContentVm) =>
        BusinessLogic.SaveLink(Mapper.Map<LinkContent>(linkContentVm));

    /// <inheritdoc cref="IPresentationLogic.OpenContentFilesFolder"/>
    public void OpenContentFilesFolder()
    {
        ElectronCheck();
        var path = BusinessLogic.GetContentFilesFolderPath();
        ShellWrapper.OpenPathAsync(path);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningWorldViewModel"/>
    public void LoadLearningWorldViewModel(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, Stream stream)
    {
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = WorldCommandFactory.GetLoadCommand(workspaceEntity, stream, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);

        SelectedViewModelsProvider.SetLearningWorld(authoringToolWorkspaceVm.LearningWorlds.Last(), command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningSpaceViewModel"/>
    public void LoadLearningSpaceViewModel(ILearningWorldViewModel learningWorldVm, Stream stream)
    {
        var worldEntity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        var command = SpaceCommandFactory.GetLoadCommand(worldEntity, stream, BusinessLogic,
            world => CMapper.Map(world, learningWorldVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningElementViewModel"/>
    public void LoadLearningElementViewModel(ILearningSpaceViewModel parentSpaceVm, int slotIndex, Stream stream)
    {
        var parentSpaceEntity = Mapper.Map<BusinessLogic.Entities.LearningSpace>(parentSpaceVm);
        var command = ElementCommandFactory.GetLoadCommand(parentSpaceEntity, slotIndex, stream, BusinessLogic,
            parent => CMapper.Map(parent, parentSpaceVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningContentViewModelAsync"/>
    public async Task<ILearningContentViewModel> LoadLearningContentViewModelAsync(string name, Stream stream)
    {
        var entity = await BusinessLogic.LoadLearningContentAsync(name, stream);

        return Mapper.Map<ILearningContentViewModel>(entity);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateAdaptivityTask"/>
    public void CreateAdaptivityTask(IAdaptivityContentViewModel adaptivityContentVm, string name)
    {
        var contentEntity = Mapper.Map<AdaptivityContent>(adaptivityContentVm);
        var command = TaskCommandFactory.GetCreateCommand(contentEntity, name,
            content => CMapper.Map(content, adaptivityContentVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditAdaptivityTask"/>
    public void EditAdaptivityTask(IAdaptivityTaskViewModel adaptivityTaskVm, string name,
        QuestionDifficulty? minimumRequiredDifficulty)
    {
        var taskEntity = Mapper.Map<AdaptivityTask>(adaptivityTaskVm);
        var command = TaskCommandFactory.GetEditCommand(taskEntity, name, minimumRequiredDifficulty,
            task => CMapper.Map(task, adaptivityTaskVm));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteAdaptivityTask"/>
    public void DeleteAdaptivityTask(IAdaptivityContentViewModel adaptivityContentVm,
        IAdaptivityTaskViewModel adaptivityTaskVm)
    {
        var contentEntity = Mapper.Map<AdaptivityContent>(adaptivityContentVm);
        var taskEntity = Mapper.Map<AdaptivityTask>(adaptivityTaskVm);
        var command = TaskCommandFactory.GetDeleteCommand(contentEntity, taskEntity,
            content => CMapper.Map(content, adaptivityContentVm));
        BusinessLogic.ExecuteCommand(command);
    }

    public void SetSelectedLearningContentViewModel(ILearningContentViewModel content)
    {
        SelectedViewModelsProvider.SetLearningContent(content, null);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateMultipleChoiceSingleResponseQuestion"/>
    public void CreateMultipleChoiceSingleResponseQuestion(IAdaptivityTaskViewModel taskViewModel,
        QuestionDifficulty difficulty, string questionText, ICollection<ChoiceViewModel> choices,
        ChoiceViewModel correctChoice, int expectedCompletionTime)
    {
        var taskEntity = Mapper.Map<AdaptivityTask>(taskViewModel);
        var choicesEntity = Mapper.Map<ICollection<Choice>>(choices);
        var correctChoiceEntity = Mapper.Map<Choice>(correctChoice);
        var command = QuestionCommandFactory.GetCreateMultipleChoiceSingleResponseQuestionCommand(taskEntity,
            difficulty, questionText, choicesEntity, correctChoiceEntity, expectedCompletionTime,
            task => CMapper.Map(taskEntity, taskViewModel));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.CreateMultipleChoiceMultipleResponseQuestion"/>
    public void CreateMultipleChoiceMultipleResponseQuestion(IAdaptivityTaskViewModel taskViewModel,
        QuestionDifficulty difficulty, string questionText, ICollection<ChoiceViewModel> choices,
        ICollection<ChoiceViewModel> correctChoices, int expectedCompletionTime)
    {
        var taskEntity = Mapper.Map<AdaptivityTask>(taskViewModel);
        var choicesEntity = Mapper.Map<ICollection<Choice>>(choices);
        var correctChoicesEntity = Mapper.Map<ICollection<Choice>>(correctChoices);
        var command = QuestionCommandFactory.GetCreateMultipleChoiceMultipleResponseQuestionCommand(taskEntity,
            difficulty, questionText, choicesEntity, correctChoicesEntity, expectedCompletionTime,
            task => CMapper.Map(task, taskViewModel));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditMultipleChoiceSingleResponseQuestion"/>
    public void EditMultipleChoiceSingleResponseQuestion(
        MultipleChoiceSingleResponseQuestionViewModel questionViewModel,
        string questionText, ICollection<ChoiceViewModel> choices, ChoiceViewModel correctChoice,
        int expectedCompletionTime)
    {
        var questionEntity = Mapper.Map<MultipleChoiceSingleResponseQuestion>(questionViewModel);
        var choicesEntity = Mapper.Map<ICollection<Choice>>(choices);
        var correctChoiceEntity = Mapper.Map<Choice>(correctChoice);
        var command = QuestionCommandFactory.GetEditMultipleChoiceSingleResponseQuestionCommand(questionEntity,
            questionText, choicesEntity, correctChoiceEntity, expectedCompletionTime,
            question => CMapper.Map(question, questionViewModel));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditMultipleChoiceMultipleResponseQuestion"/>
    public void EditMultipleChoiceMultipleResponseQuestion(
        MultipleChoiceMultipleResponseQuestionViewModel questionViewModel, string questionText,
        ICollection<ChoiceViewModel> choices, ICollection<ChoiceViewModel> correctChoices, int expectedCompletionTime)
    {
        var questionEntity = Mapper.Map<MultipleChoiceMultipleResponseQuestion>(questionViewModel);
        var choicesEntity = Mapper.Map<ICollection<Choice>>(choices);
        var correctChoicesEntity = Mapper.Map<ICollection<Choice>>(correctChoices);
        var command = QuestionCommandFactory.GetEditMultipleChoiceMultipleResponseQuestionCommand(questionEntity,
            questionText, choicesEntity, correctChoicesEntity, expectedCompletionTime,
            question => CMapper.Map(question, questionViewModel));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.EditMultipleChoiceQuestionWithTypeChange"/>
    public void EditMultipleChoiceQuestionWithTypeChange(IAdaptivityTaskViewModel taskViewModel,
        IMultipleChoiceQuestionViewModel question,
        bool isSingleResponse, string text, ICollection<ChoiceViewModel> choices,
        ICollection<ChoiceViewModel> correctChoices,
        int expectedCompletionTime)
    {
        var taskEntity = Mapper.Map<AdaptivityTask>(taskViewModel);
        var questionEntity = Mapper.Map<IMultipleChoiceQuestion>(question);
        var choicesEntity = Mapper.Map<ICollection<Choice>>(choices);
        var correctChoicesEntity = Mapper.Map<ICollection<Choice>>(correctChoices);
        var command = QuestionCommandFactory.GetEditMultipleChoiceQuestionWithTypeChangeCommand(taskEntity,
            questionEntity,
            isSingleResponse, text, choicesEntity, correctChoicesEntity, expectedCompletionTime,
            task => CMapper.Map(task, taskViewModel));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.DeleteAdaptivityQuestion"/>
    public void DeleteAdaptivityQuestion(IAdaptivityTaskViewModel taskViewModel, IAdaptivityQuestionViewModel question)
    {
        var taskEntity = Mapper.Map<AdaptivityTask>(taskViewModel);
        var questionEntity = Mapper.Map<IAdaptivityQuestion>(question);
        var command = QuestionCommandFactory.GetDeleteCommand(taskEntity, questionEntity,
            task => CMapper.Map(task, taskViewModel));
        BusinessLogic.ExecuteCommand(command);
    }

    public void CreateAdaptivityRule(IAdaptivityQuestionViewModel question, IAdaptivityTriggerViewModel trigger,
        IAdaptivityActionViewModel action)
    {
        var questionEntity = Mapper.Map<IAdaptivityQuestion>(question);
        var triggerEntity = Mapper.Map<IAdaptivityTrigger>(trigger);
        var actionEntity = Mapper.Map<IAdaptivityAction>(action);
        var command = AdaptivityRuleCommandFactory.GetCreateCommand(questionEntity, triggerEntity, actionEntity,
            entity => CMapper.Map(entity, question));
        BusinessLogic.ExecuteCommand(command);
    }

    public void DeleteAdaptivityRule(IAdaptivityQuestionViewModel question, IAdaptivityRuleViewModel rule)
    {
        var questionEntity = Mapper.Map<IAdaptivityQuestion>(question);
        var ruleEntity = Mapper.Map<IAdaptivityRule>(rule);
        var command = AdaptivityRuleCommandFactory.GetDeleteCommand(questionEntity, ruleEntity,
            entity => CMapper.Map(entity, question));
        BusinessLogic.ExecuteCommand(command);
    }

    public void EditCommentAction(CommentActionViewModel action, string comment)
    {
        var actionEntity = Mapper.Map<CommentAction>(action);
        var command = AdaptivityActionCommandFactory.GetEditCommentAction(actionEntity, comment,
            entity => CMapper.Map(entity, action));
        BusinessLogic.ExecuteCommand(command);
    }

    public void EditContentReferenceAction(ContentReferenceActionViewModel action, ILearningContentViewModel content,
        string comment)
    {
        var actionEntity = Mapper.Map<ContentReferenceAction>(action);
        var contentEntity = Mapper.Map<ILearningContent>(content);
        var command = AdaptivityActionCommandFactory.GetEditContentReferenceAction(actionEntity, contentEntity,
            comment,
            entity => CMapper.Map(entity, action));
        BusinessLogic.ExecuteCommand(command);
    }

    public void EditElementReferenceAction(ElementReferenceActionViewModel action, Guid elementGuid, string comment)
    {
        var actionEntity = Mapper.Map<ElementReferenceAction>(action);
        var command = AdaptivityActionCommandFactory.GetEditElementReferenceAction(actionEntity, elementGuid,
            comment,
            entity => CMapper.Map(entity, action));
        BusinessLogic.ExecuteCommand(command);
    }

    /// <inheritdoc cref="IPresentationLogic.GetLmsWorldList"/>
    public async Task<List<LmsWorldViewModel>> GetLmsWorldList()
    {
        var worldsEntity = await BusinessLogic.GetLmsWorldList();
        return Mapper.Map<List<LmsWorldViewModel>>(worldsEntity);
    }

    public async Task ExportLearningWorldToArchiveAsync(ILearningWorldViewModel world)
    {
        ElectronCheck();
        try
        {
            var pathToArchive = await _dialogManager!.ShowSaveAsDialogAsync("Archive export path",
                fileFilters: new[] { new FileFilterProxy("Zip archive", new[] { "zip" }) }
            );
            await BusinessLogic.ExportLearningWorldToArchiveAsync(
                Mapper.Map<BusinessLogic.Entities.LearningWorld>(world),
                pathToArchive);
        }
        catch (OperationCanceledException)
        {
            Logger.LogInformation("Export to archive canceled by user");
        }
    }

    public async Task<LearningWorldViewModel?> ImportLearningWorldFromArchiveAsync()
    {
        ElectronCheck();
        try
        {
            var pathToArchive = await _dialogManager!.ShowOpenFileDialogAsync("Archive import path",
                fileFilters: new[] { new FileFilterProxy("Zip archive", new[] { "zip" }) }
            );
            var worldEntity = await BusinessLogic.ImportLearningWorldFromArchiveAsync(pathToArchive);
            var viewModel = Mapper.Map<LearningWorldViewModel>(worldEntity);
            SelectedViewModelsProvider.SetLearningWorld(viewModel, null);
            return viewModel;
        }
        catch (OperationCanceledException)
        {
            Logger.LogInformation("Import from archive canceled by user");
            return null;
        }
    }

    public IFileInfo? GetFileInfoForLearningWorld(ILearningWorldViewModel world)
    {
        if (string.IsNullOrWhiteSpace(world.SavePath)) return null;
        return BusinessLogic.GetFileInfoForPath(world.SavePath);
    }

    public void DeleteLearningWorldByPath(string savePath)
    {
        BusinessLogic.DeleteFileByPath(savePath);
    }

    /// <summary>
    /// Gets Save Filepath for saving.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="fileEnding">File ending for the file.</param>
    /// <param name="fileFormatDescriptor"></param>
    /// <returns>Path to the file in which the object should be saved.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    private async Task<string> GetSaveFilepathAsync(string title, string fileEnding, string fileFormatDescriptor)
    {
        var filepath = await GetSaveFilepathAsync(title, new FileFilterProxy[]
        {
            new(fileFormatDescriptor, new[] { fileEnding })
        });
        if (!filepath.EndsWith($".{fileEnding}")) filepath += $".{fileEnding}";
        return filepath;
    }

    private async Task<string> GetSaveFilepathAsync(string title, FileFilterProxy[] fileFilterProxies)
    {
        try
        {
            var filepath = await _dialogManager!.ShowSaveAsDialogAsync(title, null, fileFilterProxies);
            return filepath;
        }
        catch (OperationCanceledException)
        {
            Logger.LogInformation("Save as dialog cancelled by user");
            throw;
        }
    }

    /// <summary>
    /// Gets Load Filepath for loading.
    /// </summary>
    /// <param name="title">Title of the dialog.</param>
    /// <param name="fileEnding">File ending for the file.</param>
    /// <param name="fileFormatDescriptor"></param>
    /// <returns>Path to the file which should be loaded.</returns>
    /// <exception cref="OperationCanceledException">Operation was cancelled by user.</exception>
    private async Task<string> GetLoadFilepathAsync(string title, string fileEnding, string fileFormatDescriptor)
    {
        var filepath = await GetLoadFilepathAsync(title, new FileFilterProxy[]
        {
            new(fileFormatDescriptor, new[] { fileEnding })
        });
        if (!filepath.EndsWith($".{fileEnding}")) filepath += $".{fileEnding}";
        return filepath;
    }

    private async Task<string> GetLoadFilepathAsync(string title, FileFilterProxy[] fileFilterProxies)
    {
        try
        {
            var filepath = await _dialogManager!.ShowOpenFileDialogAsync(title, null, fileFilterProxies);
            return filepath;
        }
        catch (OperationCanceledException)
        {
            Logger.LogInformation("Load dialog cancelled by user");
            throw;
        }
    }

    /// <summary>
    /// Performs sanity checks regarding Electron presence.
    /// </summary>
    /// <exception cref="NotImplementedException">Thrown when we are not running in Electron.</exception>
    /// <exception cref="InvalidOperationException">Thrown when we are running in Electron but no <see cref="IElectronDialogManager"/>
    /// implementation is present in dependency injection container.</exception>
    private void ElectronCheck()
    {
        if (!RunningElectron)
            throw new NotImplementedException("Browser upload/download not yet implemented");
        if (_dialogManager == null)
            throw new InvalidOperationException("dialogManager received from DI unexpectedly null");
    }

    #region LearningWorldSavePaths

    /// <inheritdoc cref="IPresentationLogic.GetWorldSavePath"/>
    public async Task<string> GetWorldSavePath()
    {
        ElectronCheck();
        var filepath = await GetLoadFilepathAsync("Load Learning World", WorldFileEnding, WorldFileFormatDescriptor);
        return filepath;
    }

    /// <inheritdoc cref="IPresentationLogic.LoadLearningWorldFromPath"/>
    public void LoadLearningWorldFromPath(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm, string path,
        bool setAsSelected = true)
    {
        var workspaceEntity = Mapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
        var command = WorldCommandFactory.GetLoadCommand(workspaceEntity, path, BusinessLogic,
            workspace => CMapper.Map(workspace, authoringToolWorkspaceVm));
        BusinessLogic.ExecuteCommand(command);
        if (!setAsSelected) return;
        var viewmodel = authoringToolWorkspaceVm.LearningWorlds.First(lw => lw.Id == command.LearningWorld!.Id);
        SelectedViewModelsProvider.SetLearningWorld(viewmodel, command);
    }

    public IEnumerable<IFileInfo> GetSavedLearningWorldPaths()
    {
        return BusinessLogic.GetSavedLearningWorldPaths();
    }

    #endregion

    #region BackendAccess

    public Task<bool> IsLmsConnected()
    {
        return BusinessLogic.IsLmsConnected();
    }

    public string LoginName => BusinessLogic.LoginName;

    public Task Login(string username, string password)
    {
        return BusinessLogic.Login(username, password);
    }

    public void Logout()
    {
        BusinessLogic.Logout();
    }

    public async Task<UploadResponseViewModel> ConstructAndUploadBackupAsync(ILearningWorldViewModel world,
        IProgress<int> progress,
        CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(world);
        var basePath = ApplicationPaths.SavedWorldsFolder;
        var fileName = $"{world.Name}.mbz";
        var filepath = Path.Join(basePath, fileName);
        try
        {
            BusinessLogic.ConstructBackup(entity, filepath);
            var response = await BusinessLogic.UploadLearningWorldToBackendAsync(filepath, progress, cancellationToken);
            return Mapper.Map<UploadResponseViewModel>(response);
        }
        finally
        {
            //delete file after upload or exception
            FileSystem.File.Delete(filepath);
        }
    }


    /// <inheritdoc cref="IPresentationLogic.GetLmsWorldList"/>
    public async Task DeleteLmsWorld(LmsWorldViewModel worldVm)
    {
        var world = Mapper.Map<LmsWorld>(worldVm);
        await BusinessLogic.DeleteLmsWorld(world);
    }

#if DEBUG

    public void ConstructDebugBackup(ILearningWorldViewModel world)
    {
        var entity = Mapper.Map<BusinessLogic.Entities.LearningWorld>(world);
        var basePath = ApplicationPaths.SavedWorldsFolder;
        var fileName = $"{world.Name}.mbz";
        var filepath = Path.Join(basePath, fileName);
        BusinessLogic.ConstructBackup(entity, filepath);
        Logger.LogDebug("Written debug backup to {Filepath}", filepath);
    }

#endif

    #endregion
}