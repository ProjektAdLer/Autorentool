using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningSpace;
using Microsoft.AspNetCore.Components;

namespace AuthoringTool.PresentationLogic.ModalDialog;

class ModalDialogFactory : ILearningSpaceViewModalDialogFactory, ILearningWorldViewModalDialogFactory, IAuthoringToolWorkspaceViewModalDialogFactory
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
    RenderFragment ILearningSpaceViewModalDialogFactory.GetCreateLearningElementFragment(
        LearningContentViewModel? dragAndDropLearningContent, ModalDialogOnClose onCloseCallback, string spaceName)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetCreateLearningElementInputFields(dragAndDropLearningContent, spaceName);
        const string title = "Create new learning element";
        const string text = "Please enter the required data for the learning element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }
    
    /// <inheritdoc cref="ILearningWorldViewModalDialogFactory.GetCreateLearningElementFragment"/>
    RenderFragment ILearningWorldViewModalDialogFactory.GetCreateLearningElementFragment(LearningContentViewModel? dragAndDropLearningContent,
        IEnumerable<ILearningSpaceViewModel> learningSpaces, string worldName, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = WorldViewInputFieldsFactory.GetCreateLearningElementInputFields(dragAndDropLearningContent, learningSpaces, worldName);
        const string title = "Create new learning element";
        const string text = "Please enter the required data for the learning element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    /// <inheritdoc cref="ILearningWorldViewModalDialogFactory.GetCreateLearningSpaceFragment"/>
    public RenderFragment GetCreateLearningSpaceFragment(ModalDialogOnClose onCloseCallback)
    {
        var inputFields = WorldViewInputFieldsFactory.GetCreateLearningSpaceInputFields();
        const string title = "Create new learning space";
        const string text = "Please enter the required data for the learning space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    /// <inheritdoc cref="ILearningSpaceViewModalDialogFactory.GetEditLearningSpaceFragment"/>
    RenderFragment ILearningSpaceViewModalDialogFactory.GetEditLearningSpaceFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetEditLearningSpaceInputFields();
        const string title = "Edit existing learning space";
        const string text = "Please enter the required data for the learning space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues);
    }
    
    /// <inheritdoc cref="ILearningWorldViewModalDialogFactory.GetEditLearningSpaceFragment"/>
    RenderFragment ILearningWorldViewModalDialogFactory.GetEditLearningSpaceFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = WorldViewInputFieldsFactory.GetEditLearningSpaceInputFields();
        const string title = "Edit existing learning space";
        const string text = "Please enter the required data for the learning space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues);
    }

    /// <inheritdoc cref="ILearningSpaceViewModalDialogFactory.GetEditLearningElementFragment"/>
    RenderFragment ILearningSpaceViewModalDialogFactory.GetEditLearningElementFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = SpaceViewInputFieldsFactory.GetEditLearningElementInputFields();
        const string title = "Edit existing learning element";
        const string text = "Please enter the required data for the learning element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues);
    }
    
    /// <inheritdoc cref="ILearningWorldViewModalDialogFactory.GetEditLearningElementFragment"/>
    RenderFragment ILearningWorldViewModalDialogFactory.GetEditLearningElementFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = WorldViewInputFieldsFactory.GetEditLearningElementInputFields();
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

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetReplaceWorldFragment"/>
    public RenderFragment GetReplaceWorldFragment(ModalDialogOnClose onCloseCallback, string worldToReplaceWithName)
    {
        const string title = "Replace world?";
        var text = $"You already have a world with the name {worldToReplaceWithName} loaded." +
                   " Do you want to replace it?";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogFactory.GetReplaceUnsavedWorldFragment"/>
    public RenderFragment GetReplaceUnsavedWorldFragment(ModalDialogOnClose onCloseCallback,
        string replacedUnsavedWorldName)
    {
        const string title = "Save replaced world?";
        var text = $"Replaced world {replacedUnsavedWorldName} has unsaved changes. Do you want to save it?";
        const ModalDialogType dialogType = ModalDialogType.YesNo;
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
        IDictionary<string, string>? initialValues = null)
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
            builder.CloseComponent();
        };
    }
}