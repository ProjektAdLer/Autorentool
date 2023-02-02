using System.ComponentModel;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Shared;

namespace Presentation.PresentationLogic.Space;

public interface ISpacePresenter : INotifyPropertyChanged
{
    void EditSpace(string name, string shortname, string authors, string description, string goals, int requiredPoints);
    bool EditSpaceDialogOpen { get; set; }
    IDictionary<string, string>? EditSpaceDialogInitialValues { get; }
    bool EditElementDialogOpen { get; set; }
    IDictionary<string, string>? EditElementDialogInitialValues { get; }
    bool CreateElementDialogOpen { get; set; }
    ISpaceViewModel? SpaceVm { get; }
    void SetSelectedElement(IElementViewModel element);
    void DeleteSelectedElement();
    void AddNewElement(int slotIndex);
    void AddNewElement();
    Task LoadElementAsync(int slotIndex);
    Task SaveSelectedElementAsync();
    Task ShowSelectedElementContentAsync();
    void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void EditSelectedElement();
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void SetSpace(ISpaceViewModel space);
    void CreateElementWithPreloadedContent(ContentViewModel content);
    ContentViewModel? DragAndDropContent { get; }
    IDisplayableObject? RightClickedObject { get; }
    void OnWorldPropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragElement(object sender, DraggedEventArgs<IElementViewModel> draggedEventArgs);
    void ClickedElement(IElementViewModel obj);
    void RightClickedElement(IElementViewModel obj);
    void EditElement(IElementViewModel obj);
    void EditElement(int slotIndex);
    void DeleteElement(IElementViewModel obj);
    void HideRightClickMenu();
    void ShowElementContent(IElementViewModel obj);
    void SetSpaceLayout(FloorPlanEnum floorPlanName);
}