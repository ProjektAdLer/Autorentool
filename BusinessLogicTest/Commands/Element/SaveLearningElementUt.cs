using BusinessLogic.API;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]

public class SaveLearningElementUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var parent  = new LearningSpace("a","d","e",4, Theme.Campus);
        var element = EntityProvider.GetLearningElement(parent: parent);
        const string filepath = "c:\\temp\\test";
        var logger = Substitute.For<ILogger<ElementCommandFactory>>();
        
        var command = new SaveLearningElement(mockBusinessLogic, element, filepath, logger);
        
        command.Execute();
        
        mockBusinessLogic.Received().SaveLearningElement(element,filepath);
    }
}