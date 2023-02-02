using System.ComponentModel;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;

namespace Presentation.PresentationLogic.World;

public interface IWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging, IPositioningService, ISpaceNamesProvider
{
    bool CreateSpaceDialogOpen { get; }
    bool CreatePathWayConditionDialogOpen { get; }
    bool EditSpaceDialogOpen { get; }
    bool EditPathWayConditionDialogOpen { get; }
    Dictionary<string, string>? EditSpaceDialogInitialValues { get; }
    Dictionary<string, string>? EditConditionDialogInitialValues { get; }
    Dictionary<string, string>? EditSpaceDialogAnnotations { get; }
    IWorldViewModel? WorldVm { get; }
    bool SelectedObjectIsSpace { get; }
    bool ShowingSpaceView { get; }
    void DeleteSelectedObject();
    Task LoadSpaceAsync();
    Task SaveSelectedSpaceAsync();
    void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnCreatePathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OpenEditSelectedObjectDialog();
    void OnEditPathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void ShowSelectedSpaceView();
    void CloseSpaceView();
    void AddNewSpace();
    void AddNewPathWayCondition();
    void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition);
    void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> draggedEventArgs);
    void RightClickOnObjectInPathWay(IObjectInPathWayViewModel objectInPathWay);
    void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj);
    void DoubleClickOnSpaceInWorld(IObjectInPathWayViewModel obj);
    void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition);
    void HideRightClickMenu();
    IObjectInPathWayViewModel? RightClickedObject { get; }
    void EditObjectInPathWay(IObjectInPathWayViewModel obj);
    void DeleteSpace(ISpaceViewModel obj);
    void DeleteDraggableObject(IObjectInPathWayViewModel obj);
}