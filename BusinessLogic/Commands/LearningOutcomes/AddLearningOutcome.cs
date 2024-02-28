using System.Globalization;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared.LearningOutcomes;

namespace BusinessLogic.Commands.LearningOutcomes;

public class AddLearningOutcome : IAddLearningOutcome
{
    private IMemento? _memento;

    public AddLearningOutcome(LearningOutcomeCollection learningOutcomeCollection, TaxonomyLevel taxonomyLevel,
        string what, string verbOfVisibility, string whereby, string whatFor, CultureInfo cultureInfo,
        Action<LearningOutcomeCollection> mappingAction, ILogger<AddLearningOutcome> logger, int index)
    {
        LearningOutcomeCollection = learningOutcomeCollection;
        LearningOutcome =
            new StructuredLearningOutcome(taxonomyLevel, what, whereby, whatFor, verbOfVisibility, cultureInfo);
        MappingAction = mappingAction;
        Logger = logger;
        Index = index;
    }

    public AddLearningOutcome(LearningOutcomeCollection learningOutcomeCollection, string manualLearningOutcomeText,
        Action<LearningOutcomeCollection> mappingAction, ILogger<AddLearningOutcome> logger, int index)
    {
        LearningOutcomeCollection = learningOutcomeCollection;
        LearningOutcome = new ManualLearningOutcome(manualLearningOutcomeText);
        MappingAction = mappingAction;
        Logger = logger;
        Index = index;
    }

    internal LearningOutcomeCollection LearningOutcomeCollection { get; }
    internal ILearningOutcome LearningOutcome { get; }
    internal Action<LearningOutcomeCollection> MappingAction { get; }
    private ILogger<AddLearningOutcome> Logger { get; }
    internal int Index { get; }
    public string Name => nameof(AddLearningOutcome);

    public void Execute()
    {
        _memento = LearningOutcomeCollection.GetMemento();

        if (Index < 0)
        {
            LearningOutcomeCollection.LearningOutcomes.Add(LearningOutcome);
        }
        else
        {
            LearningOutcomeCollection.LearningOutcomes.Insert(Index, LearningOutcome);
        }

        LearningOutcomeCollection.UnsavedChanges = true;

        Logger.LogTrace("Added LearningOutcome {@LearningOutcome} to LearningOutcomeCollection", LearningOutcome.Id);

        MappingAction.Invoke(LearningOutcomeCollection);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningOutcomeCollection.RestoreMemento(_memento);

        Logger.LogTrace("Undone adding LearningOutcome to LearningOutcomeCollection {@LearningOutcomeCollection}",
            LearningOutcomeCollection);

        MappingAction.Invoke(LearningOutcomeCollection);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing add LearningOutcome to LearningOutcomeCollection");
        Execute();
    }
}