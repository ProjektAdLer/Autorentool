namespace BusinessLogic.Entities;

public interface ILearningWorld
{
    List<ILearningSpace> LearningSpaces { get; set; }
    List<PathWayCondition> PathWayConditions { get; set; }
    List<IObjectInPathWay> ObjectsInPathWays { get; }
    List<LearningPathway> LearningPathways { get; set; }
    List<Topic> Topics { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Goals { get; set; }
    string SavePath { get; set; }
    ICollection<ILearningElement> UnplacedLearningElements { get; set; }
    bool UnsavedChanges { get; set; }
    Guid Id { get; }
    string Name { get; set; }
    bool InternalUnsavedChanges { get; }
    string EvaluationLink { get; set; }
    string EnrolmentKey { get; set; }
}