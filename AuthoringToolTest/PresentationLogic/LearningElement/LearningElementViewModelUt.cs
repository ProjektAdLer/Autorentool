using AuthoringTool.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningElement;

[TestFixture]
public class LearningElementViewModelUt
{
    [Test]
    public void LearningElement_Constructor_InitializesAllProperties()
    {
        var Name = "asdf";
        var Shortname = "jkl;";
        var Parent = "Learning World";
        var Assignment = "Cool World";
        var Type = "h5p";
        var Content = "foo = bar";
        var Authors = "ben and jerry";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new LearningElementViewModel(Name, Shortname, Parent, Assignment, Type, Content, Authors,
            Description, Goals, PositionX, PositionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(Parent));
            Assert.That(systemUnderTest.Assignment, Is.EqualTo(Assignment));
            Assert.That(systemUnderTest.Type, Is.EqualTo(Type));
            Assert.That(systemUnderTest.Content, Is.EqualTo(Content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
        
    }
}