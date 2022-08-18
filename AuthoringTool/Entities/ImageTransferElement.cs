using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

public class ImageTransferElement : LearningElement
{
    internal ImageTransferElement(string name, string shortname,  ILearningElementParent? parentName, LearningContent? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,learningContent, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
}