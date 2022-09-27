using NUnit.Framework;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement.TransferElement;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.LearningElement.TransferElement;

[TestFixture]

public class ImageTransferElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "eee";
        var shortname = "fff";
        var parent = new LearningWorldViewModel("ggg", "hhh", "iii", "", "", "");
        var content = new LearningContentViewModel("jjj", "kkk", "");
        var authors = "lll";
        var description = "mmm";
        var goals = "nnn";
        var workload = 15;
        var points = 6;
        var difficulty = LearningElementDifficultyEnum.Medium;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new ImageTransferElementViewModel(name, shortname, parent, content, authors,
            description, goals, difficulty, workload, points, positionX, positionY);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }
}