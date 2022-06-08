using System;
using System.Collections.Generic;
using AuthoringTool.PresentationLogic.LearningContent;
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
        var content1 = new LearningContentViewModel("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContentViewModel("z", "e", new byte[]{0x05,0x01});
        var ele1 = new LearningElementViewModel("a", "b", null, content1, "e", "f", "g",17, 23);
        var ele2 = new LearningElementViewModel("z", "zz", null,  content2, "z","zzz", "z", 444, double.MaxValue);
        var LearningElements = new List<LearningElementViewModel> { ele1, ele2 };
        var space1 = new LearningSpaceViewModel("ff", "ff", "ff", "ff", "ff");
        var LearningSpaces = new List<LearningSpaceViewModel> { space1 };

        var systemUnderTest = new LearningWorldViewModel(Name, Shortname, Authors, Language, Description, Goals, 
            unsavedChanges:false, LearningElements, LearningSpaces);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(Language)); 
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.UnsavedChanges, Is.False);
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(LearningElements));
            Assert.That(systemUnderTest.LearningSpaces, Is.EqualTo(LearningSpaces));
        });
    }

    [Test]
    public void LearningWorldViewModel_FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "awf";
        var systemUnderTest = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }
}