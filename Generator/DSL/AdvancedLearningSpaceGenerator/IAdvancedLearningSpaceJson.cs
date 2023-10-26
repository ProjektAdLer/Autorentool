namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningSpaceJson : ILearningSpaceJson
{
    IAdvancedLearningSpaceLayoutJson AdvancedLearningSpaceLayout { get; set; }
}