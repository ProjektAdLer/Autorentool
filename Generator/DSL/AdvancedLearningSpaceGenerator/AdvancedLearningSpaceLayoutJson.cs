using Newtonsoft.Json;
using Shared;

namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public class AdvancedLearningSpaceLayoutJson : IAdvancedLearningSpaceLayoutJson
{
    [JsonConstructor]
    public AdvancedLearningSpaceLayoutJson(List<AdvancedLearningElementSlotJson> advancedLearningElementSlots)
    {
        AdvancedLearningElementSlots = advancedLearningElementSlots;
    }

    public AdvancedLearningSpaceLayoutJson()
    {
        AdvancedLearningElementSlots = new List<AdvancedLearningElementSlotJson>();
    }

    public List<AdvancedLearningElementSlotJson> AdvancedLearningElementSlots { get; set; }
}