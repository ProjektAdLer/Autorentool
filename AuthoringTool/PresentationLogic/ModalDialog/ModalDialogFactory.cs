using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;
using Microsoft.AspNetCore.Components;

namespace AuthoringTool.PresentationLogic.ModalDialog;

class ModalDialogFactory : ILearningSpaceViewModalDialogFactory
{
    public ModalDialogFactory(ILearningSpaceViewModalDialogInputFieldsFactory inputFieldsFactory)
    {
        InputFieldsFactory = inputFieldsFactory;
    }

    public ILearningSpaceViewModalDialogInputFieldsFactory InputFieldsFactory { get; }

    public RenderFragment GetCreateLearningElementFragment(LearningContentViewModel? dragAndDropLearningContent,
        ModalDialogOnClose onCloseCallback)
    {
        var inputFields = InputFieldsFactory.GetCreateLearningElementInputFields(dragAndDropLearningContent);
        const string title = "Create new learning element";
        const string text = "Please enter the required data for the learning element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields);
    }

    public RenderFragment GetEditLearningSpaceFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputFields = InputFieldsFactory.GetEditLearningSpaceInputFields();
        const string title = "Edit existing learning space";
        const string text = "Please enter the required data for the learning space below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputFields,
            initialInputValues);
    }

    public RenderFragment GetEditLearningElementFragment(IDictionary<string, string> initialInputValues, ModalDialogOnClose onCloseCallback)
    {
        var inputfields = InputFieldsFactory.GetEditLearningElementInputFields();
        const string title = "Edit existing learning element";
        const string text = "Please enter the required data for the learning element below:";
        const ModalDialogType dialogType = ModalDialogType.OkCancel;
        return GetModalDialogFragmentInternal(title, text, onCloseCallback, dialogType, inputfields,
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