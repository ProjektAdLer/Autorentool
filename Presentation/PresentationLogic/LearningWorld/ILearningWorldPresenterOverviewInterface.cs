using System.ComponentModel;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenterOverviewInterface : ISelectedSetter
{
    ILearningWorldViewModel? LearningWorldVm { get; }
    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
    event PropertyChangedEventHandler? PropertyChanged;
    event PropertyChangingEventHandler? PropertyChanging;
}