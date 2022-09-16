using BusinessLogic.API;
using BusinessLogic.Commands;
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
        var space = new LearningSpace("a", "b", "c", "d", "e", 5);
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningSpace(filepath).Returns(space);
        
        var command = new SaveLearningSpace(mockBusinessLogic, space, filepath);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveLearningSpace(space,filepath);
    }
}