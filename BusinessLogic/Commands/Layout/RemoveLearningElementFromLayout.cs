using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

public class RemoveLearningElementFromLayout : IUndoCommand
{
    public string Name => nameof(RemoveLearningElementFromLayout);
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal ILearningElement LearningElement { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _mementoWorld;
    private IMemento? _mementoSpace;
    private IMemento? _mementoSpaceLayout;

    public RemoveLearningElementFromLayout(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        LearningSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningSpace.Id);
        LearningElement = LearningSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _mementoWorld = LearningWorld.GetMemento();
        _mementoSpace = LearningSpace.GetMemento();
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();

        LearningSpace.UnsavedChanges = true;

        var oldSlot =
            LearningSpace.LearningSpaceLayout.LearningElements.First(kvP => kvP.Value.Equals(LearningElement)).Key;

        LearningSpace.LearningSpaceLayout.LearningElements.Remove(oldSlot);

        if (LearningWorld.UnplacedLearningElements.Contains(LearningElement) == false)
        {
            LearningWorld.UnplacedLearningElements.Add(LearningElement);
        }

        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_mementoWorld is null)
        {
            throw new InvalidOperationException("_mementoWorld is null");
        }

        if (_mementoSpace is null)
        {
            throw new InvalidOperationException("_mementoSpace is null");
        }

        if (_mementoSpaceLayout is null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        LearningWorld.RestoreMemento(_mementoWorld);
        LearningSpace.RestoreMemento(_mementoSpace);
        LearningSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}