using Presentation.Components.Forms.Models;

namespace TestHelpers;

public static class FormModelProvider
{
    
    public static LearningWorldFormModel GetLearningWorld()
    {
        return new LearningWorldFormModel();
    }

    public static TForm Get<TForm>() where TForm : class, new() =>
        (typeof(TForm).Name switch
        {
            nameof(LearningWorldFormModel) => GetLearningWorld() as TForm,
            _ => throw new ArgumentOutOfRangeException()
        })!;
}