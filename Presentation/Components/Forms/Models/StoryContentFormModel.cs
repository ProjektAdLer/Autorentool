using Shared;

namespace Presentation.Components.Forms.Models;

public class StoryContentFormModel : ILearningContentFormModel
{
    /// <summary>
    /// Parameterless constructor required for <see cref="BaseForm{TForm,TEntity}"/> TForm constraint.
    /// </summary>
    public StoryContentFormModel()
    {
        Name = "";
        StoryText = new List<string>();
        NpcMood = NpcMood.Friendly;
    }

    public List<string> StoryText { get; set; }

    public NpcMood NpcMood
    {
        get;
        set;
    }

    public string Name { get; set; }
}