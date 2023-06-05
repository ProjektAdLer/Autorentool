using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Shared;
using TestHelpers;

namespace PresentationTest.PresentationLogic.LearningElement;

[TestFixture]
public class LearningElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var parent = ViewModelProvider.GetLearningSpace();
        var content = ViewModelProvider.GetFileContent();
        var description = "very cool element";
        var goals = "learn very many things";
        var workload = 5;
        var points = 6;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.l_h5p_slotmachine_1;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElementViewModel(name, content,
            description, goals, difficulty, elementModel, parent, workload, points, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.ElementModel, Is.EqualTo(elementModel));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
        
    }

    [Test]
    public void FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "aef";
        var systemUnderTest = ViewModelProvider.GetLearningElement();
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }
}