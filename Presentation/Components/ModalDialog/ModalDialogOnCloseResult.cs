namespace Presentation.Components.ModalDialog;

/// <summary>
/// Represents the Result of closing a modal dialog.
/// </summary>
public class ModalDialogOnCloseResult
{
    /// <summary>
    /// Construct a new ModalDialogOnCloseResult with just a return value and no input fields.
    /// </summary>
    /// <param name="returnValue">The return value of the dialog.</param>
    public ModalDialogOnCloseResult(ModalDialogReturnValue returnValue)
    {
        ReturnValue = returnValue;
        InputFieldValues = null;
    }
    
    /// <summary>
    /// Construct a new ModalDialogOnCloseResult with a return value and values for input fields.
    /// </summary>
    /// <param name="returnValue">The return value of the dialog.</param>
    /// <param name="inputFieldValues">The values of the input fields of the dialog.</param>
    public ModalDialogOnCloseResult(ModalDialogReturnValue returnValue, IDictionary<string, string> inputFieldValues)
    {
        ReturnValue = returnValue;
        InputFieldValues = inputFieldValues;
    }
    
    /// <summary>
    /// The return value of the dialog.
    /// </summary>
    public ModalDialogReturnValue ReturnValue { get; }
    
    /// <summary>
    /// The values of the input fields of the dialog.
    /// </summary>
    public IDictionary<string, string>? InputFieldValues { get; }
    
    /// <summary>
    /// Deconstruct this ModalDialogOnCloseResult into its parameters.
    /// </summary>
    /// <param name="returnValue">The return value of the dialog.</param>
    /// <param name="inputFieldValues">The values of the input fields of the dialog.</param>
    public void Deconstruct(out ModalDialogReturnValue returnValue, out IDictionary<string, string>? inputFieldValues)
    {
        returnValue = ReturnValue;
        inputFieldValues = InputFieldValues;
    }
}