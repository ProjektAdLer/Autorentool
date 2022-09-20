using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class SaveLearningElementUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var parent  = new LearningWorld("a", "b","c","d","e","f");
        var element = new LearningElement("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy,parent);
        const string filepath = "c:\\temp\\test";
        
        var command = new SaveLearningElement(mockBusinessLogic, element, filepath);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveLearningElement(element,filepath);
    }
}