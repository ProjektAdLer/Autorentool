using AuthoringTool.PresentationLogic.LearningSpace;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningSpace;

[TestFixture]

public class LearningSpacePresenterUt
{
    [Test]
    public void LearningSpacePresenter_CreateNewLearningSpace_CreatesCorrectViewModel()
    {
        var systemUnderTest = new LearningSpacePresenter();
        var name = "a";
        var shortname = "b";
        var authors = "d";
        var description = "e";
        var goals = "f";

        var space = systemUnderTest.CreateNewLearningSpace(name, shortname, authors, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo(name));
            Assert.That(space.Shortname, Is.EqualTo(shortname));
            Assert.That(space.Authors, Is.EqualTo(authors));
            Assert.That(space.Description, Is.EqualTo(description));
            Assert.That(space.Goals, Is.EqualTo(goals));
        });
    }
    
    [Test]
    public void LearningSpacePresenter_EditLearningSpace_EditsViewModelCorrectly()
    {
        var systemUnderTest = new LearningSpacePresenter();
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        
        var name = "new space";
        var shortname = "ns";
        var authors = "marvin";
        var description = "space with learning stuff";
        var goals = "learn";

        space = systemUnderTest.EditLearningSpace(space, name, shortname, authors, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo(name));
            Assert.That(space.Shortname, Is.EqualTo(shortname));
            Assert.That(space.Authors, Is.EqualTo(authors));
            Assert.That(space.Description, Is.EqualTo(description));
            Assert.That(space.Goals, Is.EqualTo(goals));
        });
    }
}