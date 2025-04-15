namespace BusinessLogic.Entities.LearningContent.LinkContent;

public interface ILinkContent : ILearningContent
{
    string Link { get; set; }
    bool IsDeleted { get; set; }
}