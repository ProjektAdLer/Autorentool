namespace AuthoringTool.Components.ModalDialog;

///<summary>Class that represents drop down input field with predetermined value choices.</summary>
public class ModalDialogDropdownInputField : ModalDialogInputField
{
    internal ModalDialogDropdownInputField(string name, IEnumerable<ModalDialogDropdownInputFieldChoiceMapping> valuesToChoiceMapping, bool required = false) : base(name, ModalDialogInputType.Text, required)
    {
        ValuesToChoiceMapping = valuesToChoiceMapping;
    }
    internal readonly IEnumerable<ModalDialogDropdownInputFieldChoiceMapping> ValuesToChoiceMapping;
}