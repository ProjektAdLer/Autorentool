namespace AuthoringToolLib.Components.ModalDialog;

///<summary>Class that represents free form input field in a modal dialog with free values.</summary>
public class ModalDialogInputField
{
    internal ModalDialogInputField(string name, ModalDialogInputType type, bool required = false)
    {
        Name = name;
        Type = type;
        Required = required;
    }
    ///<summary>The name of the input field or data that is to be submitted.</summary>
    internal readonly string Name;
    ///<summary>The type of input for the field.</summary>
    internal readonly ModalDialogInputType Type;
    ///<summary>Whether or not the field is required.</summary>
    internal readonly bool Required;
}