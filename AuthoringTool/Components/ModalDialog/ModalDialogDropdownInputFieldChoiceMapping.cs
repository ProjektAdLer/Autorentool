namespace AuthoringTool.Components.ModalDialog;

///<summary>Represents a mapping from required values to available choices in a <see cref="ModalDialogDropdownInputField"/>.</summary>
///<remarks>If no values are required for the Choices to be available, <see cref="RequiredValues"/> can be set to null.</remarks>
public record struct ModalDialogDropdownInputFieldChoiceMapping(IDictionary<string, string>? RequiredValues,
    IEnumerable<string> AvailableChoices);