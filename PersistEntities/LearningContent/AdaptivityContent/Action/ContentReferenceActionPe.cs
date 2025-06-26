using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningContent.Action;

public class ContentReferenceActionPe : IAdaptivityActionPe
{
    /// <summary>
    /// Creates a new instance of <see cref="ContentReferenceActionPe"/>.
    /// </summary>
    /// <param name="content">Content to be referenced. Must not be <see cref="IAdaptivityContentPe"/>.</param>
    /// <param name="comment">A comment for the action.</param>
    /// <exception cref="ArgumentException">Content was <see cref="IAdaptivityContentPe"/>.</exception>
    public ContentReferenceActionPe(ILearningContentPe content, string comment)
    {
        if (content is IAdaptivityContentPe)
            throw new ArgumentException("Content cannot be an adaptivity content", nameof(content));
        Content = content;
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ContentReferenceActionPe()
    {
        Content = null!;
        Comment = "";
        Id = Guid.Empty;
    }

    [DataMember] public ILearningContentPe Content { get; set; }
    [DataMember] public string Comment { get; set; }
    [IgnoreDataMember] public Guid Id { get; private set; }

    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}