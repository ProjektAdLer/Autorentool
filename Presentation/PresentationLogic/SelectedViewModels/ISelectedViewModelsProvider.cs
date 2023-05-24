using System.ComponentModel;
using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.SelectedViewModels;

public interface ISelectedViewModelsProvider : INotifyPropertyChanged
{
    ILearningWorldViewModel? LearningWorld { get; }
    ISelectableObjectInWorldViewModel? LearningObjectInPathWay { get; }
    ILearningElementViewModel? LearningElement { get; }
    
    void SetLearningWorld(ILearningWorldViewModel? learningWorld, ICommand? command);
    void SetLearningObjectInPathWay(ISelectableObjectInWorldViewModel? learningObjectInPathWay, ICommand? command);
    void SetLearningElement(ILearningElementViewModel? learningElement, ICommand? command);
}