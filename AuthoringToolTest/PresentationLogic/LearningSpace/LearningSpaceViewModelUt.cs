using System.Collections.Generic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningSpace;

[TestFixture]
public class LearningSpaceViewModelUt
{
    [Test]
    public void LearningSpace_Constructor_InitializesAllProperties()
    {
        var Name = "asdf";
        var Shortname = "jkl;";
        var Authors = "ben and jerry";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var PositionX = 5f;
        var PositionY = 21f;
        var ele1 = new LearningElementViewModel("a", "b", null,  "e", "f", "g", "h","i", 17, 23);
        var ele2 = new LearningElementViewModel("z", "zz", null,  "zzz", "z", "z","zz","zzz", 444, double.MaxValue);
        var LearningElements = new List<LearningElementViewModel> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpaceViewModel(Name, Shortname, Authors, Description, Goals, LearningElements,
            PositionX, PositionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(LearningElements));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
        
    }
}