using System.Runtime.Serialization;
using JetBrains.Annotations;
using PersistEntities.LearningContent.Question;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent;

[KnownType(typeof(MultipleChoiceMultipleResponseQuestionPe))]
[KnownType(typeof(MultipleChoiceSingleResponseQuestionPe))]
public class AdaptivityTaskPe : IAdaptivityTaskPe
{
    public AdaptivityTaskPe(ICollection<IAdaptivityQuestionPe> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTaskPe()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Id = Guid.Empty;
    }

    [DataMember] public ICollection<IAdaptivityQuestionPe> Questions { get; set; }
    [DataMember] public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
    [IgnoreDataMember] public Guid Id { get; set; }
    
    
    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}