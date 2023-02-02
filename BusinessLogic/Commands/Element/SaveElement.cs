using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class SaveElement : ICommand
{
    private readonly IBusinessLogic _businessLogic;
    private readonly Entities.Element _element;
    private readonly string _filepath;
    
    public SaveElement(IBusinessLogic businessLogic, Entities.Element element, string filepath)
    {
        _businessLogic = businessLogic;
        _element = element;
        _filepath = filepath;
    }
    
    public void Execute()
    {
        _businessLogic.SaveElement(_element, _filepath);
    }
}