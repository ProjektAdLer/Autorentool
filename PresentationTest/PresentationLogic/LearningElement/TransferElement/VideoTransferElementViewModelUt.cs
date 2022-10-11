using NUnit.Framework;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement.TransferElement;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace PresentationTest.PresentationLogic.LearningElement.TransferElement;

[TestFixture]

public class VideoTransferElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "ppp";
        var shortname = "qqq";
        var parent = new LearningSpaceViewModel("rrr", "sss", "ttt", "", "", 2);
        var content = new LearningContentViewModel("uuu", "vvv", new byte[] {0x08, 0x07});
        var url = "url";
        var authors = "www";
        var description = "xxx";
        var goals = "yyy";
        var workload = 11;
        var points = 6;
        var difficulty = LearningElementDifficultyEnum.Hard;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new VideoTransferElementViewModel(name, shortname, parent, content, url, authors,
            description, goals, difficulty, workload, points, positionX, positionY);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Url, Is.EqualTo(url));
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