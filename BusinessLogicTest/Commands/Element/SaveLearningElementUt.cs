using BusinessLogic.API;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class SaveLearningElementUt
{
    [Test]
    // ANF-ID: [AWA0017, AWA0012]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var parent = new LearningSpace("a", "d", 4, Theme.CampusAschaffenburg);
        var element = EntityProvider.GetLearningElement(parent: parent);
        const string filepath = "c:\\temp\\test";

        var command =
            new SaveLearningElement(mockBusinessLogic, element, filepath, new NullLogger<SaveLearningElement>());

        command.Execute();

        mockBusinessLogic.Received().SaveLearningElement(element, filepath);
    }
}