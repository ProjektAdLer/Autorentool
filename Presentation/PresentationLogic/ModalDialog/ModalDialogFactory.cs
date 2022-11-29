using Microsoft.AspNetCore.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;

namespace Presentation.PresentationLogic.ModalDialog;

public class ModalDialogFactory : ILearningSpaceViewModalDialogFactory, ILearningWorldViewModalDialogFactory, IAuthoringToolWorkspaceViewModalDialogFactory
{
    public ModalDialogFactory(ILearningSpaceViewModalDialogInputFieldsFactory spaceViewInputFieldsFactory,
        ILearningWorldViewModalDialogInputFieldsFactory worldViewInputFieldsFactory,
        IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory authoringToolWorkspaceViewModalDialogInputFieldsFactory)
    {
        SpaceViewInputFieldsFactory = spaceViewInputFieldsFactory;
        WorldViewInputFieldsFactory = worldViewInputFieldsFactory;
        AuthoringToolWorkspaceViewModalDialogInputFieldsFactory = authoringToolWorkspaceViewModalDialogInputFieldsFactory;
    }

    private IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory AuthoringToolWorkspaceViewModalDialogInputFieldsFactory { get; }
    private ILearningSpaceViewModalDialogInputFieldsFactory SpaceViewInputFieldsFactory { get; }
    private ILearningWorldViewModalDialogInputFieldsFactory WorldViewInputFieldsFactory { get; }

    /// <inheritdoc cref="ILearningSpaceViewModalDialogFactory.GetCreateLearningElementFragment"/>
    public RenderFragment GetCreateLearningElementFragment(
        LearningContentViewModel? dragAndDropLearningContent, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetCreateLearningElementInputFields(dragAndDropLearningContent);
        const string title = "Create new learning element";
        const string text = "Please enter the required data for the learning element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    /// <inheritdoc cref="ILearningWorldViewModalDialogFactory.GetCreateLearningSpaceFragment"/>
    public RenderFragment GetCreateLearningSpaceFragment(ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null)
    {
        var inputFields = WorldViewInputFieldsFactory.GetCreateLearningSpaceInputFields();
        const string title = "Create new learning space";
        const string text = "Please enter the required data for the learning space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields, annotations: annotations);
    }

    /// <inheritdoc cref="ILearningSpaceViewModalDialogFactory.GetEditLearningSpaceFragment"/>
    public RenderFragment GetEditLearningSpaceFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetEditLearningSpaceInputFields();
        const string title = "Edit existing learning space";
        const string text = "Please enter the required data for the learning space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues, annotations);
    }

    /// <inheritdoc cref="ILearningWorldViewModalDialogFactory.GetEditLearningSpaceFragment"/>
    RenderFragment ILearningWorldViewModalDialogFactory.GetEditLearningSpaceFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback, IDictionary<string, string>? annotations = null)
    {
        var inputFields = WorldViewInputFieldsFactory.GetEditLearningSpaceInputFields();
        const string title = "Edit existing learning space";
        const string text = "Please enter the required data for the learning space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues, annotations);
    }

    /// <inheritdoc cref="ILearningSpaceViewModalDialogFactory.GetEditLearningElementFragment"/>
    public RenderFragment GetEditLearningElementFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetEditLearningElementInputFields();
        const string title = "Edit existing learning element";
        const string text = "Please enter the required data for the learning element below:";
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

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetCreateLearningWorldFragment"/>
    public RenderFragment GetCreateLearningWorldFragment(ModalDialogOnClose onCloseCallback)
    {
        var inputFields = AuthoringToolWorkspaceViewModalDialogInputFieldsFactory.GetCreateLearningWorldInputFields();
        const string title = "Create new learning world";
        const string text = "Please enter the required data for the learning world below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetEditLearningWorldFragment"/>
    public RenderFragment GetEditLearningWorldFragment(IDictionary<string, string> initialInputValues,
        ModalDialogOnClose onCloseCallback)
    {
        var inputFields = AuthoringToolWorkspaceViewModalDialogInputFieldsFactory.GetEditLearningWorldInputFields();
        const string title = "Edit existing learning world";
        const string text = "Please enter the required data for the learning world below:";
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