using NUnit.Framework;

namespace PresentationTest.PresentationLogic.LearningElement.TransferElement;

[TestFixture]

public class ImageTransferElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var Name = "eee";
        var Shortname = "fff";
        var Parent = new LearningWorldViewModel("ggg", "hhh", "iii", "", "", "");
        var Content = new LearningContentViewModel("jjj", "kkk", new byte[] {0x06, 0x03});
        var Authors = "lll";
        var Description = "mmm";
        var Goals = "nnn";
        var Workload = 15;
        var Difficulty = LearningElementDifficultyEnum.Medium;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new ImageTransferElementViewModel(Name, Shortname, Parent, Content, Authors,
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