using Shared;

namespace Presentation.Components.Forms;

public static class NpcMoodPreviewHandler
{
    public static string GetIconForNpcAndMood(ElementModel elementModel, NpcMood npcMood)
    {
        if (!elementModel.ToString().Contains("a_npc"))
        {
            throw new ArgumentException("ElementModel is not an NPC", nameof(elementModel));
        }

        // Images are stored in the following directory: CustomIcons/StoryElementModels/Moods/{elementModel}/{elementModel}-{npcMood}.png
        var elementModelName = elementModel.ToString().ToLower().Replace("_", "-");
        var iconPath = "CustomIcons/StoryElementModels/" + elementModelName +
                       "/" + elementModelName +
                       "-" + npcMood.ToString().ToLower() + ".png";

        return iconPath;
    }
}