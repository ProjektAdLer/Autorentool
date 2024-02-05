using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.LearningOutcomes;

public class DeleteLearningOutcome : IDeleteLearningOutcome
{
    private IMemento? _memento;

    public DeleteLearningOutcome(LearningOutcomeCollection learningOutcomeCollection, ILearningOutcome learningOutcome,
        Action<LearningOutcomeCollection> mappingAction, ILogger<DeleteLearningOutcome> logger)
    {
        LearningOutcomeCollection = learningOutcomeCollection;
        LearningOutcome = learningOutcomeCollection.LearningOutcomes.First(lo => lo.Id == learningOutcome.Id);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningOutcomeCollection LearningOutcomeCollection { get; }
    internal ILearningOutcome LearningOutcome { get; }
    internal Action<LearningOutcomeCollection> MappingAction { get; }
    private ILogger<DeleteLearningOutcome> Logger { get; }

    public string Name => nameof(DeleteLearningOutcome);

    public void Execute()
    {
        _memento = LearningOutcomeCollection.GetMemento();

        LearningOutcomeCollection.LearningOutcomes.Remove(LearningOutcome);

        Logger.LogTrace("Deleted LearningOutcome {@LearningOutcome} from LearningOutcomeCollection",
            LearningOutcome.Id);

        MappingAction.Invoke(LearningOutcomeCollection);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningOutcomeCollection.RestoreMemento(_memento);

        Logger.LogTrace("Undone deleting LearningOutcome from LearningOutcomeCollection {@LearningOutcomeCollection}",
            LearningOutcomeCollection);

        MappingAction.Invoke(LearningOutcomeCollection);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing delete LearningOutcome from LearningOutcomeCollection");
        Execute();
    }
}