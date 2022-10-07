using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.PresentationLogic.ModalDialog;

public class ModalDialogInputFieldsFactory : ILearningSpaceViewModalDialogInputFieldsFactory, ILearningWorldViewModalDialogInputFieldsFactory, IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory
{
    /// <inheritdoc cref="ILearningSpaceViewModalDialogInputFieldsFactory.GetCreateLearningElementInputFields"/>
    public IEnumerable<ModalDialogInputField> GetCreateLearningElementInputFields(LearningContentViewModel? dragAndDropLearningContent)
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

        return BuildModalDialogCreateElementInputFields(typeField, contentField);
    }

    /// <inheritdoc cref="ILearningWorldViewModalDialogInputFieldsFactory.GetCreateLearningSpaceInputFields"/>
    public IEnumerable<ModalDialogInputField> GetCreateLearningSpaceInputFields() =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new("Authors", ModalDialogInputType.Text),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text),
            new("Required Points", ModalDialogInputType.Number)
        };

    /// <inheritdoc cref="ILearningWorldViewModalDialogInputFieldsFactory.GetEditLearningSpaceInputFields"/>
    public IEnumerable<ModalDialogInputField> GetEditLearningSpaceInputFields() => GetCreateLearningSpaceInputFields();

    /// <inheritdoc cref="ILearningSpaceViewModalDialogInputFieldsFactory.GetEditLearningElementInputFields"/>
    public IEnumerable<ModalDialogInputField> GetEditLearningElementInputFields() =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new("Url", ModalDialogInputType.Text),
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
            new("Workload (min)", ModalDialogInputType.Number),
            new("Points", ModalDialogInputType.Number)
        };

    /// <summary>
    /// Builds modal dialog from input fields.
    /// </summary>
    /// <param name="typeField">Dropdown input field of the element type.</param>
    /// <param name="contentField">Dropdown input field of the content type.</param>
    /// <returns>IEnumerable of modal dialog input fields.</returns>
    private IEnumerable<ModalDialogInputField> BuildModalDialogCreateElementInputFields(
        ModalDialogDropdownInputField typeField, ModalDialogDropdownInputField contentField) =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            typeField,
            contentField,
            new("Url", ModalDialogInputType.Text),
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
            new("Workload (min)", ModalDialogInputType.Number),
            new("Points", ModalDialogInputType.Number)
        };


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
            "txt" => ContentTypeEnum.Text, 
            "c" => ContentTypeEnum.Text, 
            "h" => ContentTypeEnum.Text, 
            "cpp" => ContentTypeEnum.Text, 
            "cc" => ContentTypeEnum.Text, 
            "c++" => ContentTypeEnum.Text, 
            "py" => ContentTypeEnum.Text, 
            "cs" => ContentTypeEnum.Text, 
            "js" => ContentTypeEnum.Text, 
            "php" => ContentTypeEnum.Text, 
            "html" => ContentTypeEnum.Text, 
            "css" => ContentTypeEnum.Text,
            //"mp4" => ContentTypeEnum.Video,
            "h5p" => ContentTypeEnum.H5P,
            "pdf" => ContentTypeEnum.PDF,
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
            ContentTypeEnum.Text => new ModalDialogDropdownInputField("Type",
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
            ContentTypeEnum.PDF => new ModalDialogDropdownInputField("Type",
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
            ContentTypeEnum.Text => new ModalDialogDropdownInputField("Content",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Transfer.ToString() } },
                        new[] { ContentTypeEnum.Text.ToString() })
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
            ContentTypeEnum.PDF => new ModalDialogDropdownInputField("Content",
                new[]
                {
                    new ModalDialogDropdownInputFieldChoiceMapping(
                        new Dictionary<string, string> { { "Type", ElementTypeEnum.Transfer.ToString() } },
                        new[] { ContentTypeEnum.PDF.ToString() })
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
                        ContentTypeEnum.Image.ToString(), ContentTypeEnum.Text.ToString(),
                        ContentTypeEnum.Video.ToString(), ContentTypeEnum.PDF.ToString()
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

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory.GetCreateLearningWorldInputFields"/>
    public IEnumerable<ModalDialogInputField> GetCreateLearningWorldInputFields() =>
        new ModalDialogInputField[]
        {
                new("Name", ModalDialogInputType.Text, true),
                new("Shortname", ModalDialogInputType.Text, true),
                new("Authors", ModalDialogInputType.Text),
                new("Language", ModalDialogInputType.Text, true),
                new("Description", ModalDialogInputType.Text, true),
                new("Goals", ModalDialogInputType.Text)
        };

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory.GetEditLearningWorldInputFields"/>
    public IEnumerable<ModalDialogInputField> GetEditLearningWorldInputFields() => GetCreateLearningWorldInputFields();
}