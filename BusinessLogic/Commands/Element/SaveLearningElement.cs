using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class SaveLearningElement : ICommand
{
    private readonly IBusinessLogic _businessLogic;
    private readonly LearningElement _learningElement;
    private readonly string _filepath;
    
    public SaveLearningElement(IBusinessLogic businessLogic, LearningElement learningElement, string filepath)
    {
        _businessLogic = businessLogic;
        _learningElement = learningElement;
        _filepath = filepath;
    }
    
    public void Execute()
    {
        _businessLogic.SaveLearningElement(_learningElement, _filepath);
    }
}