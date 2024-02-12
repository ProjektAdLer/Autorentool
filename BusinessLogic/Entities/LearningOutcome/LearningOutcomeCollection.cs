using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningOutcome;

public class LearningOutcomeCollection
{
    public LearningOutcomeCollection(List<ILearningOutcome>? learningOutcomes = null)
    {
        LearningOutcomes = learningOutcomes ?? new List<ILearningOutcome>();
    }

    [UsedImplicitly]
    private LearningOutcomeCollection()
    {
        LearningOutcomes = new List<ILearningOutcome>();
    }

    public List<ILearningOutcome> LearningOutcomes { get; set; }

    public IMemento GetMemento()
    {
        return new LearningOutcomeCollectionMemento(LearningOutcomes);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningOutcomeCollectionMemento learningOutcomeCollectionMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        LearningOutcomes = learningOutcomeCollectionMemento.LearningOutcomes;
    }

    private record LearningOutcomeCollectionMemento : IMemento
    {
        internal LearningOutcomeCollectionMemento(List<ILearningOutcome> learningOutcomes)
        {
            LearningOutcomes = new List<ILearningOutcome>(learningOutcomes);
        }

        internal List<ILearningOutcome> LearningOutcomes { get; set; }
    }
}