using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class SaveElementUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var parent  = new Space("a", "b","c","d","e",4);
        var element = new Element("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy,parent);
        const string filepath = "c:\\temp\\test";
        
        var command = new SaveElement(mockBusinessLogic, element, filepath);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveElement(element,filepath);
    }
}