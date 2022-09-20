using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class SaveLearningWorldUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        const string filepath = "filepath";
        
        var command = new SaveLearningWorld(mockBusinessLogic, world, filepath);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveLearningWorld(world, filepath);
    }
}