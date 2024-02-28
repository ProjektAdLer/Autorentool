using System.ComponentModel;
using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.SelectedViewModels;

public interface ISelectedViewModelsProvider : INotifyPropertyChanged
{
    ILearningWorldViewModel? LearningWorld { get; }
    ISelectableObjectInWorldViewModel? LearningObjectInPathWay { get; }
    ILearningElementViewModel? LearningElement { get; }
    ILearningContentViewModel? LearningContent { get; }
    /// <summary>
    /// Active element slot in the learning space layout. Should not be set at the same time as <see cref="ActiveStorySlotInSpace"/>.
    /// </summary>
    int ActiveElementSlotInSpace { get; }
    /// <summary>
    /// Active story slot in the learning space layout. Should not be set at the same time as <see cref="ActiveElementSlotInSpace"/>.
    /// </summary>
    int ActiveStorySlotInSpace { get; }

    void SetLearningWorld(ILearningWorldViewModel? learningWorld, ICommand? command);
    void SetLearningObjectInPathWay(ISelectableObjectInWorldViewModel? learningObjectInPathWay, ICommand? command);
    void SetLearningElement(ILearningElementViewModel? learningElement, ICommand? command);
    void SetLearningContent(ILearningContentViewModel? content, ICommand? command);

    /// <summary>
    /// Sets the active element slot in the learning space layout.
    /// </summary>
    /// <param name="slot">The numeric index of the slot.</param>
    /// <param name="command">The command that ended up setting this slot, required to revert to previous selection when command is undone.</param>
    void SetActiveElementSlotInSpace(int slot, ICommand? command);

    /// <summary>
    /// Sets the active story slot in the learning space layout.
    /// </summary>
    /// <param name="slot">The numeric index of the slot.</param>
    /// <param name="command">The command that ended up setting this slot, required to revert to previous selection when command is undone.</param>
    void SetActiveStorySlotInSpace(int slot, ICommand? command);
}