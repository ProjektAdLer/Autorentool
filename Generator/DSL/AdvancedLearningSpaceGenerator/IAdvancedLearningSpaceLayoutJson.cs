using Shared;

namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningSpaceLayoutJson
{
    List<AdvancedLearningElementSlotJson> AdvancedLearningElementSlots { get; set; }
}