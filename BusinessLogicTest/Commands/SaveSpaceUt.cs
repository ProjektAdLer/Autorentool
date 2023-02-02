using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class SaveSpaceUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new Space("a", "b", "c", "d", "e", 5);
        const string filepath = "c:\\temp\\test";
        
        var command = new SaveSpace(mockBusinessLogic, space, filepath);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveSpace(space,filepath);
    }
}