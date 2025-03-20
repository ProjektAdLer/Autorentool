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

        var iconPath = elementModel switch
        {
            ElementModel.a_npc_alerobot => "CustomIcons/StoryElementModels/Moods/a-npc-alerobot/a-npc-alerobot",
            ElementModel.a_npc_defaultnpc => "CustomIcons/StoryElementModels/Moods/a-npc-defaultfemale/a-npc-defaultfemale",
            ElementModel.a_npc_bullyfemale => "CustomIcons/StoryElementModels/Moods/a-npc-bullyfemale/a-npc-bullyfemale",
            ElementModel.a_npc_bullymale => "CustomIcons/StoryElementModels/Moods/a-npc-bullymale/a-npc-bullymale",
            ElementModel.a_npc_dozentlukas => "CustomIcons/StoryElementModels/Moods/a-npc-dozentlukas/a-npc-dozentlukas",
            ElementModel.a_npc_hiphopfemale => "CustomIcons/StoryElementModels/Moods/a-npc-hiphopfemale/a-npc-hiphopfemale",
            ElementModel.a_npc_hiphopmale => "CustomIcons/StoryElementModels/Moods/a-npc-hiphopmale/a-npc-hiphopmale",
            ElementModel.a_npc_oldman => "CustomIcons/StoryElementModels/Moods/a-npc-oldman/a-npc-oldman",
            ElementModel.a_npc_santafemale => "CustomIcons/StoryElementModels/Moods/a-npc-santafemale/a-npc-santafemale",
            ElementModel.a_npc_santamale => "CustomIcons/StoryElementModels/Moods/a-npc-santamale/a-npc-santamale",
            ElementModel.a_npc_sheriffjustice => "CustomIcons/StoryElementModels/Moods/a-npc-sheriffjustice/a-npc-sheriffjustice",
            _ => throw new ArgumentOutOfRangeException(nameof(elementModel), elementModel,
                "There are no mood icons for this NPC.")
        };

        iconPath += npcMood switch
        {
            NpcMood.Happy => "-happy.png",
            NpcMood.Welcome => "-welcome.png",
            NpcMood.Thumpsup => "-thumbsup.png",
            NpcMood.Shocked => "-shocked.png",
            NpcMood.Disappointed => "-disappointed.png",
            NpcMood.Angry => "-angry.png",
            NpcMood.Tired => "-tired.png",
            _ => throw new ArgumentOutOfRangeException(nameof(npcMood), npcMood, null)
        };

        return iconPath;
        
    }
}