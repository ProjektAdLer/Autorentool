using BusinessLogic.API;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Space;

[TestFixture]
public class SaveLearningSpaceUt
{
    [Test]
    // ANF-ID: [AWA0025]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "d", 5, Theme.CampusAschaffenburg);
        const string filepath = "c:\\temp\\test";

        var command = new SaveLearningSpace(mockBusinessLogic, space, filepath, new NullLogger<SaveLearningSpace>());

        command.Execute();

        mockBusinessLogic.Received().SaveLearningSpace(space, filepath);
    }
}