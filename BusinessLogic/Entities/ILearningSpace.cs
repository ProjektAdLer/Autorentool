using BusinessLogic.Entities.LearningOutcome;
using Shared;
using Shared.Theme;

namespace BusinessLogic.Entities;

public interface ILearningSpace : IObjectInPathWay
{
    //Guid Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    LearningOutcomeCollection LearningOutcomeCollection { get; }
    int RequiredPoints { get; set; }

    Theme Theme { get; set; }

    //bool UnsavedChanges { get; set; }
    ILearningSpaceLayout LearningSpaceLayout { get; set; }
    IEnumerable<ILearningElement> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;
    Topic? AssignedTopic { get; set; }
    bool InternalUnsavedChanges { get; }
}