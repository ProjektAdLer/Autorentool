using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class SaveWorld : ICommand
{
    private readonly IBusinessLogic _businessLogic;
    private readonly Entities.World _world;
    private readonly string _filepath;
    
    public SaveWorld(IBusinessLogic businessLogic, Entities.World world, string filepath)
    {
        _businessLogic = businessLogic;
        _world = world;
        _filepath = filepath;
    }
    
    public void Execute()
    {
        _businessLogic.SaveWorld(_world, _filepath);
    }
}