using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningSpace;
using Microsoft.AspNetCore.Components;

namespace AuthoringTool.PresentationLogic.ModalDialog;

class ModalDialogFactory : ILearningSpaceViewModalDialogFactory, ILearningWorldViewModalDialogFactory
{
    public ModalDialogFactory(ILearningSpaceViewModalDialogInputFieldsFactory spaceViewInputFieldsFactory,
        ILearningWorldViewModalDialogInputFieldsFactory worldViewInputFieldsFactory)
    {
        SpaceViewInputFieldsFactory = spaceViewInputFieldsFactory;
        WorldViewInputFieldsFactory = worldViewInputFieldsFactory;
    }

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

    private RenderFragment GetModalDialogFragmentInternal(string title, string text, ModalDialogOnClose onClose,
        ModalDialogType dialogType, IEnumerable<ModalDialogInputField> inputFields,
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