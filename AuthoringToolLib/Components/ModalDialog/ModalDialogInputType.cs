namespace AuthoringToolLib.Components.ModalDialog;

///<summary>Specifies which type of data is required in a <see cref="ModalDialogInputField"/>.</summary>
internal enum ModalDialogInputType
{
    //if we need more types, feel free to expand these, but please stick to the upper case versions
    //of the allowed value of the 'input' field on the html <input> tag 
    //see https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input
    //Image is useless, its just another submit button
    //For file, we have to implement an exception to use Electron.NET for the file dialog
    Text, Number, Date, Password, Email
}