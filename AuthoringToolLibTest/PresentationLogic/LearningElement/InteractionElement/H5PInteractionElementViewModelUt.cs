using AuthoringToolLib.PresentationLogic.LearningContent;
using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningElement.InteractionElement;
using AuthoringToolLib.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolLibTest.PresentationLogic.LearningElement.InteractionElement;

[TestFixture]

public class H5PInteractionElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var Name = "ccd";
        var Shortname = "dee";
        var Parent = new LearningWorldViewModel("ffg", "ghh", "iij", "", "", "");
        var Content = new LearningContentViewModel("jkk", "llm", new byte[] {0x09, 0x03});
        var Authors = "mnn";
        var Description = "oop";
        var Goals = "pqq";
        var Workload = 20;
        var Difficulty = LearningElementDifficultyEnum.Hard;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new H5PInteractionElementViewModel(Name, Shortname, Parent, Content, Authors,
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