using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class SaveSpace : ICommand
{
    private readonly IBusinessLogic _businessLogic;
    private readonly Entities.Space _space;
    private readonly string _filepath;
    
    public SaveSpace(IBusinessLogic businessLogic, Entities.Space space, string filepath)
    {
        _businessLogic = businessLogic;
        _space = space;
        _filepath = filepath;
    }
    
    public void Execute()
    {
        _businessLogic.SaveSpace(_space, _filepath);
    }
}