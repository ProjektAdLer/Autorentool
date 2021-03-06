using System;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolTest.Entities;

[TestFixture]
public class LearningElementUt
{
    [Test]
    public void LearningElement_Constructor_InitializesAllProperties()
    {
        var Name = "asdf";
        var Shortname = "jkl;";
        var ParentName = "foobar";
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var Authors = "ben and jerry";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var Difficulty = LearningElementDifficultyEnum.Medium;
        var Workload = 5;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new LearningElement(Name, Shortname, ParentName, content, Authors, Description, Goals,
             Difficulty, Workload, PositionX, PositionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.ParentName, Is.EqualTo(ParentName));
            Assert.That(systemUnderTest.Content, Is.EqualTo(content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(Difficulty));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(Workload));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
    }
}