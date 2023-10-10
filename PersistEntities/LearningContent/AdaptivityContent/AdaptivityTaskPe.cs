using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using PersistEntities.LearningContent.Question;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent;

[KnownType(typeof(MultipleChoiceMultipleResponseQuestionPe))]
[KnownType(typeof(MultipleChoiceSingleResponseQuestionPe))]
public class AdaptivityTaskPe : IAdaptivityTaskPe
{
    [JsonConstructor]
    public AdaptivityTaskPe(ICollection<IAdaptivityQuestionPe> questions, QuestionDifficulty? minimumRequiredDifficulty,
        string name)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Name = name;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTaskPe()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Name = "";
        Id = Guid.Empty;
    }

    [DataMember] public ICollection<IAdaptivityQuestionPe> Questions { get; set; }
    [DataMember] public QuestionDifficulty? MinimumRequiredDifficulty { get; set; }
    [DataMember] public string Name { get; set; }
    [IgnoreDataMember] public Guid Id { get; set; }


    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}