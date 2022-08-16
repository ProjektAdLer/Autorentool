using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

public class VideoActivationElement : LearningElement
{
    internal VideoActivationElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
}