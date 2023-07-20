using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Element;

public class LoadLearningElement : ILoadLearningElement
{
    public string Name => nameof(LoadLearningElement);
    internal IBusinessLogic BusinessLogic { get; }

    internal LearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement? LearningElement { get; private set; }
    internal string Filepath { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private ILogger<ElementCommandFactory> Logger { get; }
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public LoadLearningElement(LearningSpace parentSpace, int slotIndex, string filepath, IBusinessLogic businessLogic, 
        Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        Filepath = filepath;
        BusinessLogic = businessLogic;
        MappingAction = mappingAction;
        Logger = logger;
    }
    
    public LoadLearningElement(LearningSpace parentSpace, int slotIndex, Stream stream, IBusinessLogic businessLogic,
        Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        Filepath = "";
        BusinessLogic = businessLogic;
        LearningElement = BusinessLogic.LoadLearningElement(stream);
        MappingAction = mappingAction;
        Logger = logger;
    }
    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();

        LearningElement ??= BusinessLogic.LoadLearningElement(Filepath);
        LearningElement.Parent = ParentSpace;
        ParentSpace.LearningSpaceLayout.LearningElements[SlotIndex] = LearningElement;

        Logger.LogTrace("Loaded LearningElement {LearningElementName} ({LearningElementId}) into slot {SlotIndex} of LearningSpace {LearningSpaceName} ({LearningSpaceId})", LearningElement?.Name, LearningElement?.Id, SlotIndex, ParentSpace.Name ,ParentSpace.Id);

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

        Logger.LogTrace("Undone loading of LearningElement {LearningElementName} ({LearningElementId}) into slot {SlotIndex} of LearningSpace {LearningSpaceName} ({LearningSpaceId})", LearningElement?.Name, LearningElement?.Id, SlotIndex, ParentSpace.Name ,ParentSpace.Id);
        
        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing LoadLearningElement");
        Execute();
    }
}