using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class SaveLearningSpace : ISaveLearningSpace
{
    public string Name => nameof(SaveLearningSpace);
    internal IBusinessLogic BusinessLogic { get; }
    internal LearningSpace LearningSpace { get; }
    internal string Filepath { get; }
    
    public SaveLearningSpace(IBusinessLogic businessLogic, LearningSpace learningSpace, string filepath)
    {
        BusinessLogic = businessLogic;
        LearningSpace = learningSpace;
        Filepath = filepath;
    }
    
    public void Execute()
    {
        BusinessLogic.SaveLearningSpace(LearningSpace, Filepath);
    }
}