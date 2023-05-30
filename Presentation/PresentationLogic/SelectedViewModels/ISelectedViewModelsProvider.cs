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
    int ActiveSlot { get; }

    void SetLearningWorld(ILearningWorldViewModel? learningWorld, ICommand? command);
    void SetLearningObjectInPathWay(ISelectableObjectInWorldViewModel? learningObjectInPathWay, ICommand? command);
    void SetLearningElement(ILearningElementViewModel? learningElement, ICommand? command);
    void SetLearningContent(ILearningContentViewModel? content, ICommand? command);
    void SetActiveSlot(int slot);
}