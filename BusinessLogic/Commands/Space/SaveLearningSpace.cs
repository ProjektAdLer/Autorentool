using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class SaveLearningSpace : ICommand
{
    public string Name => nameof(SaveLearningSpace);
    private readonly IBusinessLogic _businessLogic;
    private readonly LearningSpace _learningSpace;
    private readonly string _filepath;
    
    public SaveLearningSpace(IBusinessLogic businessLogic, LearningSpace learningSpace, string filepath)
    {
        _businessLogic = businessLogic;
        _learningSpace = learningSpace;
        _filepath = filepath;
    }
    
    public void Execute()
    {
        _businessLogic.SaveLearningSpace(_learningSpace, _filepath);
    }
}