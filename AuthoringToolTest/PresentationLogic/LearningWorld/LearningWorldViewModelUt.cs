using System.Collections.Generic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldViewModelUt
{
    
    [Test]
    public void LearningWorldViewModel_Constructor_InitializesAllProperties()
    {
        var Name = "asdf";
        var Shortname = "jkl;";
        var Authors = "ben and jerry";
        var Language = "german";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var ele1 = new LearningElementViewModel("a", "b", "c", "d", "e", "f", "g", 17, 23);
        var ele2 = new LearningElementViewModel("z", "zz", "z", "z", "zzz", "z", "z", 444, double.MaxValue);
        var LearningElements = new List<LearningElementViewModel> { ele1, ele2 };
        var space1 = new LearningSpaceViewModel("ff", "ff", "ff", "ff", "ff");
        var LearningSpaces = new List<LearningSpaceViewModel> { space1 };

        var systemUnderTest = new LearningWorldViewModel(Name, Shortname, Authors, Language, Description, Goals,
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