using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Element;

public class DeleteLearningElementInSpace : IDeleteLearningElementInSpace
{
    public string Name => nameof(DeleteLearningElementInSpace);
    internal LearningElement LearningElement { get; }
    internal LearningSpace ParentSpace { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private ILogger<ElementCommandFactory> Logger { get; }
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public DeleteLearningElementInSpace(LearningElement learningElement, LearningSpace parentSpace,
        Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();

        ParentSpace.UnsavedChanges = true;
        var kvP = ParentSpace.LearningSpaceLayout.LearningElements.First(x => x.Value.Id == LearningElement.Id);
        
        ParentSpace.LearningSpaceLayout.LearningElements.Remove(kvP.Key);

        Logger.LogTrace("Deleted LearningElement {LearningElementName} ({LearningElementId}) in LearningSpace {LearningSpaceName} ({LearningSpaceId})", LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);
        
        MappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        ParentSpace.RestoreMemento(_memento);
        ParentSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        Logger.LogTrace("Undone deletion of LearningElement {LearningElementName} ({LearningElementId}) in LearningSpace {LearningSpaceName} ({LearningSpaceId}). Restored LearningSpace to previous state", LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);
        
        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningElementInSpace");
        Execute();
    }
}