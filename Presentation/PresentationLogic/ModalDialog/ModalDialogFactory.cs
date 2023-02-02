using Microsoft.AspNetCore.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.Content;

namespace Presentation.PresentationLogic.ModalDialog;

public class ModalDialogFactory : ISpaceViewModalDialogFactory, IWorldViewModalDialogFactory, IAuthoringToolWorkspaceViewModalDialogFactory
{
    public ModalDialogFactory(ISpaceViewModalDialogInputFieldsFactory spaceViewInputFieldsFactory,
        IWorldViewModalDialogInputFieldsFactory worldViewInputFieldsFactory,
        IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory authoringToolWorkspaceViewModalDialogInputFieldsFactory)
    {
        SpaceViewInputFieldsFactory = spaceViewInputFieldsFactory;
        WorldViewInputFieldsFactory = worldViewInputFieldsFactory;
        AuthoringToolWorkspaceViewModalDialogInputFieldsFactory = authoringToolWorkspaceViewModalDialogInputFieldsFactory;
    }

    private IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory AuthoringToolWorkspaceViewModalDialogInputFieldsFactory { get; }
    private ISpaceViewModalDialogInputFieldsFactory SpaceViewInputFieldsFactory { get; }
    private IWorldViewModalDialogInputFieldsFactory WorldViewInputFieldsFactory { get; }

    /// <inheritdoc cref="ISpaceViewModalDialogFactory.GetCreateElementFragment"/>
    public RenderFragment GetCreateElementFragment(
        ContentViewModel? dragAndDropContent, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetCreateElementInputFields(dragAndDropContent);
        const string title = "Create new element";
        const string text = "Please enter the required data for the element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    /// <inheritdoc cref="IWorldViewModalDialogFactory.GetCreateSpaceFragment"/>
    public RenderFragment GetCreateSpaceFragment(ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null)
    {
        var inputFields = WorldViewInputFieldsFactory.GetCreateSpaceInputFields();
        const string title = "Create new space";
        const string text = "Please enter the required data for the space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields, annotations: annotations);
    }

    public RenderFragment GetCreatePathWayConditionFragment(ModalDialogOnClose onCloseCallback)
    {
        var inputFields = WorldViewInputFieldsFactory.GetCreatePathWayConditionInputFields();
        const string title = "Create new pathway condition";
        const string text = "Please choose between an and or an or pathway condition below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    /// <inheritdoc cref="ISpaceViewModalDialogFactory.GetEditSpaceFragment"/>
    public RenderFragment GetEditSpaceFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetEditSpaceInputFields();
        const string title = "Edit existing space";
        const string text = "Please enter the required data for the space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues, annotations);
    }

    /// <inheritdoc cref="ISpaceViewModalDialogFactory.GetEditSpaceFragment"/>
    public RenderFragment GetEditPathWayConditionFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = WorldViewInputFieldsFactory.GetEditPathWayConditionInputFields();
        const string title = "Edit existing pathway condition";
        const string text = "Please choose between an and or an or pathway condition below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields, initialInputValues);
    }

    /// <inheritdoc cref="IWorldViewModalDialogFactory.GetEditSpaceFragment"/>
    RenderFragment IWorldViewModalDialogFactory.GetEditSpaceFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null)
    {
        var inputFields = WorldViewInputFieldsFactory.GetEditSpaceInputFields();
        const string title = "Edit existing space";
        const string text = "Please enter the required data for the space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues, annotations);
    }

    /// <inheritdoc cref="ISpaceViewModalDialogFactory.GetEditElementFragment"/>
    public RenderFragment GetEditElementFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetEditElementInputFields();
        const string title = "Edit existing element";
        const string text = "Please enter the required data for the element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetInformationMessageFragment"/>
    public RenderFragment GetInformationMessageFragment(ModalDialogOnClose onCloseCallback, string informationMessage)
    {
        const string title = "Information";
        const ModalDialogType dialogType = ModalDialogType.Ok;
        return GetModalDialogFragmentInternal(title, informationMessage, onCloseCallback, dialogType);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetSaveUnsavedWorldsFragment"/>
    public RenderFragment GetSaveUnsavedWorldsFragment(ModalDialogOnClose onCloseCallback, string unsavedWorldName)
    {
        const string title = "Save unsaved worlds?";
        var text = $"World {unsavedWorldName} has unsaved changes. Do you want to save it?";
        const ModalDialogType dialogType = ModalDialogType.YesNoCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetDeleteUnsavedWorldFragment"/>
    public RenderFragment GetDeleteUnsavedWorldFragment(ModalDialogOnClose onCloseCallback,
        string deletedUnsavedWorldName)
    {
        const string title = "Save deleted world?";
        var text = $"Deleted world {deletedUnsavedWorldName} has unsaved changes. Do you want to save it?";
        const ModalDialogType dialogType = ModalDialogType.YesNo;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetCreateWorldFragment"/>
    public RenderFragment GetCreateWorldFragment(ModalDialogOnClose onCloseCallback)
    {
        var inputFields = AuthoringToolWorkspaceViewModalDialogInputFieldsFactory.GetCreateWorldInputFields();
        const string title = "Create new world";
        const string text = "Please enter the required data for the world below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetEditWorldFragment"/>
    public RenderFragment GetEditWorldFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback)
    {
        var inputFields = AuthoringToolWorkspaceViewModalDialogInputFieldsFactory.GetEditWorldInputFields();
        const string title = "Edit existing world";
        const string text = "Please enter the required data for the world below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetErrorStateFragment"/>
    public RenderFragment GetErrorStateFragment(ModalDialogOnClose onCloseCallback, string errorState)
    {
        const string title = "Exception encountered";
        const ModalDialogType dialogType = ModalDialogType.Ok;
        return GetModalDialogFragmentInternal(title, errorState, onCloseCallback, dialogType);
    }

    private RenderFragment GetModalDialogFragmentInternal(string title, string text, ModalDialogOnClose onClose,
        ModalDialogType dialogType, IEnumerable<ModalDialogInputField>? inputFields = null,
        IDictionary<string, string>? initialValues = null, IDictionary<string, string>? annotations = null)
    {
        return builder =>
        {
            builder.OpenComponent<Components.ModalDialog.ModalDialog>(0);
            builder.AddAttribute(1, "Title", title);
            builder.AddAttribute(2, "Text", text);
            builder.AddAttribute(3, "OnClose", onClose);
            builder.AddAttribute(4, "DialogType", dialogType);
            builder.AddAttribute(5, "InputFields", inputFields);
            builder.AddAttribute(6, "InputFieldsInitialValue", initialValues);
            builder.AddAttribute(7, "InputFieldsAnnotations", annotations);
            builder.CloseComponent();
        };
    }
}