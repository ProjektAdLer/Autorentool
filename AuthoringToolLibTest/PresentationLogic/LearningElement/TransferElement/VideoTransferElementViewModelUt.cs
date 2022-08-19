using AuthoringToolLib.PresentationLogic.LearningContent;
using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningElement.TransferElement;
using AuthoringToolLib.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolLibTest.PresentationLogic.LearningElement.TransferElement;

[TestFixture]

public class VideoTransferElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var Name = "ppp";
        var Shortname = "qqq";
        var Parent = new LearningWorldViewModel("rrr", "sss", "ttt", "", "", "");
        var Content = new LearningContentViewModel("uuu", "vvv", new byte[] {0x08, 0x07});
        var Authors = "www";
        var Description = "xxx";
        var Goals = "yyy";
        var Workload = 11;
        var Difficulty = LearningElementDifficultyEnum.Hard;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new VideoTransferElementViewModel(Name, Shortname, Parent, Content, Authors,
            Description, Goals, Difficulty, Workload, PositionX, PositionY);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(Parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(Content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(Workload));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(Difficulty));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
    }
}