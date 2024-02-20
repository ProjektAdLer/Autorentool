using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningOutcome;

public class LearningOutcomeCollection
{
    public LearningOutcomeCollection(List<ILearningOutcome>? learningOutcomes = null)
    {
        LearningOutcomes = learningOutcomes ?? new List<ILearningOutcome>();
        UnsavedChanges = true;
    }

    [UsedImplicitly]
    private LearningOutcomeCollection()
    {
        LearningOutcomes = new List<ILearningOutcome>();
        UnsavedChanges = false;
    }

    public List<ILearningOutcome> LearningOutcomes { get; set; }

    public bool UnsavedChanges { get; set; }

    public IMemento GetMemento()
    {
        return new LearningOutcomeCollectionMemento(LearningOutcomes, UnsavedChanges);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningOutcomeCollectionMemento learningOutcomeCollectionMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        LearningOutcomes = learningOutcomeCollectionMemento.LearningOutcomes;
        UnsavedChanges = learningOutcomeCollectionMemento.UnsavedChanges;
    }

    private record LearningOutcomeCollectionMemento : IMemento
    {
        internal LearningOutcomeCollectionMemento(List<ILearningOutcome> learningOutcomes, bool unsavedChanges)
        {
            UnsavedChanges = unsavedChanges;
            LearningOutcomes = new List<ILearningOutcome>(learningOutcomes);
        }

        internal List<ILearningOutcome> LearningOutcomes { get; set; }

        public bool UnsavedChanges { get; }
    }
}