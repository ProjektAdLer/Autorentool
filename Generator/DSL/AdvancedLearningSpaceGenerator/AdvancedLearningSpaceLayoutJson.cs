using Newtonsoft.Json;
using Shared;

namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public class AdvancedLearningSpaceLayoutJson : IAdvancedLearningSpaceLayoutJson
{
    [JsonConstructor]
    public AdvancedLearningSpaceLayoutJson(
        List<AdvancedLearningElementSlotJson> advancedLearningElementSlots,
        List<AdvancedDecorationJson> advancedDecorations,
        List<AdvancedCornerPointJson> advancedCornerPoints,
        List<LearningElementJson> learningElements,
        Double entryDoorPositionX, Double entryDoorPositionY, 
        Double exitDoorPositionX, Double exitDoorPositionY)
    {
        AdvancedLearningElementSlots = advancedLearningElementSlots;
        AdvancedDecorations = advancedDecorations;
        AdvancedCornerPoints = advancedCornerPoints;
        LearningElements = learningElements;
        EntryDoorPositionX = entryDoorPositionX;
        EntryDoorPositionY = entryDoorPositionY;
        ExitDoorPositionX = exitDoorPositionX;
        ExitDoorPositionY = exitDoorPositionY;
    }

    public AdvancedLearningSpaceLayoutJson()
    {
        AdvancedLearningElementSlots = new List<AdvancedLearningElementSlotJson>();
        AdvancedDecorations = new List<AdvancedDecorationJson>();
        AdvancedCornerPoints = new List<AdvancedCornerPointJson>();
        LearningElements = new List<LearningElementJson>();
        EntryDoorPositionX = 0;
        EntryDoorPositionY = 0;
        ExitDoorPositionX = 0;
        ExitDoorPositionY = 0;
    }
    

    public List<AdvancedLearningElementSlotJson> AdvancedLearningElementSlots { get; set; }
    public List<AdvancedDecorationJson> AdvancedDecorations { get; set; }
    public List<AdvancedCornerPointJson> AdvancedCornerPoints { get; set; }
    public List<LearningElementJson> LearningElements { get; set; }
    public Double EntryDoorPositionX { get; set; }
    public Double EntryDoorPositionY { get; set; }
    public Double ExitDoorPositionX { get; set; }
    public Double ExitDoorPositionY { get; set; }
}