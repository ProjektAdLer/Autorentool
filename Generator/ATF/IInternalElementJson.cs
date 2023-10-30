namespace Generator.ATF;

public interface IInternalElementJson : IElementJson
{
    int ElementMaxScore { get; set; }
    string ElementModel { get; set; }
    int LearningSpaceParentId { get; set; }
    string? ElementDescription { get; set; }
    string[] ElementGoals { get; set; }
}