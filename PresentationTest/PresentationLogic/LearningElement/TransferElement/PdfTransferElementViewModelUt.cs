using NUnit.Framework;

namespace PresentationTest.PresentationLogic.LearningElement.TransferElement;

[TestFixture]

public class PdfTransferElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var Name = "zzz";
        var Shortname = "aaa";
        var Parent = new LearningWorldViewModel("abb", "bbc", "ccc", "", "", "");
        var Content = new LearningContentViewModel("ddd", "dee", new byte[] {0x02, 0x01});
        var Authors = "eef";
        var Description = "fff";
        var Goals = "ggg";
        var Workload = 14;
        var Difficulty = LearningElementDifficultyEnum.Medium;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new PdfTransferElementViewModel(Name, Shortname, Parent, Content, Authors,
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