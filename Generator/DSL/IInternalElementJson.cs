namespace Generator.DSL;

public interface IInternalElementJson : IElementJson
{
    int ElementMaxScore { get; set; }

    string ElementModel { get; set; }

    int LearningSpaceParentId { get; set; }
}