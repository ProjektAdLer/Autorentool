using AuthoringToolLib.PresentationLogic.LearningElement;

namespace AuthoringToolLib.Entities;

public class PdfTransferElement : LearningElement
{
    internal PdfTransferElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
}