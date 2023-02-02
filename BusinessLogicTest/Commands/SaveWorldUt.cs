using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class SaveWorldUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new World("a", "b", "c", "d", "e", "f");
        const string filepath = "filepath";
        
        var command = new SaveWorld(mockBusinessLogic, world, filepath);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveWorld(world, filepath);
    }
}