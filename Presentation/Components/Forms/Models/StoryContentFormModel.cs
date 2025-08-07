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
        NpcName = "";
        NpcMood = NpcMood.Welcome;
        ExitAfterStorySequence = false;
    }

    public List<string> StoryText { get; set; }

    public string NpcName { get; set; }
    public NpcMood NpcMood { get; set; }

    public string Name { get; set; }
    public bool ExitAfterStorySequence { get; set; }
}