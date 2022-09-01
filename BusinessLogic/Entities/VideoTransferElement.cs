using Shared;

namespace BusinessLogic.Entities;

public class VideoTransferElement : LearningElement
{
    public VideoTransferElement(string name, string shortname,  ILearningElementParent? parent, LearningContent learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname, learningContent, authors, description, 
        goals, difficulty, parent, workload, positionX, positionY)
    {
    }
}