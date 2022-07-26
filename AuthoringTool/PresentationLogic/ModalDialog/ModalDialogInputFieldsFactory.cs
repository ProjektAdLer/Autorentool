using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.ModalDialog;

class ModalDialogInputFieldsFactory : ILearningSpaceViewModalDialogInputFieldsFactory
{
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

    public IEnumerable<ModalDialogInputField> GetEditLearningSpaceInputFields() =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
            new("Authors", ModalDialogInputType.Text),
            new("Description", ModalDialogInputType.Text, true),
            new("Goals", ModalDialogInputType.Text)
        };

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

    internal IEnumerable<ModalDialogInputField> BuildModalDialogCreateElementInputFields(ModalDialogDropdownInputField typeField,
        ModalDialogDropdownInputField contentField) =>
        new ModalDialogInputField[]
        {
            new("Name", ModalDialogInputType.Text, true),
            new("Shortname", ModalDialogInputType.Text, true),
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

    internal ContentTypeEnum GetContentType(LearningContentViewModel dragAndDropLearningContent) =>
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

    internal ModalDialogDropdownInputField GetTypeDropdownInputField(ContentTypeEnum contentType)
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
                }, true),
            _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, "Unknown content type")
        };
    }
    
    internal ModalDialogDropdownInputField GetContentDropdownInputField(ContentTypeEnum contentType)
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

    internal static ModalDialogDropdownInputField GetDefaultTypeDropdownInputField() =>
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

    internal static ModalDialogDropdownInputField GetDefaultContentDropdownInputField() =>
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