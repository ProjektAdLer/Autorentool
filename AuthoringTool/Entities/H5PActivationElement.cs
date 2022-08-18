using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

public class H5PActivationElement : LearningElement
{
    internal H5PActivationElement(string name, string shortname,  ILearningElementParent? parentName, LearningContent? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,learningContent, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
}