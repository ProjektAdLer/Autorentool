using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.ModalDialog;

class ModalDialogInputFieldsFactory : ILearningSpaceViewModalDialogInputFieldsFactory, ILearningWorldViewModalDialogInputFieldsFactory
{
    /// <inheritdoc cref="ILearningSpaceViewModalDialogInputFieldsFactory.GetCreateLearningElementInputFields"/>
    public IEnumerable<ModalDialogInputField> GetCreateLearningElementInputFields(LearningContentViewModel? dragAndDropLearningContent, string spaceName)
    {
        var parentField = new ModalDialogDropdownInputField("Parent",
            new[]
            {
                new ModalDialogDropdownInputFieldChoiceMapping(null,
                    new[] {ElementParentEnum.Space.ToString()})
            }, true);
        
        var assignmentField = new ModalDialogDropdownInputField("Assignment",
            new[]
            {
                new ModalDialogDropdownInputFieldChoiceMapping(
                    new Dictionary<string, string> {{"Parent", ElementParentEnum.Space.ToString()}},
                    new []{spaceName}),
            }, true);
        
        return GetCreateLearningElementInputFieldsInternal(dragAndDropLearningContent, parentField, assignmentField);
    }

    /// <inheritdoc cref="ILearningWorldViewModalDialogInputFieldsFactory.GetCreateLearningElementInputFields"/>
    public IEnumerable<ModalDialogInputField> GetCreateLearningElementInputFields(LearningContentViewModel? dragAndDropLearningContent,
        IEnumerable<ILearningSpaceViewModel> learningSpaces, string worldName)
    {
        var parentField = GetParentDropdownInputField();
        var assignmentField = GetAssignmentDropdownInputField(learningSpaces, worldName);
        
        return GetCreateLearningElementInputFieldsInternal(dragAndDropLearningContent, parentField, assignmentField);
    }

    /// <summary>
    /// Gets the input fields for the modal dialog.
    /// </summary>
    /// <param name="dragAndDropLearningContent">Possibly drag-and-dropped learning content.</param>
    /// <param name="parentField">Dropdown input field of the element-parent.</param>
    /// <param name="assignmentField">Dropdown input field of the parent-assignment.</param>
    /// <returns>The input fields for the modal dialog.</returns>
    private IEnumerable<ModalDialogInputField> GetCreateLearningElementInputFieldsInternal(LearningContentViewModel? dragAndDropLearningContent,
        ModalDialogDropdownInputField parentField, ModalDialogDropdownInputField assignmentField)
    {
        ModalDialogDropdownInputField typeField;
        ModalDialogDropdownInputField contentField;
        if (dragAndDropLearningContent != null)
        {
            var contentType = GetContentType(dragAndDropLearningContent);
            typeField = GetTypeDropdownInputField(contentType);
            contentField = GetContentDropdownInputField(contentType);
        }
        else
        {
            typeField = GetDefaultTypeDropdownInputField();
            contentField = GetDefaultContentDropdownInputField();
        }

        return BuildModalDialogCreateElementInputFields(parentField, assignmentField, typeField, contentField);
    }

    /// <inheritdoc cref="ILearningWorldViewModalDialogInputFieldsFactory.GetCreateLearningSpaceInputFields"/>
    public IEnumerable<ModalDialogInputField> GetCreateLearningSpaceInputFields() =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new("Authors", ModalDialogInputType.Text),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text)
        };

    /// <inheritdoc cref="GetEditLearningSpaceInputFields"/>
    public IEnumerable<ModalDialogInputField> GetEditLearningSpaceInputFields() => GetCreateLearningSpaceInputFields();

    /// <inheritdoc cref="GetEditLearningElementInputFields"/>
    public IEnumerable<ModalDialogInputField> GetEditLearningElementInputFields() =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new("Authors", ModalDialogInputType.Text),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text),
            new ModalDialogDropdownInputField("Difficulty",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(null,
                        new[] {LearningElementDifficultyEnum.Easy.ToString(),
                            LearningElementDifficultyEnum.Medium.ToString(),
                            LearningElementDifficultyEnum.Hard.ToString(),
                            LearningElementDifficultyEnum.None.ToString() })
                }, true),
            new("Workload (min)", ModalDialogInputType.Number)
        };

    /// <summary>
    /// Builds modal dialog from input fields.
    /// </summary>
    /// <param name="parentField">Dropdown input field of the element-parent.</param>
    /// <param name="assignmentField">Dropdown input field of the parent-assignment.</param>
    /// <param name="typeField">Dropdown input field of the element type.</param>
    /// <param name="contentField">Dropdown input field of the content type.</param>
    /// <returns>IEnumerable of modal dialog input fields.</returns>
    private IEnumerable<ModalDialogInputField> BuildModalDialogCreateElementInputFields(
        ModalDialogDropdownInputField parentField, ModalDialogDropdownInputField assignmentField,
        ModalDialogDropdownInputField typeField, ModalDialogDropdownInputField contentField) =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            parentField,
            assignmentField,
            typeField,
            contentField,
            new("Authors", ModalDialogInputType.Text),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text),
            new ModalDialogDropdownInputField("Difficulty",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(null,
                        new[] {LearningElementDifficultyEnum.Easy.ToString(),
                            LearningElementDifficultyEnum.Medium.ToString(),
                            LearningElementDifficultyEnum.Hard.ToString() })
                }, true),
            new("Workload (min)", ModalDialogInputType.Number)
        };

    /// <summary>
    /// Gets dropdown input field of the element-parent.
    /// </summary>
    /// <returns>Modal dialog input field of the element-parent.</returns>
    private ModalDialogDropdownInputField GetParentDropdownInputField() =>
        new ("Parent",
            new[]
            {
                new ModalDialogDropdownInputFieldChoiceMapping(null,
                    new[] {ElementParentEnum.World.ToString(), ElementParentEnum.Space.ToString()})
            }, true);

    /// <summary>
    /// Gets dropdown input field of the parent-assignment.
    /// </summary>
    /// <param name="learningSpaces">LearningSpaces that already exist in the learning world.</param>
    /// <param name="worldName">Name of the learning world.</param>
    /// <returns>Modal dialog input field of the parent-assignment.</returns>
    private ModalDialogDropdownInputField GetAssignmentDropdownInputField(
        IEnumerable<ILearningSpaceViewModel> learningSpaces, string worldName) =>
        new ("Assignment",
            new[]
            {
                new ModalDialogDropdownInputFieldChoiceMapping(
                    new Dictionary<string, string> {{"Parent", ElementParentEnum.Space.ToString()}},
                    learningSpaces.Select(space => space.Name)),
                new ModalDialogDropdownInputFieldChoiceMapping(
                    new Dictionary<string, string> {{"Parent", ElementParentEnum.World.ToString()}},
                    new[] {worldName})
            }, true);


    /// <summary>
    /// Gets the content type from the file ending. 
    /// </summary>
    /// <param name="dragAndDropLearningContent">Drag-and-dropped learning content.</param>
    /// <returns>Content type</returns>
    /// <exception cref="Exception">Thrown when the file extension can not be mapped to a content type.</exception>
    private ContentTypeEnum GetContentType(LearningContentViewModel dragAndDropLearningContent) =>
        dragAndDropLearningContent.Type switch
        {
            "jpg" => ContentTypeEnum.Image,
            "png" => ContentTypeEnum.Image,
            "webp" => ContentTypeEnum.Image,
            "bmp" => ContentTypeEnum.Image,
            "mp4" => ContentTypeEnum.Video,
            "h5p" => ContentTypeEnum.H5P,
            "pdf" => ContentTypeEnum.Pdf,
            _ => throw new Exception(
                $"Can not map the file extension '{dragAndDropLearningContent.Type}' to a ContentType ")
        };

    /// <summary>
    /// Gets the element type fields from the content type.
    /// </summary>
    /// <param name="contentType">Content type of the drag-and-dropped learning content.</param>
    /// <returns>Modal dialog input field.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when content type is unknown.</exception>
    private ModalDialogDropdownInputField GetTypeDropdownInputField(ContentTypeEnum contentType)
    {
        return contentType switch
        {
            ContentTypeEnum.Image => new ModalDialogDropdownInputField("Type",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(null,
                        new[] { ElementTypeEnum.Transfer.ToString() })
                }, true),
            ContentTypeEnum.Video => new ModalDialogDropdownInputField("Type",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(null,
                        new[] { ElementTypeEnum.Transfer.ToString(), ElementTypeEnum.Activation.ToString() })
                }, true),
            ContentTypeEnum.Pdf => new ModalDialogDropdownInputField("Type",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(null,
                        new[] { ElementTypeEnum.Transfer.ToString() })
                }, true),
            ContentTypeEnum.H5P => new ModalDialogDropdownInputField("Type",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(null,
                        new[]
                        {
                            ElementTypeEnum.Activation.ToString(), ElementTypeEnum.Interaction.ToString(),
                            ElementTypeEnum.Test.ToString()
                        })
                },  true),
            _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, "Unknown content type")
        };
    }

    /// <summary>
    /// Gets the element type dropdown field depending on the content type.
    /// </summary>
    /// <param name="contentType">Content type of the drag-and-dropped learning content.</param>
    /// <returns>Modal dialog input field of the element type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when content type is unknown.</exception>
    private ModalDialogDropdownInputField GetContentDropdownInputField(ContentTypeEnum contentType)
    {
        return contentType switch
        {
            ContentTypeEnum.Image => new ModalDialogDropdownInputField("Content",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Transfer.ToString() } },
                        new[] { ContentTypeEnum.Image.ToString() })
                }, true),
            ContentTypeEnum.Video => new ModalDialogDropdownInputField("Content",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Transfer.ToString() } },
                        new[] { ContentTypeEnum.Video.ToString() }),
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Activation.ToString() } },
                        new[] { ContentTypeEnum.Video.ToString() })
                }, true),
            ContentTypeEnum.Pdf => new ModalDialogDropdownInputField("Content",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Transfer.ToString() } },
                        new[] { ContentTypeEnum.Pdf.ToString() })
                }, true),
            ContentTypeEnum.H5P => new ModalDialogDropdownInputField("Content",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Activation.ToString() } },
                        new[] { ContentTypeEnum.H5P.ToString() }),
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Interaction.ToString() } },
                        new[] { ContentTypeEnum.H5P.ToString() }),
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Test.ToString() } },
                        new[] { ContentTypeEnum.H5P.ToString() })
                }, true),
            _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, "Unknown content type")
        };
    }

    /// <summary>
    /// Gets the default element type dropdown field for modal dialog.
    /// </summary>
    /// <returns>Modal dialog input field for element type.</returns>
    private static ModalDialogDropdownInputField GetDefaultTypeDropdownInputField() =>
        new("Type",
            new[]
            {
                new ModalDialogDropdownInputFieldChoiceMapping(null,
                    new[]
                    {
                        ElementTypeEnum.Transfer.ToString(), ElementTypeEnum.Activation.ToString(),
                        ElementTypeEnum.Interaction.ToString(), ElementTypeEnum.Test.ToString()
                    })
            }, true);

    /// <summary>
    /// Gets the default content type dropdown field for modal dialog.
    /// </summary>
    /// <returns>Modal dialog input field for the content type.</returns>
    private static ModalDialogDropdownInputField GetDefaultContentDropdownInputField() =>
        new("Content",
            new[]
            {
                new ModalDialogDropdownInputFieldChoiceMapping(
                    new Dictionary<string, string> { { "Type", ElementTypeEnum.Transfer.ToString() } },
                    new[]
                    {
                        ContentTypeEnum.Image.ToString(), ContentTypeEnum.Video.ToString(),
                        ContentTypeEnum.Pdf.ToString()
                    }),
                new ModalDialogDropdownInputFieldChoiceMapping(
                    new Dictionary<string, string> { { "Type", ElementTypeEnum.Activation.ToString() } },
                    new[] { ContentTypeEnum.H5P.ToString(), ContentTypeEnum.Video.ToString() }),
                new ModalDialogDropdownInputFieldChoiceMapping(
                    new Dictionary<string, string> { { "Type", ElementTypeEnum.Interaction.ToString() } },
                    new[] { ContentTypeEnum.H5P.ToString() }),
                new ModalDialogDropdownInputFieldChoiceMapping(
                    new Dictionary<string, string> { { "Type", ElementTypeEnum.Test.ToString() } },
                    new[] { ContentTypeEnum.H5P.ToString() })
            }, true);
}