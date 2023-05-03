using System.ComponentModel;
using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.SelectedViewModels;

public interface ISelectedViewModelsProvider : INotifyPropertyChanged
{
    LearningWorldViewModel? LearningWorld { get; }
    ISelectableObjectInWorldViewModel? LearningObjectInPathWay { get; }
    ILearningElementViewModel? LearningElement { get; }
    
    void SetLearningWorld(LearningWorldViewModel? learningWorld, ICommand? command);
    void SetLearningObjectInPathWay(ISelectableObjectInWorldViewModel? learningObjectInPathWay, ICommand? command);
    void SetLearningElement(ILearningElementViewModel? learningElement, ICommand? command);
}