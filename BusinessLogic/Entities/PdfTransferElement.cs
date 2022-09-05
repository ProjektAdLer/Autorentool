using Shared;

namespace BusinessLogic.Entities;

public class PdfTransferElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private PdfTransferElement() : base()
    {
    }

    public PdfTransferElement(string name, string shortname,  ILearningElementParent? parent, LearningContent learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname, learningContent, authors, description, 
        goals, difficulty, parent, workload, positionX, positionY)
    {
    }
}