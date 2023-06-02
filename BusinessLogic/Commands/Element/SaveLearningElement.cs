using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class SaveLearningElement : ISaveLearningElement
{
    public string Name => nameof(SaveLearningElement);
    internal IBusinessLogic BusinessLogic { get; }
    internal LearningElement LearningElement { get; }
    internal string Filepath { get; }
    
    public SaveLearningElement(IBusinessLogic businessLogic, LearningElement learningElement, string filepath)
    {
        BusinessLogic = businessLogic;
        LearningElement = learningElement;
        Filepath = filepath;
    }
    
    public void Execute()
    {
        BusinessLogic.SaveLearningElement(LearningElement, Filepath);
    }
}