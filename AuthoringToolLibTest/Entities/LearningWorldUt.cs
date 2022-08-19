using System.Collections.Generic;
using AuthoringToolLib.Entities;
using AuthoringToolLib.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolLibTest.Entities;

[TestFixture]
public class LearningWorldUt
{
    [Test]
    public void LearningWorld_Constructor_InitializesAllProperties()
    {
        var Name = "asdf";
        var Shortname = "jkl;";
        var Authors = "ben and jerry";
        var Language = "german";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h",LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz",LearningElementDifficultyEnum.Medium, 444, double.MaxValue);
        var LearningElements = new List<LearningElement> { ele1, ele2 };
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var LearningSpaces = new List<LearningSpace> { space1 };

        var systemUnderTest = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            LearningElements, LearningSpaces);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(Language)); 
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(LearningElements));
            Assert.That(systemUnderTest.LearningSpaces, Is.EqualTo(LearningSpaces));
        });
    }
}