using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

/// <summary>
/// 
/// </summary>
public interface IAdaptivityContent
{
    /// <summary>
    /// The main questions the content contains.
    /// </summary>
    public IEnumerable<IQuestion> Questions { get; set; }

    /// <summary>
    /// Additional questions that are not part of the main content, but are shown when certain rules are met.
    /// </summary>
    public IEnumerable<IQuestion> AdditionalQuestions { get; set; }

    /// <summary>
    /// The rules defining adaptivity on the content.
    /// </summary>
    public IEnumerable<IAdaptivityRule> Rules { get; set; }
}