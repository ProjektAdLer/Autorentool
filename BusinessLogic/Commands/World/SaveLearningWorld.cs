using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class SaveLearningWorld : ICommand
{
    public string Name => nameof(SaveLearningWorld);
    private readonly IBusinessLogic _businessLogic;
    private readonly LearningWorld _learningWorld;
    private readonly string _filepath;
    
    public SaveLearningWorld(IBusinessLogic businessLogic, LearningWorld learningWorld, string filepath)
    {
        _businessLogic = businessLogic;
        _learningWorld = learningWorld;
        _filepath = filepath;
    }
    
    public void Execute()
    {
        _businessLogic.SaveLearningWorld(_learningWorld, _filepath);
    }
}