namespace Presentation.Components.Forms.Models;

public class StoryContentFormModel : ILearningContentFormModel
{
    /// <summary>
    /// Parameterless constructor required for <see cref="BaseForm{TForm,TEntity}"/> TForm constraint.
    /// </summary>
    public StoryContentFormModel()
    {
        Name = "";
        StoryText = "";
    }

    public string Name { get; set; }
    public string StoryText { get; set; }
}