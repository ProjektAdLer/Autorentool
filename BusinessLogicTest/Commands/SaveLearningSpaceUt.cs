using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class SaveLearningSpaceUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "d", "e", 5);
        const string filepath = "c:\\temp\\test";
        
        var command = new SaveLearningSpace(mockBusinessLogic, space, filepath);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveLearningSpace(space,filepath);
    }
}