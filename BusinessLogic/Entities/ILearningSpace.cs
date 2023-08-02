using Shared;

namespace BusinessLogic.Entities;

public interface ILearningSpace : IObjectInPathWay
{
    //Guid Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int RequiredPoints { get; set; }

    Theme Theme { get; set; }

    //bool UnsavedChanges { get; set; }
    ILearningSpaceLayout LearningSpaceLayout { get; set; }
    IEnumerable<ILearningElement> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;
    Topic? AssignedTopic { get; set; }
    bool InternalUnsavedChanges { get; }
}