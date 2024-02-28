using Presentation.Components.Adaptivity.Forms.Models;
using Presentation.Components.Forms.Models;

namespace TestHelpers;

public static class FormModelProvider
{
    public static LearningWorldFormModel GetLearningWorld()
    {
        return new LearningWorldFormModel();
    }

    public static LearningSpaceFormModel GetLearningSpace()
    {
        return new LearningSpaceFormModel();
    }

    public static LearningElementFormModel GetLearningElement()
    {
        return new LearningElementFormModel();
    }

    public static ILearningContentFormModel GetFileContent()
    {
        return new FileContentFormModel();
    }

    public static LinkContentFormModel GetLinkContent()
    {
        return new LinkContentFormModel();
    }

    public static MultipleChoiceQuestionFormModel GetMultipleChoiceQuestion()
    {
        return new MultipleChoiceQuestionFormModel();
    }

    public static TForm Get<TForm>() where TForm : class, new() =>
        (typeof(TForm).Name switch
        {
            nameof(LearningWorldFormModel) => GetLearningWorld() as TForm,
            nameof(LearningSpaceFormModel) => GetLearningSpace() as TForm,
            nameof(LearningElementFormModel) => GetLearningElement() as TForm,
            nameof(LinkContentFormModel) => GetLinkContent() as TForm,
            nameof(MultipleChoiceQuestionFormModel) => GetMultipleChoiceQuestion() as TForm,
            _ => throw new ArgumentOutOfRangeException()
        })!;
}