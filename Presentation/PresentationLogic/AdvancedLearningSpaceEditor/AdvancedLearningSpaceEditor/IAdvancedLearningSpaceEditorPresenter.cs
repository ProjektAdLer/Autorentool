using System.ComponentModel;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public interface IAdvancedLearningSpaceEditorPresenter: INotifyPropertyChanged
{
    IAdvancedLearningSpaceViewModel? AdvancedLearningSpaceViewModel { get; }
    void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace);
    void CreateAdvancedLearningElementSlot(double positionX = 50, double positionY = 50);
    void CreateAdvancedDecoration(double positionX = 50, double positionY = 50);
    void DeleteAdvancedComponent(IAdvancedComponentViewModel advancedComponentViewModel);
}