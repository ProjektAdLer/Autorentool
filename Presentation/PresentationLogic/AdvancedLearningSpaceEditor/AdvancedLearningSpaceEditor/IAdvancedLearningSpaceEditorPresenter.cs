using System.ComponentModel;
using Microsoft.AspNetCore.Components.Web;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;
using Shared;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public interface IAdvancedLearningSpaceEditorPresenter: INotifyPropertyChanged
{
    IAdvancedLearningSpaceViewModel? AdvancedLearningSpaceViewModel { get; }
    void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace);
    void CreateAdvancedLearningElementSlot(double positionX = 50, double positionY = 50);
    void CreateAdvancedDecoration(double positionX = 50, double positionY = 50);
    void DeleteAdvancedComponent(IAdvancedComponentViewModel advancedComponentViewModel);
    void DeleteCornerPoint(DoublePoint cornerPoint);
    void AddCornerPoint(MouseEventArgs mouseEventArgs);
    void RotateAdvancedComponent(IAdvancedComponentViewModel advancedComponentViewModel);
    IDictionary<int, DoublePoint> GetSpaceCornerPoints();
}