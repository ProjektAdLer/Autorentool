using AuthoringTool.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
    [Test]
    public void LearningWorldPresenter_CreateNewLearningWorld_CreatesCorrectViewModel()
    {
        var systemUnderTest = new LearningWorldPresenter();
        var name = "cool world";
        var shortname = "cw";
        var authors = "niklas";
        var language = "german";
        var description = "A very cool world";
        var goals = "lots of learning stuff";

        var world = systemUnderTest.CreateNewLearningWorld(name, shortname, authors, language, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo(name));
            Assert.That(world.Shortname, Is.EqualTo(shortname));
            Assert.That(world.Authors, Is.EqualTo(authors));
            Assert.That(world.Language, Is.EqualTo(language));
            Assert.That(world.Description, Is.EqualTo(description));
            Assert.That(world.Goals, Is.EqualTo(goals));
        });
    }

    [Test]
    public void LearningWorldPresenter_EditLearningWorld_EditsViewModelCorrectly()
    {
        var systemUnderTest = new LearningWorldPresenter();
        var world = new LearningWorldViewModel("f", "fa", "a", "u", "e", "o");
        
        var name = "cool world";
        var shortname = "cw";
        var authors = "niklas";
        var language = "german";
        var description = "A very cool world";
        var goals = "lots of learning stuff";

        world = systemUnderTest.EditLearningWorld(world, name, shortname, authors, language, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo(name));
            Assert.That(world.Shortname, Is.EqualTo(shortname));
            Assert.That(world.Authors, Is.EqualTo(authors));
            Assert.That(world.Language, Is.EqualTo(language));
            Assert.That(world.Description, Is.EqualTo(description));
            Assert.That(world.Goals, Is.EqualTo(goals));
        });
    }
}