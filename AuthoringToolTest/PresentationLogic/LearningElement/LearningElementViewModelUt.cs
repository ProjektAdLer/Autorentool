using System;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningWorld;
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
        var Parent = new LearningWorldViewModel("foo", "bar", "baz", "", "", "");
        var Content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var Authors = "ben and jerry";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var Workload = 5;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new LearningElementViewModel(Name, Shortname, Parent, Content, Authors,
            Description, Goals, Workload, PositionX, PositionY);
        
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
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
        
    }

    [Test]
    public void LearningElement_FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "aef";
        var systemUnderTest = new LearningElementViewModel("foo", "foo", null, null,
            "foo",  "foo", "foo");
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }
}