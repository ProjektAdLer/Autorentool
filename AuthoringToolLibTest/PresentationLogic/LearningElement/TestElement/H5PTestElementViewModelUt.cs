using AuthoringToolLib.PresentationLogic.LearningContent;
using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningElement.TestElement;
using AuthoringToolLib.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolLibTest.PresentationLogic.LearningElement.TestElement;

[TestFixture]

public class H5PTestElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var Name = "rrs";
        var Shortname = "stt";
        var Parent = new LearningWorldViewModel("uuv", "vww", "xxy", "", "", "");
        var Content = new LearningContentViewModel("yzz", "aaa", new byte[] {0x04, 0x05});
        var Authors = "bbb";
        var Description = "ccc";
        var Goals = "ddd";
        var Workload = 20;
        var Difficulty = LearningElementDifficultyEnum.Easy;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new H5PTestElementViewModel(Name, Shortname, Parent, Content, Authors,
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